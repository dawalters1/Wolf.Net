using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Achievements;
using WOLF.Net.Entities.API;
using WOLF.Net.Enums.Misc;
using WOLF.Net.Networking;

namespace WOLF.Net.Helper
{
    public class GroupAchievementHelper : BaseHelper<Unlockable>
    {
        /// <summary>
        /// Get a list of group achievements
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="requestNew"></param>
        /// <returns>Response<List<SubscriberAchievement>></returns>
        public async Task<List<Unlockable>> GetByIdAsync(int groupId)
        {
            var achievements = await WebSocket.Emit<Response<List<Unlockable>>>(Request.ACHIEVEMENT_GROUP_LIST,
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

        internal GroupAchievementHelper(WolfBot bot, WebSocket WebSocket) : base(bot, WebSocket) { }
    }
}