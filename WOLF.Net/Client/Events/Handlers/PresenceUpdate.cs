using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Subscribers;

namespace WOLF.Net.Client.Events.Handlers
{
    public class PresenceUpdate:Event<Entities.Subscribers.PresenceUpdate>
    {
        public override string Command => Event.SUBSCRIBER_BLOCK_ADD;

        public override void HandleAsync(Entities.Subscribers.PresenceUpdate data)
        {
            throw new NotImplementedException();
        }
    }
}
