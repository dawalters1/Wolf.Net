using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Misc;
using WOLF.Net.Enums.Messages;

namespace WOLF.Net.Entities.Messages
{
    public class BaseMessage
    {
        internal BaseMessage() { }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("edited")]
        public Edit Edited { get; set; }

        [JsonProperty("recipient")]
        public IdHash Recipient { get; set; }

        [JsonProperty("originator")]
        public IdHash Originator { get; set; }

        [JsonProperty("isGroup")]
        public bool IsGroup { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("mimeType")]
        public string MimeType { get; set; }

        [JsonProperty("data")]
        public byte[] Data { get; set; }

        [JsonProperty("flightId")]
        public string FlightId { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("embeds")]
        public List<Embed> Embeds { get; set; }

        public ContentType ContentType => MimeType switch
        {
            "text/plain" => ContentType.TEXT,
            "text/html" => ContentType.MESSAGE_PACK,
            "image/jpeg" => ContentType.IMAGE,
            "text/image_link" => ContentType.IMAGE,
            "text/voice_link" => ContentType.VOICE_MESSAGE,
            "audio/x-speex" => ContentType.VOICE_MESSAGE,
            "audio/aac" => ContentType.VOICE_MESSAGE,
            "application/palringo-group-action" => ContentType.GROUP_ACTION,
            "text/palringo-private-request-response" => ContentType.PRIVATE_REQUEST_RESPONSE,
            _ => ContentType.UNKNOWN
        };

        public Message NormalizeMessage(WolfBot bot)
        {
            return new Message(bot, this);
        }
    }
}
