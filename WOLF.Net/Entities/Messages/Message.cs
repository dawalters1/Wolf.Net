using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Tip;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Utils;

namespace WOLF.Net.Entities.Messages
{
    public class Message
    {
        [JsonIgnore]
        private readonly WolfBot _bot;

        internal Message(WolfBot bot, BaseMessage message)
        {
            this._bot = bot;
            this.Id = message.Id;
            this.Content = Encoding.UTF8.GetString(message.Data);
            this.SourceSubscriberId = message.Originator.Id;
            this.TargetGroupId = message.IsGroup ? message.Recipient.Id : 0;
            this.Embeds = message.Embeds;
            this.Edited = message.Edited;
            this.Metadata = message.Metadata;
            this.MessageType = message.IsGroup ? MessageType.GROUP : MessageType.PRIVATE;
            this.ContentType = message.ContentType;
            this.Timestamp = message.Timestamp;
        }

        public Guid Id { get; set; }

        public string Content { get; set; }

        public int SourceSubscriberId { get; set; }

        public int TargetGroupId { get; set; }

        public List<Embed> Embeds { get; set; }

        /// <summary>
        /// Contains the timestamp and User id of the account that edited or deleted the message
        /// </summary>
        public Edit Edited { get; set; }

        /// <summary>
        /// Contains the IsSpam, IsDeleted, IsEdited and Formatting data
        /// </summary>
        public Metadata Metadata { get; set; }

        public MessageType MessageType { get; set; }

        public ContentType ContentType { get; set; }

        public string FlightId { get; set; }

        public long Timestamp { get; set; }

   
        /// <summary>
        /// The message is a private message
        /// </summary>
        public bool IsGroup => MessageType != MessageType.PRIVATE;

        public bool IsCommand => _bot.CommandManager.Commands.Any(command => _bot._usingTranslations ? _bot.Phrase().cache.Where(phrase => phrase.Name.IsEqual(command.Value.Trigger)).ToList().Any(phrase => Content.StartsWithCommand(phrase.Value)) : Content.StartsWith(command.Value.Trigger)) || _bot.FormManager.Forms.Any(form => _bot._usingTranslations ? _bot.Phrase().cache.Where(phrase => phrase.Name.IsEqual(form.Value.Trigger)).ToList().Any(s => Content.StartsWithCommand(s.Value)) : Content.StartsWith(form.Value.Trigger));


        public async Task<Response<MessageResponse>> SendMessageAsync(object content, bool includeEmbeds = false)=> await (IsGroup?_bot.Messaging().SendGroupMessageAsync(TargetGroupId, content, includeEmbeds):_bot.Messaging().SendPrivateMessageAsync(SourceSubscriberId, content, includeEmbeds));
        public async Task<Response<Message>> DeleteAsync() => await _bot.Messaging().DeleteAsync(this);

        public async Task<Response<Message>> RestoreAsync() => await _bot.Messaging().RestoreAsync(this);

        public async Task<Response> Tip(params TipCharm[] charms) => await _bot.Tip().AddAsync(this, charms);

    }
}
