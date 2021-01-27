using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Messages
{

    public class MessageEdit
    {
        [JsonProperty("subscriberId")]
        public int SubscriberId { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
