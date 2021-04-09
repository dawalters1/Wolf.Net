using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Misc
{
    public class Cognito
    {
        [JsonProperty("identity")]
        public string Identity { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
