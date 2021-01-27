using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Topics
{
    public class Topic
    {
        [JsonProperty("layout")]
        public string Layout { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }

        [JsonProperty("recipeId")]
        public int RecipeId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("links")]
        public List<TopicLink> Links { get; set; }
    }
}