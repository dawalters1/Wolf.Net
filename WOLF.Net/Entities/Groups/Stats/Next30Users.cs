using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Groups.Stats
{
    public class Next30Users
    {
        [JsonProperty("lineCount")]
        public int LineCount { get;  set; }

        [JsonProperty("message")]
        public string Message { get;  set; }

        [JsonProperty("nickname")]
        public string Nickname { get;  set; }

        [JsonProperty("subId")]
        public int SubId { get;  set; }
    }
}
