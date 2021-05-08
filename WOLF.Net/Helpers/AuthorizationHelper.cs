using System.Collections.Generic;
using System.Linq;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        internal readonly List<int> authorizedCache = new List<int>();
        /// <summary>
        /// 
        /// </summary>
        /// <returns>ID list of authorized subscribers</returns>
        public IReadOnlyList<int> Authorized() => authorizedCache;

        /// <summary>
        /// Clear the authorization list
        /// </summary>
        public void ClearAuthorized() => authorizedCache.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The count of Authorized users</returns>
        public int AuthorizedCount() => authorizedCache.Count;

        /// <summary>
        /// Check to see if a subscriber or subscribers is/are authorized
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns>bool</returns>
        public bool IsAuthorized(int subscriberId) => authorizedCache.Any((id) => id == subscriberId);

        /// <summary>
        /// Authorize a subscriber or subscribers
        /// </summary>
        /// <param name="subscriberIds"></param>
        public void Authorize(params int[] subscriberIds) => authorizedCache.AddRange(subscriberIds.Where((subscriberId) => !authorizedCache.Any((id) => subscriberId == id)).ToList());

        /// <summary>
        /// Unauthorize a subscriber or subscribers
        /// </summary>
        /// <param name="subscriberIds"></param>
        public void Deauthorize(params int[] subscriberIds) => authorizedCache.RemoveAll((subscriberId) => subscriberIds.Any((id) => subscriberId == id));
    }
}