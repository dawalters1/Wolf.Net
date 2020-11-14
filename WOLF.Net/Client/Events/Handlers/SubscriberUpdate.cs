using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Client.Events.Handlers
{
    public class SubscriberUpdate : Event<IdHash>
    {
        public override string Command => Event.SUBSCRIBER_UPDATE;

        public override async void HandleAsync(IdHash data)
        {
            var subscriber = Bot.Subscribers.FirstOrDefault(r => r.Id == data.Id);

            await Bot.GetSubscriber(data.Id, data.Hash!=subscriber.Hash);

        }
    }
}
