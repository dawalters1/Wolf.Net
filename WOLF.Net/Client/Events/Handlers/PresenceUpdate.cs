using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Subscribers;

namespace WOLF.Net.Client.Events.Handlers
{
    public class PresenceUpdate:Event<Entities.Subscribers.PresenceUpdate>
    {
        public override string Command => Event.PRESENCE_UPDATE;

        public override async void HandleAsync(Entities.Subscribers.PresenceUpdate data)
        {
            foreach (var group in Bot.Groups.ToList())
                if (group.Users.Any(r => r.Id == data.Id))
                    group.Users.FirstOrDefault(r => r.Id == data.Id).AdditionalInfo.Update(data.OnlineState);

            if (Bot.Contacts.Any(r => r.Id == data.Id))
                Bot.Contacts.FirstOrDefault(r => r.Id == data.Id).AdditionalInfo.Update(data.OnlineState);

            if (Bot.Subscribers.Any(r => r.Id == data.Id))
                Bot.Subscribers.FirstOrDefault(r => r.Id == data.Id).Update(data);

            Bot.On.Emit(Command, await Bot.GetSubscriberAsync(data.Id), data);
        }
    }
}
