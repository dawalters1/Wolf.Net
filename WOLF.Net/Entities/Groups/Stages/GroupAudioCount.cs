using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Groups.Stages
{
    public class GroupAudioCount
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("consumerCount")]
        public int ConsumerCount { get; set; }

        [JsonProperty("broadcasterCount")]
        public int BroadcasterCount { get; set; }
    }
}