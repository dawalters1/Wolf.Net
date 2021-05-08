using System.Collections.Generic;
using System.Linq;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        internal readonly List<int> bannedCache = new List<int>();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>ID list of banned subscribers</returns>
        public IReadOnlyList<int> BannedList() => bannedCache;

        /// <summary>
        /// Clear the banned list
        /// </summary>
        public void ClearBanned() => bannedCache.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The count of Banned users</returns>
        public int BannedCount() => bannedCache.Count;

        /// <summary>
        /// Check to see if a subscriber is banned
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns>bool</returns>
        public bool IsBanned(int subscriberId) => bannedCache.Any((id) => id == subscriberId);

        /// <summary>
        /// Ban a subscriber or subscribers
        /// </summary>
        /// <param name="subscriberIds"></param>
        public void Ban(params int[] subscriberIds) => bannedCache.AddRange(subscriberIds.Where((subscriberId) => !bannedCache.Any((id) => subscriberId == id)).ToList());

        /// <summary>
        /// Unban a subscriber or subscribers
        /// </summary>
        /// <param name="subscriberIds"></param>
        public void Unban(params int[] subscriberIds) => bannedCache.RemoveAll((subscriberId) => subscriberIds.Any((id) => subscriberId == id));
    }
}