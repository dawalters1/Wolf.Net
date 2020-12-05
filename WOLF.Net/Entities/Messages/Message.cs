using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Entities.API;
using WOLF.Net.Enums.Messages;

namespace WOLF.Net.Entities.Messages
{
    public class Message
    {
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

        public Guid Id { get;set; }

        public string Content { get;set; }

        [Obsolete("This property will be deprecated soon, please use SourceSubscriberId")]
        public int UserId => SourceSubscriberId;

        public int SourceSubscriberId { get;set; }

        [Obsolete("This property will be deprecated soon, please use SourceTargetId")]
        public int ReturnAddress => SourceTargetId;

        public int SourceTargetId { get; set; }

        /// <summary>
        /// Contains the timestamp and User id of the account that edited or deleted the message
        /// </summary>
        public MessageEdit Edited { get;set; }

        /// <summary>
        /// Contains the IsSpam, IsDeleted, IsEdited data (Normally Null)
        /// </summary>
        public MessageMetadata Metadata { get;set; }

        public MessageType MessageType { get;set; }

        public ContentType ContentType { get;set; }

        public string FlightId { get;set; }

        public long Timestamp { get;set; }

        public bool IsCommand { get; set; }

        #region

        /// <summary>
        /// The message is a private message
        /// </summary>
        public bool IsGroup => MessageType != MessageType.Private;

        #endregion

        public async Task<Response<MessageResponse>> SendMessageAsync(object content)
        {

            if (IsGroup)
                return await Bot.SendGroupMessageAsync(SourceTargetId, content);
            return await Bot.SendPrivateMessageAsync(SourceSubscriberId, content);
        }
    }
}
