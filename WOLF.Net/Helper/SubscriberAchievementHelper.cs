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
    public class SubscriberAchievementHelper : BaseHelper<SubscriberAchievement>
    {
        internal Dictionary<int, List<SubscriberAchievement>> _subscriber = new Dictionary<int, List<SubscriberAchievement>>();

        /// <summary>
        /// Get a list of subscriber achievements
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="requestNew"></param>
        /// <returns>Response<List<SubscriberAchievement>></returns>
        public async Task<Response<List<SubscriberAchievement>>> GetByIdAsync(int subscriberId, bool requestNew = false)
        {

            if (!requestNew && _subscriber.ContainsKey(subscriberId))
                return new Response<List<SubscriberAchievement>>() { Code = 207, Body = _subscriber[subscriberId], Headers = new Dictionary<string, string>() };

            var acheivements = await WebSocket.Emit<Response<List<SubscriberAchievement>>>(Request.ACHIEVEMENT_SUBSCRIBER_LIST, new { id = subscriberId });

            if (acheivements.Success)
            {
                if (_subscriber.ContainsKey(subscriberId))
                    _subscriber[subscriberId] = acheivements.Body;
                else
                    _subscriber.Add(subscriberId, acheivements.Body);
            }

            return acheivements;
        }

        internal SubscriberAchievementHelper(WolfBot bot, WebSocket WebSocket) : base(bot, WebSocket) { }
    }
}