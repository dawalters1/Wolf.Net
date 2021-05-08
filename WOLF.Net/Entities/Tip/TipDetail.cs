using Newtonsoft.Json;
using System.Collections.Generic;

namespace WOLF.Net.Entities.Tip
{
    public class TipDetail
    {
        public TipDetail() { }

        [JsonProperty("id")]
        public long Id { get;set; }

        [JsonProperty("list")]
        public List<TipCharm> List { get;set; }
   
        [JsonProperty("version")]
        public int Version { get;set; }
    }
}
