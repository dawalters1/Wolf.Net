using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Networking;

namespace WOLF.Net.Helper
{
    public class BannedHelper : BaseHelper<int>
    {
        public IReadOnlyList<int> List() => cache;

        public bool IsBanned(int subscriberId) => cache.Any((id) => id == subscriberId);

        public void Ban(params int[] subscriberIds) => cache.AddRange(subscriberIds.Where((subscriberId) => !cache.Any((id) => subscriberId == id)).ToList());

        public void Unban(params int[] subscriberIds) => cache.RemoveAll((subscriberId) => subscriberIds.Any((id) => subscriberId == id));

        internal BannedHelper(WolfBot bot, WebSocket WebSocket) : base(bot, WebSocket) { }
    }
}