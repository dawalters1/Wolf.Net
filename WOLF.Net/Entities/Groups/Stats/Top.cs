using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Groups.Stats
{
   public class Top
    {
        [JsonProperty("subId")]
        public int SubId { get;  set; }

        [JsonProperty("nickname")]
        public string Nickname { get;  set; }

        [JsonProperty("percentage")]
        public double Percentage { get;  set; }

        [JsonProperty("double")]
        public int Double { get;  set; }

        [JsonProperty("wpl")]
        public int Wpl { get;  set; }

        [JsonProperty("randomQuote")]
        public string RandomQuote { get;  set; }
    }
}
