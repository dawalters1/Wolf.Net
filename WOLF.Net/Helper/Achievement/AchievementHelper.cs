using Newtonsoft.Json;
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
    public class AchievementHelper : BaseHelper<Achievement>
    {
        internal Dictionary<Language, List<Achievement>> _achievements = new Dictionary<Language, List<Achievement>>();
        internal Dictionary<Language, List<Category>> _categories = new Dictionary<Language, List<Category>>();

        private SubscriberAchievementHelper _subscriber;
        private GroupAchievementHelper _group;

        public SubscriberAchievementHelper Subscriber() => _subscriber;
        public GroupAchievementHelper Group() => _group;

        public async Task<List<Category>> GetCategoryListAsync(Language language = Language.ENGLISH, bool requestNew = false)
        {
            if (requestNew || !_categories.ContainsKey(language))
            {
                var result = await WebSocket.Emit<Response<List<Category>>>(Request.ACHIEVEMENT_CATEGORY_LIST, new { languageId = (int)language });

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

        public async Task<List<Achievement>> GetByIdsAsync(List<int> achievementIds, Language language = Language.ENGLISH, bool requestNew = false)
        {
            List<Achievement> achievements = new List<Achievement>();

            if (!requestNew && _achievements.ContainsKey(language))
                achievements.AddRange(_achievements[language].Where((achievement) => achievement.Id == achievement.Id).ToList());

            if (achievements.Count != achievementIds.Count)
            {
                foreach (var batchAchievementIdList in achievementIds.Where((subscriberId) => !achievements.Any((achievement) => achievement.Id == subscriberId)).ToList().ChunkBy(50))
                {
                    var result = await WebSocket.Emit<Response<List<Response<Achievement>>>>(Request.ACHIEVEMENT, new
                    {
                        headers = new
                        {
                            version = 2
                        },
                        body = new
                        {
                            idList = batchAchievementIdList,
                            languageId = (int)language
                        }

                    });

                    if (result.Success)
                        for (var index = 0; index < batchAchievementIdList.Count; index++)
                        {
                            var achievement = result.Body[index];
                            if (achievement.Success)
                                achievements.Add(Process(achievement.Body, language));
                            else
                                achievements.Add(new Achievement() { Id = batchAchievementIdList[index], Exists = false });
                        }
                    else
                        achievements.AddRange(batchAchievementIdList.Select((achievementId) => new Achievement() { Id = achievementId, Exists = false }).ToList());
                }
            }

            return achievements;
        }

        public async Task<Achievement> GetByIdAsync(int id, Language language = Language.ENGLISH, bool requestNew = false) => (await GetByIdsAsync(new List<int>() { id }, language, requestNew)).FirstOrDefault();

        internal Achievement Process(Achievement achievement, Language language)
        {
            if (achievement.Exists)
            {
                if (_achievements.ContainsKey(language))
                    _achievements[language].Add(achievement);
                else
                    _achievements.Add(language, new List<Achievement>() { achievement });
            }

            return achievement;
        }

        internal AchievementHelper(WolfBot bot, WebSocket webSocket) : base(bot, webSocket) {

            _group = new GroupAchievementHelper(bot, webSocket);
            _subscriber = new SubscriberAchievementHelper(bot, webSocket);

        }

    }
}