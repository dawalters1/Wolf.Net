using Newtonsoft.Json;

namespace WOLF.Net.Entities.Messages
{
    public class Edit
    {
        [JsonProperty("subscriberId")]
        public int SubscriberId { get; set; }

        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }
    }
}
