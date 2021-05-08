using Newtonsoft.Json;
using System;

namespace WOLF.Net.Entities.Charms
{
    public class CharmStatus
    {
        [JsonProperty("id")]
        public int Id { get; set;  }

        [JsonProperty("charmId")]
        public int CharmId { get; set;  }

        [JsonProperty("expireTime")]
        public DateTime? ExpireTime { get; set;  }

        [JsonProperty("sourceSubscriberId")]
        public int? SourceSubscriberId { get; set;  }

        [JsonProperty("subscriberId")]
        public int SubscriberId { get; set;  }

    }
}
