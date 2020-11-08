using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Entities.Tipping.Leaderboards
{
    public class TipLeaderboard
    {
        [JsonProperty("leaderboard")]
        public List<TipLeaderboardItem> Leaderboard { get; set; }
    }
}
