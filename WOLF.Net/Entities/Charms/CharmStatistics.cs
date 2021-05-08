using Newtonsoft.Json;

namespace WOLF.Net.Entities.Charms
{
    public class CharmStatistics
    {
        [JsonProperty("subscriberId")]
        public int SubscriberId { get; set;  }

        [JsonProperty("totalActive")]
        public int TotalActive { get; set;  }

        [JsonProperty("totalExpired")]
        public int TotalExpired { get; set;  }

        [JsonProperty("totalGiftedReceived")]
        public int TotalGiftedReceived { get; set;  }

        [JsonProperty("totalGiftedSent")]
        public int TotalGiftedSent { get; set;  }

        [JsonProperty("totalLifetime")]
        public int TotalLifeTime { get; set;  }
    }
}
