using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.API
{
    public class Cognito
    {
        public Cognito() { }

        [JsonProperty("identity")]
        public string Identity { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
