using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Networking;

namespace WOLF.Net.Helper
{
    public class AuthorizationHelper: BaseHelper<int>
    {
        public IReadOnlyList<int> List() => cache;

        public bool IsAuthorized(int subscriberId) => cache.Any((id) => id == subscriberId); 

        public void Authorize(params int[] subscriberIds) => cache.AddRange(subscriberIds.Where((subscriberId) => !cache.Any((id) => subscriberId == id)).ToList());

        public void Deauthorize(params int[] subscriberIds) => cache.RemoveAll((subscriberId) => subscriberIds.Any((id) => subscriberId == id));

        internal AuthorizationHelper(WolfBot bot, WebSocket WebSocket) : base(bot, WebSocket) { }
    }
}
