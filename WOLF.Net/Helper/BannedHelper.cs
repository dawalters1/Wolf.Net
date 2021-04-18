using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Networking;

namespace WOLF.Net.Helper
{
    public class BannedHelper : BaseHelper<int>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>ID list of banned subscribers</returns>
        public IReadOnlyList<int> List() => cache;

        /// <summary>
        /// Clear the banned list
        /// </summary>
        public void Clear() => cache.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The count of Banned users</returns>
        public int Count() => cache.Count;

        /// <summary>
        /// Check to see if a subscriber is banned
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns>bool</returns>
        public bool IsBanned(int subscriberId) => cache.Any((id) => id == subscriberId);

        /// <summary>
        /// Ban a subscriber or subscribers
        /// </summary>
        /// <param name="subscriberIds"></param>
        public void Ban(params int[] subscriberIds) => cache.AddRange(subscriberIds.Where((subscriberId) => !cache.Any((id) => subscriberId == id)).ToList());
      
        /// <summary>
        /// Unban a subscriber or subscribers
        /// </summary>
        /// <param name="subscriberIds"></param>
        public void Unban(params int[] subscriberIds) => cache.RemoveAll((subscriberId) => subscriberIds.Any((id) => subscriberId == id));

        internal BannedHelper(WolfBot bot, WebSocket WebSocket) : base(bot, WebSocket) { }
    }
}