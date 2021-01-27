using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Misc;
using WOLF.Net.Enums.Groups;

namespace WOLF.Net.Entities.Groups
{
    public class SubscriberGroup
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("capabilities")]
        public Capability Capabilities { get; set; }

        [JsonProperty("additionalInfo")]
        public IdHash AdditionalInfo { get; set; }
    }
}
