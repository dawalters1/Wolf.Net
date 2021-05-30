using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Achievements;
using WOLF.Net.Entities.API;
using WOLF.Net.Enums.Misc;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        internal Dictionary<Language, List<Achievement>> achievementCache = new Dictionary<Language, List<Achievement>>();
        internal Dictionary<Language, List<Category>> categoryCache = new Dictionary<Language, List<Category>>();

        public async Task<List<Category>> GetCategoryListAsync(Language language = Language.ENGLISH, bool requestNew = false)
        {
            if (requestNew || !categoryCache.ContainsKey(language))
            {
                var result = await _webSocket.Emit<Response<List<Category>>>(Request.ACHIEVEMENT_CATEGORY_LIST, new { languageId = (int)language });

                if (result.Success)
                {
                    if (!categoryCache.ContainsKey(language))
                        categoryCache.Add(language, result.Body);
                    else
                        categoryCache[language] = result.Body;
                }
            }

            return categoryCache.ContainsKey(language) ? categoryCache[language] : new List<Category>();
        }

        public async Task<List<Achievement>> GetByIdsAsync(List<int> achievementIds, Language language = Language.ENGLISH, bool requestNew = false)
        {
            List<Achievement> achievements = new List<Achievement>();

            if (!requestNew && achievementCache.ContainsKey(language))
                achievements.AddRange(achievementCache[language].Where((achievement) => achievement.Id == achievement.Id).ToList());

            if (achievements.Count != achievementIds.Count)
            {
                foreach (var batchAchievementIdList in achievementIds.Where((subscriberId) => !achievements.Any((achievement) => achievement.Id == subscriberId)).ToList().ChunkBy(50))
                {
                    var result = await _webSocket.Emit<Response<List<Response<Achievement>>>>(Request.ACHIEVEMENT, new
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

        public async Task<List<Unlockable>> GetSubscriberAchievementsAsync(int subscriberId)
        {
            var achievements = await _webSocket.Emit<Response<List<Unlockable>>>(Request.ACHIEVEMENT_SUBSCRIBER_LIST,
                new
                {
                    headers = new
                    {
                        version = 2,
                    },
                    body = new
                    {
                        id = subscriberId
                    }
                });

            if (achievements.Success)
                return achievements.Body;

            return new List<Unlockable>();
        }

        public async Task<List<Unlockable>> GetGroupAchievementsAsync(int groupId)
        {
            var achievements = await _webSocket.Emit<Response<List<Unlockable>>>(Request.ACHIEVEMENT_GROUP_LIST,
                new
                {
                    headers = new
                    {
                        version = 2,
                    },
                    body = new
                    {
                        id = groupId
                    }
                });

            if (achievements.Success)
                return achievements.Body;

            return new List<Unlockable>();
        }

        internal Achievement Process(Achievement achievement, Language language)
        {
            if (achievement.Exists)
            {
                if (achievementCache.ContainsKey(language))
                    achievementCache[language].Add(achievement);
                else
                    achievementCache.Add(language, new List<Achievement>() { achievement });
            }

            return achievement;
        }
    }
}
