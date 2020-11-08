using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Topics
{
    public class TopicLink
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
