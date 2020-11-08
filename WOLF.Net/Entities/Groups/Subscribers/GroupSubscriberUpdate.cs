using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Groups;

namespace WOLF.Net.Entities.Groups.Subscribers
{
    public class GroupSubscriberUpdate
    {
        [JsonProperty("subscriberId")]
        public int SubscriberId { get; set; }

        [JsonProperty("groupId")]
        public int GroupId { get; set; }

        [JsonProperty("capabilities")]
        public Capability Capabilities { get; set; }
    }
}
