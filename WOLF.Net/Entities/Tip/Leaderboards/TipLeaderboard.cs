using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Entities.Tip.Leaderboards
{
    public class TipLeaderboard
    {
        [JsonProperty("leaderboard")]
        public List<TipLeaderboardItem> Leaderboard { get; set; }
    }
}
