using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Networking;
using System.Linq;
using System.Drawing;
using WOLF.Net.Utils;
using System.Dynamic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace WOLF.Net.Helper
{
    public class MessagingHelper : BaseHelper<Message>
    {
        internal Dictionary<Func<Message, bool>, TaskCompletionSource<Message>> _currentMessageSubscriptions
                 = new Dictionary<Func<Message, bool>, TaskCompletionSource<Message>>();

        internal bool PropertyExists(dynamic obj, string name)
        {
            Type objType = obj.GetType();

            if (objType == typeof(ExpandoObject))
            {
                return ((IDictionary<string, object>)obj).ContainsKey(name);
            }

            return objType.GetProperty(name) != null;
        }

        /// <summary>
        /// Cancerous, but dont care it works
        /// </summary>
        /// <param name="body"></param>
        /// <param name="content"></param>
        /// <param name="includeEmbeds"></param>
        /// <returns></returns>
        internal async Task<object> GetFormattingData(dynamic body, string content, bool includeEmbeds)
        {
            dynamic formatting = new ExpandoObject();

            var groupLinks = new List<dynamic>();


            foreach (Match result in Regex.Matches(content, @"\[.*?\]"))
            {
                dynamic link = new ExpandoObject();
                link.start = content.IndexOf(result.Value);
                link.end = content.IndexOf(result.Value) + result.Value.Length - 1;
                link.name = result.Value.TrimStart('[').TrimEnd(']');

                var group = await Bot.Group().GetByNameAsync((string)link.name);

                if (group.Exists)
                    link.groupId = group.Id;

                groupLinks.Add(link);
            };

            var links = Regex.Matches(content, @"(\b(http|ftp|https):(\/\/|\\\\)[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?|\bwww\.[^\s])").Select((result) =>
            new
            {
                start = content.IndexOf(result.Value),
                end = content.IndexOf(result.Value) + result.Value.Length - 1,
                value = result.Value
            }).ToList();

            if (groupLinks.Count > 0 || links.Count > 0)
            {
                var data = new List<dynamic>();

                if (groupLinks.Count > 0)
                {
                    data.AddRange(groupLinks);

                    formatting.groupLinks = groupLinks.Select((link) =>
                    {
                        dynamic fixedLink = new ExpandoObject();
                        fixedLink.start = link.start;
                        fixedLink.end = link.end;

                        if (PropertyExists(link, "groupId"))
                            fixedLink.groupId = link.groupId;

                        return fixedLink;
                    }).ToList();
                }
                if (links.Count > 0)
                {
                    data.AddRange(links);
                    formatting.links = links.Select((link) => new { link.start, link.end, url = link.value }).ToList();
                }
                body.metadata = new
                {
                    formatting
                };

                if (includeEmbeds && data.Count > 0)
                {
                    var embeds = new List<object>();

                    foreach (dynamic link in data.OrderBy((link) => (int)link.start).ToList())
                    {
                        if (PropertyExists(link, "name") && PropertyExists(link, "groupId"))
                        {
                            embeds.Add(new
                            {
                                type = "groupPreview",
                                link.groupId
                            });

                            continue;
                        }
                        else
                        {
                            var metadata = await Bot.Messaging().LinkMetadataAsync((string)link.value);

                            if (metadata.Success && !metadata.Body.IsBlackListed)
                            {
                                embeds.Add(new
                                {
                                    type = metadata.Body.ImageSize > 0 ? "imagePreview" : "linkPreview",
                                    url = (string)link.value,
                                    image = metadata.Body.ImageSize > 0 || string.IsNullOrWhiteSpace(metadata.Body.ImageUrl) ? null : (await Public.DownloadImageFromUrl(metadata.Body.ImageUrl)).ToBytes(),
                                    title = metadata.Body.Title,
                                    body = metadata.Body.Description
                                });

                                break;
                            }
                        }
                    }

                    if (embeds.Count > 0)
                        body.embeds = embeds;
                }
            }
            return body;
        }

        internal async Task<Response<MessageResponse>> SendMessageAsync(int recipient, object content, MessageType messageType, bool includeEmbeds = false)
        {
            bool isImage = content.GetType() == typeof(Bitmap);

            dynamic body = new ExpandoObject();

            body.recipient = recipient;
            body.isGroup = messageType == MessageType.GROUP;
            body.mimeType = isImage ? "image/jpeg" : "text/plain";
            body.data = !isImage ? Encoding.UTF8.GetBytes(content.ToString()) : ((Bitmap)content).ToBytes();
            body.flightId = Guid.NewGuid();

            return await WebSocket.Emit<Response<MessageResponse>>(Request.MESSAGE_SEND, isImage ? body : await GetFormattingData(body, content.ToString(), includeEmbeds));
        }

        public async Task<Response<MessageResponse>> SendGroupMessageAsync(int groupId, object content, bool includeEmbeds = false) => await SendMessageAsync(groupId, content, MessageType.GROUP, includeEmbeds);

        public async Task<Response<MessageResponse>> SendPrivateMessageAsync(int subscriberId, object content, bool includeEmbeds = false) => await SendMessageAsync(subscriberId, content, MessageType.PRIVATE, includeEmbeds);

        internal async Task<Response> GroupSubscribeAsync() => await WebSocket.Emit<Response>(Request.MESSAGE_GROUP_SUBSCRIBE, new { headers = new { version = 3 } });

        internal async Task<Response> GroupUnsubscribeAsync(int groupId) => await WebSocket.Emit<Response>(Request.MESSAGE_GROUP_SUBSCRIBE, new { headers = new { version = 3 }, body = new { id = groupId } });

        internal async Task<Response> PrivateSubscribeAsync() => await WebSocket.Emit<Response>(Request.MESSAGE_PRIVATE_SUBSCRIBE, new { headers = new { version = 2 } });

        public async Task<Response<Message>> DeleteAsync(int targetId, long targetTimestamp, bool isGroup = true)
        {
            var result = await WebSocket.Emit<Response<BaseMessage>>(Request.MESSAGE_UPDATE, new
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
                return new Response<Message>() { Code = 200, Body = result.Body.NormalizeMessage(Bot) };

            return new Response<Message>() { Code = result.Code, Body = default, Headers = result.Headers };
        }

        public async Task<Response<Message>> DeleteAsync(Message message) => await DeleteAsync(message.TargetGroupId, message.Timestamp, message.IsGroup);

        public async Task<Response<Message>> RestoreAsync(int targetId, long targetTimestamp, bool isGroup = true)
        {
            var result = await WebSocket.Emit<Response<BaseMessage>>(Request.MESSAGE_UPDATE, new
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
                return new Response<Message>() { Code = 200, Body = result.Body.NormalizeMessage(Bot) };

            return new Response<Message>() { Code = result.Code, Body = default, Headers = result.Headers };
        }

        public async Task<Response<Message>> RestoreAsync(Message message) => await RestoreAsync(message.TargetGroupId, message.Timestamp, message.IsGroup);

        public async Task<Message> SubscribeToNextMessageAsync(Func<Message, bool> func)
        {
            try
            {
                var task = new TaskCompletionSource<Message>();

                if (_currentMessageSubscriptions.TryAdd(func, task))
                    return await _currentMessageSubscriptions[func].Task;
                return null;
            }
            catch (TimeoutException)
            {
                _currentMessageSubscriptions.Remove(func);

                return null;
            }
        }

        public async Task<Message> SubscribeToNextGroupMessageAsync(int groupId, double timeout = Timeout.Infinite) => await SubscribeToNextMessageAsync(r => r.MessageType == MessageType.GROUP && r.TargetGroupId == groupId).TimeoutAfter(timeout);

        public async Task<Message> SubscribeToNextPrivateMessageAsync(int userId, double timeout = Timeout.Infinite) => await SubscribeToNextMessageAsync(r => r.MessageType == MessageType.PRIVATE && r.SourceSubscriberId == userId).TimeoutAfter(timeout);

        public async Task<Message> SubscribeToNextGroupUserMessageAsync(int userId, int groupId, double timeout = Timeout.Infinite) => await SubscribeToNextMessageAsync(r => r.MessageType == MessageType.GROUP && r.TargetGroupId == groupId && r.SourceSubscriberId == userId).TimeoutAfter(timeout);

        public async Task<Response<LinkMetadata>> LinkMetadataAsync(Uri uri) => await LinkMetadataAsync(uri.ToString());

        public async Task<Response<LinkMetadata>> LinkMetadataAsync(string url) => await WebSocket.Emit<Response<LinkMetadata>>(Request.METADATA_URL, new { url });

        public async Task<Response<List<Message>>> GetGroupHistoryAsync(int groupId, long timestamp = 0)
        {
            var result = await WebSocket.Emit<Response<List<BaseMessage>>>(Request.MESSAGE_GROUP_HISTORY_LIST, new
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
                Body = result.Body.Select(baseMessage => baseMessage.NormalizeMessage(Bot)).ToList(),
                Headers = result.Headers
            };
        }

        public async Task<Response<List<Message>>> GetGroupHistoryAsync(Message message)
        {
            if (message.MessageType != MessageType.GROUP)
                return new Response<List<Message>>() { Code = 400, Body = default, Headers = new Dictionary<string, string>() { { "error", "You cannot request group message history from a private message" } } };

            return await GetGroupHistoryAsync(message.TargetGroupId, message.Timestamp);
        }

        public async Task<Response<List<Message>>> GetPrivateHistoryAsync(int subscriberId, long timestamp = 0)
        {
            var result = await WebSocket.Emit<Response<List<BaseMessage>>>(Request.MESSAGE_PRIVATE_HISTORY_LIST, new
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
                Body = result.Body.Select(baseMessage => baseMessage.NormalizeMessage(Bot)).ToList(),
                Headers = result.Headers
            };
        }

        public async Task<Response<List<Message>>> GetPrivateHistoryAsync(Message message, long timestamp = 0)
        {
            if (message.MessageType != MessageType.PRIVATE)
                return new Response<List<Message>>() { Code = 400, Body = default, Headers = new Dictionary<string, string>() { { "error", "You cannot request private message history from a group message" } } };

            return await GetPrivateHistoryAsync(message.TargetGroupId, timestamp);
        }

        internal MessagingHelper(WolfBot Bot, WebSocket WebSocket) : base(Bot, WebSocket) { }
    }
}
