using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Messages.Tipping
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
