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
    public class SubscriberAchievementHelper : BaseHelper<Unlockable>
    {
        /// <summary>
        /// Get a list of subscriber achievements
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="requestNew"></param>
        /// <returns>Response<List<SubscriberAchievement>></returns>
        public async Task<List<Unlockable>> GetByIdAsync(int subscriberId)
        {
            var achievements = await WebSocket.Emit<Response<List<Unlockable>>>(Request.ACHIEVEMENT_SUBSCRIBER_LIST,
                new { 
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

        internal SubscriberAchievementHelper(WolfBot bot, WebSocket WebSocket) : base(bot, WebSocket) { }
    }
}