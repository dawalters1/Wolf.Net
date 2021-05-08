using Newtonsoft.Json;

namespace WOLF.Net.Entities.Messages
{
    public class MessageResponse
    {
        [JsonProperty("uuid")]
        public string Id { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
