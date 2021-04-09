using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Networking.Events.Handlers
{
    public class SubscriberUpdate : BaseEvent<IdHash>
    {
        public override string Command => Event.SUBSCRIBER_UPDATE;
        public override bool ReturnBody => true;
        public override async void Handle(IdHash data)
        {
            try
            {
                var subscriber = Bot.Subscriber().cache.FirstOrDefault(r => r.Id == data.Id);

                if (subscriber == null || data.Hash == subscriber.Hash)
                    return;

                var updatedSubscriber = await Bot.Subscriber().GetByIdAsync(data.Id, true);

                foreach (var group in Bot.Group().cache.ToList())
                    if (group.Subscribers.Any(r => r.Id == data.Id))
                        group.Subscribers.FirstOrDefault(r => r.Id == data.Id).Update(updatedSubscriber);

                if (Bot.Blocked().cache.Any(r => r.Id == data.Id))
                    Bot.Blocked().cache.FirstOrDefault(r => r.Id == data.Id).Update(updatedSubscriber);

                if (Bot.Contact().cache.Any(r => r.Id == data.Id))
                    Bot.Contact().cache.FirstOrDefault(r => r.Id == data.Id).Update(updatedSubscriber);

                Bot.On.Emit(Command, updatedSubscriber);
            }
            catch (Exception d)
            {
                Bot._eventHandler.Emit(Internal.INTERNAL_ERROR, d.ToString());
            }
        }
    }
}