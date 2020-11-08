using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Groups.Stages
{
    public class GroupAudioConfiguration
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("minRepLevel")]
        public int MinRepLevel { get; set; }

        [JsonProperty("stageId")]
        public int StageId { get; set; }

    }
}