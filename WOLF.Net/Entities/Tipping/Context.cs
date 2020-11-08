using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Messages.Tipping
{
    public class Context
    {
        [JsonProperty("type")]
        public string Type { get; private set; }

        [JsonProperty("id")]
        public long Id { get; private set; }
    }
}
