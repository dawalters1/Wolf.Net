using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Misc
{
    public class Search
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }
}