using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Messages.Tipping
{
    public class Tip
    {
        [JsonProperty("charmList")]
        public List<TipCharm> CharmList { get; private set; }

        [JsonProperty("groupId")]
        public int GroupId { get; private set; }

        [JsonProperty("sourceSubscriberId")]
        public int SourceSubscriberId { get; private set; }

        [JsonProperty("subscriberId")]
        public int SubscriberId { get; private set; }
    }
}
