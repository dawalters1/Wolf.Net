using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Groups
{
    public class GroupEntityResponse
    {
        [JsonProperty("base")]
        public Group Base { get; set; }

        [JsonProperty("extended")]
        public GroupExtended Extended { get; set; }

        [JsonProperty("audioCounts")]
        public GroupAudioCount AudioCounts { get; set; }

        [JsonProperty("audioConfig")]
        public GroupAudioConfiguration AudioConfiguration { get; set; }
    }

}