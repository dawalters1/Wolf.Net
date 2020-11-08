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
        public BaseMessage() { }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("edited")]
        public MessageEdit Edited { get; set; }

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
        public MessageMetadata Metadata { get; set; }

        public ContentType ContentType => MimeType switch
        {
            "text/plain" => ContentType.Text,
            "text/html" => ContentType.MessagePack,
            "image/jpeg" => ContentType.Image,
            "text/image_link" => ContentType.Image,
            "text/voice_link" => ContentType.VoiceMessage,
            "audio/x-speex" => ContentType.VoiceMessage,
            "audio/aac" => ContentType.VoiceMessage,
            "application/palringo-group-action" => ContentType.GroupAction,
            "text/palringo-private-request-response" => ContentType.PrivateRequestResponse,
            _ => ContentType.Unknown
        };
        public Message ToNormalMessage(WolfBot bot)
        {
            return new Message(bot, this);
        }
    }
}
