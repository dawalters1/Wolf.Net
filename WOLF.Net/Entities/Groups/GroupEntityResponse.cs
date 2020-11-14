using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Groups.Stages;

namespace WOLF.Net.Entities.Groups
{
    public class GroupEntityResponse
    {
        [JsonProperty("base")]
        public Group Base { get; set; }

        [JsonProperty("extended")]
        public Extended Extended { get; set; }

        [JsonProperty("audioCounts")]
        public GroupAudioCount AudioCounts { get; set; }

        [JsonProperty("audioConfig")]
        public GroupAudioConfiguration AudioConfiguration { get; set; }

        internal Group Compile()
        {
            return new Group(Base, AudioConfiguration, AudioCounts, Extended);
        }
    }

}