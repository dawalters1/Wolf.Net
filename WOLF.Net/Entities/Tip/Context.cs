using Newtonsoft.Json;

namespace WOLF.Net.Entities.Tip
{
    public class Context
    {
        [JsonProperty("type")]
        public string Type { get;set; }

        [JsonProperty("id")]
        public long Id { get;set; }
    }
}
