using Newtonsoft.Json;
using WOLF.Net.Entities.Misc;
using WOLF.Net.Entities.Subscribers;

namespace WOLF.Net.Entities.API
{
    public class LoginResponse

    {
        [JsonProperty("cognito")]
        public Cognito Cognito { get; set; }

        [JsonProperty("isNew")]
        public bool IsNew { get; set; }

        [JsonProperty("subscriber")]
        public Subscriber Subscriber { get; set; }
    }
}
