using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Charms
{
    public class Charm
    {
        [JsonProperty("id")]
        public int Id { get; set;  }

        [JsonProperty("productId")]
        public int? ProductId { get; set;  }

        [JsonProperty("name")]
        public string Name { get; set;  }

        [JsonProperty("description")]
        public string Description { get; set;  }
    }
}
