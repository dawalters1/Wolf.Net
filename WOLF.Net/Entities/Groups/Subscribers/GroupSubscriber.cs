using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Entities.Groups.Subscribers
{
    public class GroupSubscriber
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonIgnore]
        public int GroupId { get; set; }

        [JsonProperty("additionalInfo")]
        public AdditionalInfo AdditionalInfo { get; set; }

        [JsonProperty("capabilities")]
        public Capability Capabilities { get; set; }
    }

    public class AdditionalInfo
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("nicknameShort")]
        public string Nickname { get; set; }

        [JsonProperty("privileges")]
        public long Privileges { get; set; }

        [JsonProperty("onlineState")]
        public OnlineState OnlineState { get; set; }
    }
}
