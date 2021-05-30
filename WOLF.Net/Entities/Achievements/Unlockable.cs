﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WOLF.Net.Entities.Achievements
{
    public class Unlockable
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("additionalInfo")]
        public AdditionalInfo AdditionalInfo { get; set; }

    }

    public class AdditionalInfo
    {
        [JsonProperty("eTag")]
        public string ETag { get; set; }

        [JsonProperty("awardedAt")]
        public DateTime? AwardedAt { get; set; }

        [JsonProperty("steps")]
        public int Steps { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("bestChildImageUrl")]
        public string BestChildImageUrl { get; set; }

        [JsonProperty("categoryId")]
        public int CategoryId { get; set; }

        /// <summary>
        /// this is not part of the request, this is api handled
        /// </summary>
        [JsonProperty("children")]
        public List<Unlockable> Children { get; set; }
    }
}
