using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Groups.Stats
{
    public class GroupStats
    {
        [JsonProperty("details")]
        public Details Details { get; set; }

        [JsonProperty("next30")]
        public Next30Users Next30 { get; set; }

        [JsonProperty("top25")]
        public Details Top25 { get; set; }

        [JsonProperty("topAction")]
        public Top TopAction { get; set; }

        [JsonProperty("topEmoticon")]
        public Top TopEmoticon { get; set; }

        [JsonProperty("topHappy")]
        public Top TopHappy { get; set; }

        [JsonProperty("topImage")]
        public Top TopImage { get; set; }

        [JsonProperty("topQuestion")]
        public Top TopQuestion { get; set; }

        [JsonProperty("topSad")]
        public Top TopSad { get; set; }

        [JsonProperty("topSwear")]
        public Top TopSwear { get; set; }

        [JsonProperty("topText")]
        public Top TopText { get; set; }

        [JsonProperty("topWord")]
        public Top TopWord { get; set; }

        [JsonProperty("trends")]
        public Trend Trends { get; set; }

        [JsonProperty("trendsDay")]
        public DailyTrend TrendsDay { get; set; }

        [JsonProperty("trendsHour")]
        public HourlyTrend TrendsHour { get; set; }
    }
}