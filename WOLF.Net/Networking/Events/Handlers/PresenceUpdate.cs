using System;
using System.Linq;
using WOLF.Net.Constants;

namespace WOLF.Net.Networking.Events.Handlers
{
    public class PresenceUpdate : BaseEvent<Entities.Subscribers.PresenceUpdate>
    {
        public override string Command => Event.PRESENCE_UPDATE;

        public override bool ReturnBody => true;
        public override async void Handle(Entities.Subscribers.PresenceUpdate data)
        {
            try
            {
                foreach (var group in Bot.Groups.ToList())
                    if (group.Subscribers.Any(r => r.Id == data.Id))
                        group.Subscribers.ToList().FirstOrDefault(r => r.Id == data.Id).AdditionalInfo.Update(data.OnlineState);

                if (Bot.blockedCache.Any(r => r.Id == data.Id))
                    Bot.blockedCache.FirstOrDefault(r => r.Id == data.Id).AdditionalInfo.Update(data.OnlineState);

                if (Bot.contactCache.Any(r => r.Id == data.Id))
                    Bot.contactCache.FirstOrDefault(r => r.Id == data.Id).AdditionalInfo.Update(data.OnlineState);

                if (Bot.Subscribers.Any(r => r.Id == data.Id))
                    Bot.Subscribers.FirstOrDefault(r => r.Id == data.Id).Update(data);

                Bot.On.Emit(Command, await Bot.GetSubscriberAsync(data.Id), data);
            }
            catch (Exception d)
            {
                Bot.On.Emit(Internal.ERROR, d.ToString());
            }
        }
    }
}