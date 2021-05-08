using Newtonsoft.Json;
using System;

namespace WOLF.Net.Entities.Charms
{
    public class CharmSummary
    {
        [JsonProperty("charmId")]
        public int CharmId { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("expireTime")]
        public DateTime? ExpireTime { get; set; }

        [JsonProperty("giftCount")]
        public int GiftCount { get; set; }
    }
}
