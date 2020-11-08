using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Subscribers;

namespace WOLF.Net.Entities.API
{
    public class CurrentSubscriber
    {
        [JsonProperty("cognito")]
        public Dictionary<string, string> Cognito { get; set; }

        [JsonProperty("isNew")]
        public bool IsNew { get; set; }

        [JsonProperty("subscriber")]
        public Subscriber Subscriber { get; set; }
    }
}
