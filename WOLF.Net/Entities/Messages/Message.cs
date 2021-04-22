using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Tipping;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Utilities;

namespace WOLF.Net.Entities.Messages
{
    public class Message
    {
        [JsonIgnore]
        private WolfBot Bot;

        internal Message(WolfBot bot, BaseMessage message)
        {
            this.Bot = bot;
            this.Id = message.Id;
            this.Content = Encoding.UTF8.GetString(message.Data);
            this.SourceSubscriberId = message.Originator.Id;
            this.SourceTargetId = message.IsGroup ? message.Recipient.Id : message.Originator.Id;
            this.Edited = message.Edited;
            this.Metadata = message.Metadata;
            this.MessageType = message.IsGroup ? MessageType.Group : MessageType.Private;
            this.ContentType = message.ContentType;
            this.Timestamp = message.Timestamp;
        }

        public Guid Id { get; set; }

        public string Content { get; set; }

        [Obsolete("This property will be deprecated soon, please use SourceSubscriberId", true)]
        public int UserId => SourceSubscriberId;

        public int SourceSubscriberId { get; set; }

        [Obsolete("This property will be deprecated soon, please use SourceTargetId", true)]
        public int ReturnAddress => SourceTargetId;

        public int SourceTargetId { get; set; }

        /// <summary>
        /// Contains the timestamp and User id of the account that edited or deleted the message
        /// </summary>
        public MessageEdit Edited { get; set; }

        /// <summary>
        /// Contains the IsSpam, IsDeleted, IsEdited data (Normally Null)
        /// </summary>
        public MessageMetadata Metadata { get; set; }

        public MessageType MessageType { get; set; }

        public ContentType ContentType { get; set; }

        public string FlightId { get; set; }

        public long Timestamp { get; set; }

        public bool IsCommand => Bot.CommandManager.Commands.Any(r => Bot.UsingTranslations ? Bot.GetAllPhrasesByName(r.Value.Trigger).Any(s => Content.StartsWithCommand(s.Value)) : Content.StartsWith(r.Value.Trigger)) || Bot.FormManager.Forms.Any(r => Bot.UsingTranslations ? Bot.GetAllPhrasesByName(r.Value.Trigger).Any(s => Content.StartsWithCommand(s.Value)) : Content.StartsWith(r.Value.Trigger));

        #region

        /// <summary>
        /// The message is a private message
        /// </summary>
        public bool IsGroup => MessageType != MessageType.Private;

        #endregion

        public async Task<Response<MessageResponse>> SendMessageAsync(object content, bool includeEmbeds = false)
        {

            if (IsGroup)
                return await Bot.SendGroupMessageAsync(SourceTargetId, content, includeEmbeds);
            return await Bot.SendPrivateMessageAsync(SourceSubscriberId, content, includeEmbeds);
        }

        public async Task<Response<Message>> DeleteAsync() => await Bot.DeleteMessageAsync(this);

        public async Task<Response<Message>> RestoreAsync() => await Bot.RestoreMessageAsync(this);

        public async Task<Response> Tip(params TipCharm[] charms) => await Bot.AddTip(this, charms);

    }
}
