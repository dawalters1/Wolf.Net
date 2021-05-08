using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Utils;

namespace WOLF.Net
{
    public partial class WolfBot
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
        internal async Task<object> GetFormattingDataAsync(dynamic body, string content, bool includeEmbeds)
        {
            dynamic formatting = new ExpandoObject();

            var ads = await this.GetGroupAdsFromMessageAsync(content);

            var urls = this.GetLinksFromMessageAsync(content);

            if (ads.Count > 0 || urls.Count > 0)
            {

                if (ads.Count > 0)
                    formatting.groupLinks = ads;
                if (urls.Count > 0)
                    formatting.links = urls;

                if (PropertyExists(formatting, "groupLinks") || PropertyExists(formatting, "links"))
                {
                    body.metadata = new
                    {
                        formatting
                    };

                    if (includeEmbeds)
                    {
                        var data = ads.Concat(urls);

                        if (data.Any())
                        {
                            var embeds = new List<object>();

                            foreach (dynamic link in data.OrderBy((link) => (int)link.start).ToList())
                            {
                                if (PropertyExists(link, "url"))
                                {
                                    var metadata = await LinkMetadataAsync((string)link.url);

                                    if (metadata.Success && !metadata.Body.IsBlackListed)
                                    {
                                        embeds.Add(new
                                        {
                                            type = metadata.Body.ImageSize > 0 ? "imagePreview" : "linkPreview",
                                            url = (string)link.url,
                                            image = metadata.Body.ImageSize > 0 || string.IsNullOrWhiteSpace(metadata.Body.ImageUrl) ? null : (await Public.DownloadImageFromUrl(metadata.Body.ImageUrl)).ToBytes(),
                                            title = metadata.Body.Title,
                                            body = metadata.Body.Description
                                        });

                                        break;
                                    }
                                }
                                else if (PropertyExists(link, "groupId"))
                                {
                                    embeds.Add(new
                                    {
                                        type = "groupPreview",
                                        link.groupId
                                    });

                                    continue;
                                }
                            }

                            if (embeds.Count > 0)
                                body.embeds = embeds;
                        }
                    }
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

            return await _webSocket.Emit<Response<MessageResponse>>(Request.MESSAGE_SEND, isImage ? body : await GetFormattingDataAsync(body, content.ToString(), includeEmbeds));
        }

        /// <summary>
        /// Send a group message
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="content"></param>
        /// <param name="includeEmbeds"></param>
        /// <returns>Response<MessageResponse></returns>
        public async Task<Response<MessageResponse>> SendGroupMessageAsync(int groupId, object content, bool includeEmbeds = false) => await SendMessageAsync(groupId, content, MessageType.GROUP, includeEmbeds);

        /// <summary>
        /// Send a private message
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="content"></param>
        /// <param name="includeEmbeds"></param>
        /// <returns>Response<MessageResponse></returns>
        public async Task<Response<MessageResponse>> SendPrivateMessageAsync(int subscriberId, object content, bool includeEmbeds = false) => await SendMessageAsync(subscriberId, content, MessageType.PRIVATE, includeEmbeds);

        internal async Task<Response> GroupSubscribeAsync() => await _webSocket.Emit<Response>(Request.MESSAGE_GROUP_SUBSCRIBE, new { headers = new { version = 3 } });

        internal async Task<Response> GroupUnsubscribeAsync(int groupId) => await _webSocket.Emit<Response>(Request.MESSAGE_GROUP_SUBSCRIBE, new { headers = new { version = 3 }, body = new { id = groupId } });

        internal async Task<Response> PrivateSubscribeAsync() => await _webSocket.Emit<Response>(Request.MESSAGE_PRIVATE_SUBSCRIBE, new { headers = new { version = 2 } });

        /// <summary>
        /// Delete a message
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="targetTimestamp"></param>
        /// <param name="isGroup"></param>
        /// <returns>Response<Message></returns>
        public async Task<Response<Message>> DeleteMessageAsync(int targetId, long targetTimestamp, bool isGroup = true)
        {
            var result = await _webSocket.Emit<Response<BaseMessage>>(Request.MESSAGE_UPDATE, new
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
                return new Response<Message>() { Code = 200, Body = result.Body.NormalizeMessage(this) };

            return new Response<Message>() { Code = result.Code, Body = default, Headers = result.Headers };
        }

        /// <summary>
        /// Delete a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Response<Message></returns>
        public async Task<Response<Message>> DeleteMessageAsync(Message message) => await DeleteMessageAsync(message.TargetGroupId, message.Timestamp, message.IsGroup);

        /// <summary>
        /// Restore a message
        /// </summary>
        /// <param name="targetId"></param>
        /// <param name="targetTimestamp"></param>
        /// <param name="isGroup"></param>
        /// <returns>Response<Message></returns>
        public async Task<Response<Message>> RestoreMessageAsync(int targetId, long targetTimestamp, bool isGroup = true)
        {
            var result = await _webSocket.Emit<Response<BaseMessage>>(Request.MESSAGE_UPDATE, new
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
                return new Response<Message>() { Code = 200, Body = result.Body.NormalizeMessage(this) };

            return new Response<Message>() { Code = result.Code, Body = default, Headers = result.Headers };
        }

        /// <summary>
        /// Restore a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Response<Message></returns>
        public async Task<Response<Message>> RestoreMessageAsync(Message message) => await RestoreMessageAsync(message.TargetGroupId, message.Timestamp, message.IsGroup);

        /// <summary>
        /// Subscribe to the next message the bot receives
        /// </summary>
        /// <param name="func"></param>
        /// <returns>Message</returns>
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

        /// <summary>
        /// subscribe to the next message in a group
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="timeout"></param>
        /// <returns>Message</returns>
        public async Task<Message> SubscribeToNextGroupMessageAsync(int groupId, double timeout = Timeout.Infinite) => await SubscribeToNextMessageAsync(r => r.MessageType == MessageType.GROUP && r.TargetGroupId == groupId).TimeoutAfter(timeout);

        /// <summary>
        /// Subscribe to the next message in a private conversation
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="timeout"></param>
        /// <returns>Message</returns>
        public async Task<Message> SubscribeToNextPrivateMessageAsync(int subscriberId, double timeout = Timeout.Infinite) => await SubscribeToNextMessageAsync(r => r.MessageType == MessageType.PRIVATE && r.SourceSubscriberId == subscriberId).TimeoutAfter(timeout);

        /// <summary>
        /// Subscribe to the next message in a group by a specific user
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="groupId"></param>
        /// <param name="timeout"></param>
        /// <returns>Message</returns>
        public async Task<Message> SubscribeToNextGroupUserMessageAsync(int subscriberId, int groupId, double timeout = Timeout.Infinite) => await SubscribeToNextMessageAsync(r => r.MessageType == MessageType.GROUP && r.TargetGroupId == groupId && r.SourceSubscriberId == subscriberId).TimeoutAfter(timeout);

        /// <summary>
        /// Request information about a link
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>Response<LinkMetadata></returns>
        public async Task<Response<LinkMetadata>> LinkMetadataAsync(Uri uri) => await LinkMetadataAsync(uri.ToString());
        /// <summary>
        /// Request information about a link
        /// </summary>
        /// <param name="url"></param>
        /// <returns>Response<LinkMetadata></returns>
        public async Task<Response<LinkMetadata>> LinkMetadataAsync(string url) => await _webSocket.Emit<Response<LinkMetadata>>(Request.METADATA_URL, new { url });

        /// <summary>
        /// Request group message history
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="timestamp">First message timestamp</param>
        /// <returns>Response<List<Message>></returns>
        public async Task<Response<List<Message>>> GetGroupHistoryAsync(int groupId, long timestamp = 0)
        {
            var result = await _webSocket.Emit<Response<List<BaseMessage>>>(Request.MESSAGE_GROUP_HISTORY_LIST, new
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
                Body = result.Body.Select(baseMessage => baseMessage.NormalizeMessage(this)).ToList(),
                Headers = result.Headers
            };
        }

        /// <summary>
        /// Request group message history by message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Response<List<Message>></returns>
        public async Task<Response<List<Message>>> GetGroupHistoryAsync(Message message)
        {
            if (message.MessageType != MessageType.GROUP)
                return new Response<List<Message>>() { Code = 400, Body = default, Headers = new Dictionary<string, string>() { { "error", "You cannot request group message history from a private message" } } };

            return await GetGroupHistoryAsync(message.TargetGroupId, message.Timestamp);
        }
        /// <summary>
        /// Request subscriber message history
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="timestamp">First message timestamp</param>
        /// <returns>Response<List<Message>></returns>
        public async Task<Response<List<Message>>> GetPrivateHistoryAsync(int subscriberId, long timestamp = 0)
        {
            var result = await _webSocket.Emit<Response<List<BaseMessage>>>(Request.MESSAGE_PRIVATE_HISTORY_LIST, new
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
                Body = result.Body.Select(baseMessage => baseMessage.NormalizeMessage(this)).ToList(),
                Headers = result.Headers
            };
        }

        /// <summary>
        /// Request subscriber message history by message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Response<List<Message>></returns>
        public async Task<Response<List<Message>>> GetPrivateHistoryAsync(Message message, long timestamp = 0)
        {
            if (message.MessageType != MessageType.PRIVATE)
                return new Response<List<Message>>() { Code = 400, Body = default, Headers = new Dictionary<string, string>() { { "error", "You cannot request private message history from a group message" } } };

            return await GetPrivateHistoryAsync(message.TargetGroupId, timestamp);
        }
    }
}