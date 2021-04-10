using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Achievements;
using WOLF.Net.Entities.API;
using WOLF.Net.Networking;

namespace WOLF.Net.Helper
{
    public class GroupAchievementHelper : BaseAchievementHelper
    {
        public Dictionary<int, List<Unlockable>> _group = new Dictionary<int, List<Unlockable>>();

        public async Task<Response<List<Unlockable>>> GetById(int groupId, bool requestChildren = false, bool requestNew = false)
        {
            if (!requestNew && _group.ContainsKey(groupId))
            {
                if (requestChildren)
                {
                    var groupAchievements = _group[groupId];
                    if (groupAchievements.Any((achievement) => achievement.AdditionalInfo.Steps > 0 && achievement.AdditionalInfo.Children.Count == 0))
                    {
                        foreach (var parent in groupAchievements)
                        {
                            var children = await GetChildrenByParentId(groupId, parent.Id);

                            parent.AdditionalInfo.Children = children.Success ? children.Body : new List<Unlockable>();
                        }
                    }
                }

                return new Response<List<Unlockable>>() { Code = 207, Body = _group[groupId], Headers = new Dictionary<string, string>() };
            }

            var achievements = await WebSocket.Emit<Response<List<Unlockable>>>(Request.ACHIEVEMENT_GROUP_LIST, new { id = groupId });

            if (achievements.Success)
            {
                if (requestChildren)
                {
                    foreach (var parent in achievements.Body)
                    {
                        var children = await GetChildrenByParentId(groupId, parent.Id);

                        parent.AdditionalInfo.Children = children.Success ? children.Body : new List<Unlockable>();
                    }
                }

                if (_group.ContainsKey(groupId))
                    _group[groupId] = achievements.Body;
                else
                    _group.Add(groupId, achievements.Body);
            }

            return achievements;
        }

        internal async Task<Response<List<Unlockable>>> GetChildrenByParentId(int groupId, int parentId) => await WebSocket.Emit<Response<List<Unlockable>>>(Request.ACHIEVEMENT_GROUP_LIST, new { id = groupId, parentId });

        internal GroupAchievementHelper(WolfBot bot, WebSocket WebSocket) : base(bot, WebSocket) { }
    }
}
