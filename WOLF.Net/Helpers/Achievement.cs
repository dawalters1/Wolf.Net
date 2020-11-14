using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Achievements;
using WOLF.Net.Entities.API;
using WOLF.Net.Enums.Misc;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        public List<Achievement> Achievements = new List<Achievement>();


        public async Task<List<Achievement>> GetAchievements(Language language = Language.English, bool requestNew = false)
        {
            if (!requestNew && Achievements.Count > 0)
                return Achievements;

            Achievements.Clear();

            var result = await WolfClient.Emit<List<Achievement>>(Request.ACHIEVEMENT_LIST, new
            {
                language = (int)language
            });

            if(result.Success)
            {
                Achievements = result.Body;

                return Achievements;
            }

            return Achievements;
        }

        public async Task<Response<Achievement>> GetById(int id, bool requestNew = false)
        {
            var achievements = await GetAchievements(requestNew: requestNew);

            var achievement = achievements.FirstOrDefault(r => r.Id == id);

            if (achievement != null)
                return new Response<Achievement>() { Code = 200, Body = achievement, Headers = new Dictionary<string, string>() };

            return new Response<Achievement>() { Code = 404, Body = default, Headers = new Dictionary<string, string>() };
        }

        public async Task<Dictionary<int, Response<Achievement>>> GetByIds(List<int> ids, bool requestNew = false)
        {
            Dictionary<int, Response<Achievement>> achievements = new Dictionary<int, Response<Achievement>> ();

            var achievementsList = await GetAchievements(requestNew: requestNew);

            foreach (var id in ids.Distinct())
            {
                var achievement = achievementsList.FirstOrDefault(r => r.Id == id);
                if (achievement != null)
                   achievements.Add(id, new Response<Achievement>() { Code = 200, Body = achievement, Headers = new Dictionary<string, string>() });
                else
                    achievements.Add(id, new Response<Achievement>() { Code = 404, Body = default, Headers = new Dictionary<string, string>() });
            }

            return achievements;
        }

        public async Task<Response<List<SubscriberAchievement>>> GetSubscriberAchievements(int subscriberId)
        {
            return await WolfClient.Emit<List<SubscriberAchievement>>(Request.ACHIVEMENT_SUBSCRIBER_LIST, new
            {
                id = subscriberId
            });
        }
    }
}
