using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Notifications
{
    public class Notification
    {
        [JsonProperty("actions")]
        public List<object> Actions { get; set; }

        [JsonProperty("endAt")]
        public DateTime EndAt { get; set; }

        [JsonProperty("favourit")]
        public bool Favourite { get; set; }

        [JsonProperty("global")]
        public bool Global { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; set; }

        [JsonProperty("layoutType")]
        public int LayoutType { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("newsStreamType")]
        public int NewsStreamType { get; set; }

        [JsonProperty("persistent")]
        public bool Persistent { get; set; }

        [JsonProperty("startAt")]
        public DateTime StartAt { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }
    }
}