using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Messages;

namespace WOLF.Net.Entities.Messages
{
    public class Message : IMessage
    {
        internal Message(WolfBot bot, BaseMessage message)
        {
            this.Bot = bot;
            this.Id = message.Id;
            this.Content = Encoding.UTF8.GetString(message.Data);
            this.UserId = message.Originator.Id;
            this.ReturnAddress = message.IsGroup ? message.Recipient.Id : message.Originator.Id;
            this.Edited = message.Edited;
            this.Metadata = message.Metadata;
            this.MessageType = message.IsGroup ? MessageType.Group : MessageType.Private;
            this.ContentType = message.ContentType;
            this.Timestamp = message.Timestamp;
        }

        /// <summary>
        /// Interal use only
        /// </summary>
        private WolfBot Bot { get; set; }

        public Guid Id { get; private set; }

        public string Content { get; private set; }

        public int UserId { get; private set; }

        public int? GroupId => MessageType == MessageType.Group ? (int?)ReturnAddress : null;

        public int ReturnAddress { get; private set; }

        /// <summary>
        /// Contains the timestamp and User id of the account that edited or deleted the message
        /// </summary>
        public MessageEdit Edited { get; private set; }

        /// <summary>
        /// Contains the IsSpam, IsDeleted, IsEdited data (Normally Null)
        /// </summary>
        public MessageMetadata Metadata { get; private set; }

        public MessageType MessageType { get; private set; }

        public ContentType ContentType { get; private set; }

        public string FlightId { get; private set; }

        public long Timestamp { get; private set; }

        #region

        /// <summary>
        /// The message is a private message
        /// </summary>
        public bool IsGroup => MessageType != MessageType.Private;

        #endregion
    }
}
