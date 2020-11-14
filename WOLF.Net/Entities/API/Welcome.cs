using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Subscribers;

namespace WOLF.Net.Entities.API
{
    public class Welcome
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("endpointConfig")]
        public EndpointConfig EndpointConfig { get; set; }

        [JsonProperty("loggedInUser")]
        public Subscriber LoggedInUser { get; set; }
    }

    public class EndpointConfig
    {
        [JsonProperty("avatarEndpoint")]
        public Uri AvatarEndpoint { get; set; }

        [JsonProperty("mmsUploadEndpoint")]
        public Uri MmsUploadEndpoint { get; set; }

        [JsonProperty("banner")]
        public Banner Banner { get; set; }
    }

    public class Banner
    {
        [JsonProperty("notification")]
        public Dictionary<string, Uri> Notification { get; set; }

        [JsonProperty("promotion")]
        public Dictionary<string, Uri> Promotion { get; set; }
    }
}
