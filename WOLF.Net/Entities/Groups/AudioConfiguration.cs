using Newtonsoft.Json;

namespace WOLF.Net.Entities.Groups
{
    public class AudioConfiguration
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
        /// The subscriber who updated the stage
        /// </summary>
        [JsonProperty("sourceSubscriberId")]
        public int SourceSubscriberId { get; set; }
    }
}
