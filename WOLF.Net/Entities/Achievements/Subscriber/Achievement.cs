using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Achievements.Subscriber
{
    /// <summary>
    /// Base acheivement 
    /// </summary>
    public class Achievement
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("children")]
        public List<Achievement> Children { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("notificationPhraseId")]
        public int NotificationPhraseId { get; set; }

        [JsonProperty("parentId")]
        public int ParentId { get; set; }

        [JsonProperty("typeId")]
        public int TypeId { get; set; }

        [JsonProperty("weight")]
        public int Weight { get; set; }
    }
}
