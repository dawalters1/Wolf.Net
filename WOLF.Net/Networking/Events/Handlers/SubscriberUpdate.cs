using System;
using System.Linq;
using WOLF.Net.Constants;
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
                var subscriber = Bot.Subscribers.FirstOrDefault(r => r.Id == data.Id);

                if (subscriber == null || data.Hash == subscriber.Hash)
                    return;

                var updatedSubscriber = await Bot.GetSubscriberAsync(data.Id, true);

                foreach (var group in Bot.Groups.ToList())
                    if (group.Subscribers.Any(r => r.Id == data.Id))
                        group.Subscribers.FirstOrDefault(r => r.Id == data.Id).Update(updatedSubscriber);

                if (Bot.blockedCache.Any(r => r.Id == data.Id))
                    Bot.blockedCache.FirstOrDefault(r => r.Id == data.Id).Update(updatedSubscriber);

                if (Bot.contactCache.Any(r => r.Id == data.Id))
                    Bot.contactCache.FirstOrDefault(r => r.Id == data.Id).Update(updatedSubscriber);

                Bot.On.Emit(Command, updatedSubscriber);
            }
            catch (Exception d)
            {
                Bot.On.Emit(Internal.ERROR, d.ToString());
            }
        }
    }
}