using Newtonsoft.Json;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Entities.Tip.Leaderboards
{
    public class TipLeaderboardItem
    {
        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("charmId")]
        public int CharmId { get; set; }

        [JsonProperty("quantity")]
        public int Quanitity { get; set; }

        [JsonProperty("credits")]
        public int Credits { get; set; }

        [JsonProperty("group")]
        public IdHash Group { get; set; }

        [JsonProperty("subscriber")]
        public IdHash Subscriber { get; set; }
    }
}
