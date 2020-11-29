using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Utilities;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        internal Dictionary<Func<Message, bool>, TaskCompletionSource<Message>> currentMessageSubscriptions
           = new Dictionary<Func<Message, bool>, TaskCompletionSource<Message>>();

        internal async Task<Response<MessageResponse>> SendMessageAsync(int recipient, object content, MessageType messageType)
        {
            return await WolfClient.Emit<MessageResponse>(Request.MESSAGE_SEND, new
            {
                recipient,
                isGroup = messageType == MessageType.Group,
                mimeType = content.GetType() == typeof(Bitmap) || content.GetType() == typeof(Image) ? "image/jpeg" : "text/plain",
                data = content.GetType() == typeof(Bitmap) || content.GetType() == typeof(Image) ? ((Bitmap)content).ToBytes() : Encoding.UTF8.GetBytes(content.ToString()),
                flightId = Guid.NewGuid()
            });
        }

        public async Task<Response<MessageResponse>> SendGroupMessageAsync(int groupId, object content)
        {
            return await SendMessageAsync(groupId, content, MessageType.Group);
        }

        public async Task<Response<MessageResponse>> SendPrivateMessageAsync(int subscriberId, object content)
        {
            return await SendMessageAsync(subscriberId, content, MessageType.Private);
        }

        internal async Task<Response> GroupMessageSubscribeAsync()
        {
            return await WolfClient.Emit(Request.MESSAGE_GROUP_SUBSCRIBE, new
            {
                headers = new
                {
                    version = 3
                }
            });
        }

        internal async Task<Response> GroupMessageUnsubscribeAsync(int groupId)
        {
            return await WolfClient.Emit(Request.MESSAGE_GROUP_SUBSCRIBE, new
            {
                headers = new
                {
                    version = 3
                },
                body = new
                {
                    id = groupId
                }
            });
        }

        internal async Task<Response> PrivateMessageSubscribeAsync()
        {
            return await WolfClient.Emit(Request.MESSAGE_PRIVATE_SUBSCRIBE, new
            {
                headers = new
                {
                    version = 3
                }
            });
        }

        public async Task<Response<Message>> DeleteMessageAsync(int targetId, long targetTimestamp, bool isGroup = true)
        {
            var result = await WolfClient.Emit<BaseMessage>(Request.MESSAGE_UPDATE, new
            {
                isGroup,
                metadata = new
                {
                    isDeleted = true
                },
                recipientId = targetId,
                timestamp = targetTimestamp
            });

            if (result.Success)
                return new Response<Message>() { Code = 200, Body = result.Body.ToNormalMessage() };

            return new Response<Message>() { Code = result.Code, Body = default, Headers = result.Headers };
        }

        public async Task<Response<Message>> DeleteMessageAsync(Message message)
        {
            return await DeleteMessageAsync(message.SourceTargetId, message.Timestamp, message.IsGroup);
        }

        public async Task<Response<Message>> RestoreMessageAsync(int targetId, long targetTimestamp, bool isGroup = true)
        {
            var result = await WolfClient.Emit<BaseMessage>(Request.MESSAGE_UPDATE, new
            {
                isGroup,
                metadata = new
                {
                    isDeleted = false
                },
                recipientId = targetId,
                timestamp = targetTimestamp
            });

            if (result.Success)
                return new Response<Message>() { Code = 200, Body = result.Body.ToNormalMessage() };

            return new Response<Message>() { Code = result.Code, Body = default, Headers = result.Headers };
        }

        public async Task<Response<Message>> RestoreMessageAsync(Message message)
        {
            return await RestoreMessageAsync(message.SourceTargetId, message.Timestamp, message.IsGroup);
        }

        public async Task<Message> SubscribeToNextMessageAsync(Func<Message, bool> func)
        {
            try
            {
                var task = new TaskCompletionSource<Message>();

                if (!currentMessageSubscriptions.TryAdd(func, task))
                    return null;

                return await currentMessageSubscriptions[func].Task;
            }
            catch (TimeoutException)
            {
                currentMessageSubscriptions.Remove(func);

                return null;
            }
        }

        public async Task<Message> SubscribeToNextGroupMessageAsync(int groupId, double timeout = Timeout.Infinite)
        {
            return await SubscribeToNextMessageAsync(r => r.MessageType == MessageType.Group && r.SourceTargetId == groupId).TimeoutAfter(timeout);
        }

        public async Task<Message> SubscribeToNextPrivateMessageAsync(int userId, double timeout = Timeout.Infinite)
        {
            return await SubscribeToNextMessageAsync(r => r.MessageType == MessageType.Private && r.SourceSubscriberId == userId).TimeoutAfter(timeout);
        }

        public async Task<Message> SubscribeToNextGroupUserMessageAsync(int userId, int groupId, double timeout = Timeout.Infinite)
        {
            return await SubscribeToNextMessageAsync(r => r.MessageType == MessageType.Group && r.SourceTargetId == groupId && r.SourceSubscriberId == userId).TimeoutAfter(timeout);
        }

        public async Task<Response<LinkMetadata>> LinkMetadataAsync(Uri uri)
        {
            return await LinkMetadataAsync(uri.ToString());
        }

        public async Task<Response<LinkMetadata>> LinkMetadataAsync(string url)
        {
            return await WolfClient.Emit<LinkMetadata>(Request.METADATA_URL, new
            {
                url
            });
        }

        public async Task<Response<List<Message>>> GetGroupMessageHistoryAsync(int groupId, long timestamp = 0)
        {
            var result = await WolfClient.Emit<List<BaseMessage>>(Request.MESSAGE_GROUP_HISTORY_LIST, new
            {
                headers = new
                {
                    version = 3
                },
                body = new
                {
                    id = groupId,
                    chronological = false,
                    timestampBegin = timestamp != 0,
                    timestampEnd = timestamp == 0 ? null : timestamp.ToString()
                }
            });

            return new Response<List<Message>>()
            {
                Code = result.Code,
                Body = result.Body.Select(r => r.ToNormalMessage()).ToList(),
                Headers = result.Headers
            };
        }

        public async Task<Response<List<Message>>> GetGroupMessageHistoryAsync(Message message, long timestamp)
        {
            if (message.MessageType != MessageType.Group)
                return new Response<List<Message>>() { Code = 400, Body = default, Headers = new Dictionary<string, string>() { { "error", "You cannot request group message history from a private message" } } };

            return await GetGroupMessageHistoryAsync(message.SourceTargetId, timestamp);
        }

        public async Task<Response<List<Message>>> GetPrivateMessageHistoryAsync(int subscriberId, long timestamp = 0)
        {
            var result = await WolfClient.Emit<List<BaseMessage>>(Request.MESSAGE_PRIVATE_HISTORY_LIST, new
            {
                headers = new
                {
                    version = 2
                },
                body = new
                {
                    id = subscriberId,
                    timestampEnd = timestamp == 0 ? null : timestamp.ToString()
                }
            });

            return new Response<List<Message>>()
            {
                Code = result.Code,
                Body = result.Body.Select(r => r.ToNormalMessage()).ToList(),
                Headers = result.Headers
            };
        }

        public async Task<Response<List<Message>>> GetPrivateMessageHistoryAsync(Message message, long timestamp = 0)
        {
            if (message.MessageType != MessageType.Private)
                return new Response<List<Message>>() { Code = 400, Body = default, Headers = new Dictionary<string, string>() { { "error", "You cannot request private message history from a group message" } } };

            return await GetPrivateMessageHistoryAsync(message.SourceTargetId, timestamp);
        }
    }
}