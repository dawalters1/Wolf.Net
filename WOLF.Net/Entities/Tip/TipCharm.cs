using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Entities.Tip
{
    public class TipCharm
    {
        public TipCharm() { }

        public TipCharm(int id, int quanitity)
        {
            Id = id;
            Quantitiy = quanitity;
        }

        [JsonProperty("id")]
        public int Id { get;set; }

        [JsonProperty("quantity")]
        public int Quantitiy { get;set; }

        [JsonProperty("credits")]
        public int Credits { get;set; }

        [JsonProperty("magnitude")]
        public int Magnitude { get;set; }

        [JsonProperty("subscriber")]
        public IdHash Subscriber { get;set; }
    }
}
