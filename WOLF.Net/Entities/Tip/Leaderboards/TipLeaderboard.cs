using Newtonsoft.Json;
using System.Collections.Generic;

namespace WOLF.Net.Entities.Tip.Leaderboards
{
    public class TipLeaderboard
    {
        [JsonProperty("leaderboard")]
        public List<TipLeaderboardItem> Leaderboard { get; set; }
    }
}
