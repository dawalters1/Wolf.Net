using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Groups.Stats
{
    public class DailyTrend
    {
        [JsonProperty("day")]
        public int Day { get;  set; }

        [JsonProperty("lineCount")]
        public int LineCount { get;  set; }
    }
}
