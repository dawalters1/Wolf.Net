using Newtonsoft.Json;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Entities.Subscribers
{
    public class PresenceUpdate
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("deviceType")]
        public DeviceType DeviceType { get; set; }

        [JsonProperty("onlineState")]
        public OnlineState OnlineState { get; set; }
    }
}
