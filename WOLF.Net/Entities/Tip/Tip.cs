using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
