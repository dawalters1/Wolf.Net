using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Entities.Tip.Leaderboards
{
    public class TipLeaderboardSummary
    {
        [JsonProperty("topGifters")]
        public List<IdHash> TopGifters { get; set; }

        [JsonProperty("topGroups")]
        public List<IdHash> TopGroups { get; set; }

        [JsonProperty("topSpenders")]
        public List<IdHash> TopSpenders { get; set; }
    }
}
