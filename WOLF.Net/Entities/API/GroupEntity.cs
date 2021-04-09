using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Groups;

namespace WOLF.Net.Entities.API
{
    public class GroupEntity
    {
        [JsonProperty("base")]
        public Group Base { get; set; }

        [JsonProperty("extended")]
        public Extended Extended { get; set; }

        [JsonProperty("audioCounts")]
        public AudioCount AudioCounts { get; set; }

        [JsonProperty("audioConfig")]
        public AudioConfiguration AudioConfiguration { get; set; }

        public Group Compile()
        {
            Base.Extended = Extended;
            Base.AudioCount = AudioCounts;
            Base.AudioConfiguration = AudioConfiguration;

            return Base;
        }
    }
}
