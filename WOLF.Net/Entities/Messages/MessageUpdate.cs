using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Entities.Messages
{
    public class MessageUpdate
    {
        [JsonProperty("recipient")]
        public IdHash Recipient { get; set; }

        [JsonProperty("originator")]
        public IdHash Originator { get; set; }

        [JsonProperty("isGroup")]
        public bool IsGroup { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("edit")]
        public Edit Edit { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        [JsonProperty("data")]
        public byte[] Data { get; set; }

        [JsonIgnore]
        public string Content => Data == null ? null : Encoding.UTF8.GetString(Data);
    }
}
