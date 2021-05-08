using Newtonsoft.Json;
using System.Collections.Generic;

namespace WOLF.Net.Entities.Tip
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
