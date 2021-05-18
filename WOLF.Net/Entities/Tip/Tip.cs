using Newtonsoft.Json;
using System.Collections.Generic;

namespace WOLF.Net.Entities.Tip
{
    public class Tip
    {
        [JsonProperty("charmList")]
        public List<TipCharm> CharmList { get;set; }

        [JsonProperty("groupId")]
        public int GroupId { get;set; }

        [JsonProperty("sourceSubscriberId")]
        public int SourceSubscriberId { get;set; }

        [JsonProperty("subscriberId")]
        public int SubscriberId { get;set; }

        [JsonProperty("context")]
        public Context Context { get; set; }
    }
}
