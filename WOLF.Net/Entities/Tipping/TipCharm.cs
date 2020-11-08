using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Entities.Messages.Tipping
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
        public int Id { get; private set; }

        [JsonProperty("quantity")]
        public int Quantitiy { get; private set; }

        [JsonProperty("credits")]
        public int Credits { get; private set; }

        [JsonProperty("magnitude")]
        public int Magnitude { get; private set; }

        [JsonProperty("subscriber")]
        public IdHash Subscriber { get; private set; }
    }
}
