using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Tipping
{
    public class TipSummary
    {
        public TipSummary() { }

        [JsonProperty("id")]
        public long Id { get;set; }

        [JsonProperty("charmList")]
        public List<TipCharm> CharmList { get;set; }
   
        [JsonProperty("version")]
        public int Version { get;set; }
    }
}
