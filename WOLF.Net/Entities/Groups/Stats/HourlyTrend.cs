using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Groups.Stats
{
    public class HourlyTrend
    {
        [JsonProperty("hour")]
        public int Hour { get;  set; }

        [JsonProperty("lineCount")]
        public int LineCount { get;  set; }
    }
}
