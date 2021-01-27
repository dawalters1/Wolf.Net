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

        /// <summary>
        /// The user who updated the stage
        /// </summary>
        [JsonProperty("sourceSubscriberId")]
        public int SourceSubscriberId { get; set; }
    }
}