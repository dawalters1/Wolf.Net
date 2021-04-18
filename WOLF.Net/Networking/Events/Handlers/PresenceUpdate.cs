using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Subscribers;

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
                foreach (var group in Bot.Group().cache.ToList())
                    if (group.Subscribers.Any(r => r.Id == data.Id))
                        group.Subscribers.ToList().FirstOrDefault(r => r.Id == data.Id).AdditionalInfo.Update(data.OnlineState);

                if (Bot.Blocked().cache.Any(r => r.Id == data.Id))
                    Bot.Blocked().cache.FirstOrDefault(r => r.Id == data.Id).AdditionalInfo.Update(data.OnlineState);

                if (Bot.Contact().cache.Any(r => r.Id == data.Id))
                    Bot.Contact().cache.FirstOrDefault(r => r.Id == data.Id).AdditionalInfo.Update(data.OnlineState);

                if (Bot.Subscriber().cache.Any(r => r.Id == data.Id))
                    Bot.Subscriber().cache.FirstOrDefault(r => r.Id == data.Id).Update(data);

                Bot.On.Emit(Command, await Bot.Subscriber().GetByIdAsync(data.Id), data);
            }
            catch (Exception d)
            {
                Bot._eventHandler.Emit(Internal.ERROR, d.ToString());
            }
        }
    }
}