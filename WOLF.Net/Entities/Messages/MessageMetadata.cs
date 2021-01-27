using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Messages
{
    public class MessageMetadata
    {
        [JsonProperty("isSpam")]
        public bool IsSpam { get;set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("isTipped")]
        public bool IsTipped { get; set; }
    }
}
