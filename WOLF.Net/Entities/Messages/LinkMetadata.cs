using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Messages
{
    public class LinkMetadata
    {
        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("isOfficial")]
        public bool IsOfficial { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("imageSize")]
        public int ImageSize { get; set; }

        [JsonProperty("isBlacklisted")]
        public bool IsBlackListed { get; set; }
    }
}
