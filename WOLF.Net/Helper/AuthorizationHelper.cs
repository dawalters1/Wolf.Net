using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Networking;

namespace WOLF.Net.Helper
{
    public class AuthorizationHelper: BaseHelper<int>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>ID list of authorized subscribers</returns>
        public IReadOnlyList<int> List() => cache;

        /// <summary>
        /// Clear the authorization list
        /// </summary>
        public void Clear() => cache.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The count of Authorized users</returns>
        public int Count() => cache.Count;

        /// <summary>
        /// Check to see if a subscriber or subscribers is/are authorized
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns>bool</returns>
        public bool IsAuthorized(int subscriberId) => cache.Any((id) => id == subscriberId); 

        /// <summary>
        /// Authorize a subscriber or subscribers
        /// </summary>
        /// <param name="subscriberIds"></param>
        public void Authorize(params int[] subscriberIds) => cache.AddRange(subscriberIds.Where((subscriberId) => !cache.Any((id) => subscriberId == id)).ToList());

        /// <summary>
        /// Unauthorize a subscriber or subscribers
        /// </summary>
        /// <param name="subscriberIds"></param>
        public void Deauthorize(params int[] subscriberIds) => cache.RemoveAll((subscriberId) => subscriberIds.Any((id) => subscriberId == id));

        internal AuthorizationHelper(WolfBot bot, WebSocket WebSocket) : base(bot, WebSocket) { }
    }
}
