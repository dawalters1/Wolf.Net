using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Messages
{
    public class Metadata
    {
        [JsonProperty("isSpam")]
        public bool IsSpam { get; set; }

        [JsonProperty("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonProperty("isTipped")]
        public bool IsTipped { get; set; }

        [JsonProperty("formatting")]
        public Formatting Formatting { get; set; }
    }

    public class Formatting
    {
        [JsonProperty("groupLinks")]
        public List<Link> GroupLinks { get; set; }

        [JsonProperty("links")]
        public List<Link> Links { get; set; }
    }
    public class Link
    {
        [JsonProperty("start")]
        public int Start { get; set; }

        [JsonProperty("end")]
        public int End { get; set; }
    }
}
