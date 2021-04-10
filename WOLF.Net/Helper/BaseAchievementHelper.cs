using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Achievements;
using WOLF.Net.Entities.API;
using WOLF.Net.Enums.Misc;
using WOLF.Net.Helper;
using WOLF.Net.Networking;

namespace WOLF.Net.Helper
{
    public class BaseAchievementHelper : BaseHelper<Achievement>
    {
        internal Dictionary<Language, List<Achievement>> _achievements = new Dictionary<Language, List<Achievement>>();
        internal Dictionary<Language, List<Category>> _categories = new Dictionary<Language, List<Category>>();

        private SubscriberAchievementHelper _subscriber;
        private GroupAchievementHelper _group;

        public SubscriberAchievementHelper  Subscriber() => _subscriber;
        public GroupAchievementHelper Group() => _group;

        public async Task<List<Achievement>> GetAllAsync(Language language = Language.ENGLISH, bool requestNew = false)
        {
            if (requestNew || !_categories.ContainsKey(language)) {
                var result = await WebSocket.Emit<Response<List<Achievement>>>(Request.ACHIEVEMENT_LIST, new { language = (int)language });

                if (result.Success)
                {
                    if (!_categories.ContainsKey(language))
                        _achievements.Add(language, result.Body);
                    else
                        _achievements[language] = result.Body;
                }
            }

            return _achievements.ContainsKey(language) ? _achievements[language] : new List<Achievement>();
        }

        public async Task<Response<Achievement>> GetByIdAsync(int id, Language language = Language.ENGLISH, bool requestNew = false)
        {
            var achievement = (await GetAllAsync(language, requestNew)).FirstOrDefault(achivement => achivement.Id == id); ;

            if (achievement != null)
                return new Response<Achievement>() { Code = 200, Body = achievement, Headers = new Dictionary<string, string>() };

            return new Response<Achievement>() { Code = 404, Body = default, Headers = new Dictionary<string, string>() };
        }

        public async Task<Dictionary<int, Response<Achievement>>> GetByIds(List<int> ids, Language language = Language.ENGLISH, bool requestNew = false)
        {
            Dictionary<int, Response<Achievement>> achievements = new Dictionary<int, Response<Achievement>>();

            var achievementsList = await GetAllAsync(language, requestNew);

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

        public async Task<List<Category>> GetCategoriesAsync(Language language = Language.ENGLISH, bool requestNew = false)
        {
            if (requestNew || !_categories.ContainsKey(language))
            {
                var result = await WebSocket.Emit<Response<List<Category>>>(Request.ACHIEVEMENT_CATEGORY_LIST, new { language = (int)language });

                if (result.Success)
                {
                    if (!_categories.ContainsKey(language))
                        _categories.Add(language, result.Body);
                    else
                        _categories[language] = result.Body;
                }
            }

            return _categories.ContainsKey(language) ? _categories[language] : new List<Category>();
        }

        internal BaseAchievementHelper(WolfBot bot, WebSocket websocket) : base(bot, websocket) { this._subscriber = new SubscriberAchievementHelper(bot, websocket); this._group = new GroupAchievementHelper(bot, websocket); }
    }
}
