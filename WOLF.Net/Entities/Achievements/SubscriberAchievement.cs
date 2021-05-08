using Newtonsoft.Json;
using System;

namespace WOLF.Net.Entities.Achievements
{
    public class SubscriberAchievement
    {
        [JsonProperty("achievementId")]
        public int AchievementId { get; set; }

        [JsonProperty("updateTime")]
        public DateTime UpdateTime { get; set; }
    }
}
