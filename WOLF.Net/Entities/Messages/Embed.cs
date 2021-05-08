using Newtonsoft.Json;

namespace WOLF.Net.Entities.Messages
{
    public class Embed
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("image")]
        public byte[] Image { get; set; }
    }
}