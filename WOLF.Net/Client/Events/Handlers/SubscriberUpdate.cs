using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Client.Events.Handlers
{
    public class SubscriberUpdate : Event<IdHash>
    {
        public override string Command => Event.SUBSCRIBER_UPDATE;

        public override void HandleAsync(IdHash data)
        {
            throw new NotImplementedException();
        }
    }
}
