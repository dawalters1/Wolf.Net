using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Messages.Tipping
{
    public class TipSummary
    {
        public TipSummary() { }

        [JsonProperty("id")]
        public long Id { get; private set; }

        [JsonProperty("charmList")]
        public List<TipCharm> CharmList { get; private set; }
   
        [JsonProperty("version")]
        public int Version { get; private set; }
    }
}
