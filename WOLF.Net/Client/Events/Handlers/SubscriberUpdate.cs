using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Client.Events.Handlers
{
    public class SubscriberUpdate : Event<IdHash>
    {
        public override string Command => Event.SUBSCRIBER_UPDATE;

        public override async void HandleAsync(IdHash data)
        {
            var subscriber = await Bot.GetSubscriberAsync(data.Id, 
                data.Hash != Bot.Subscribers.FirstOrDefault(r => r.Id == data.Id).Hash);

            foreach (var group in Bot.Groups.ToList())
                if (group.Users.Any(r => r.Id == data.Id))
                    group.Users.FirstOrDefault(r => r.Id == data.Id).Update(subscriber);

            if (Bot.Contacts.Any(r => r.Id == data.Id))
                Bot.Contacts.FirstOrDefault(r => r.Id == data.Id).Update(subscriber);

            Bot.On.Emit(Command, subscriber);
        }

        public override void Register()
        {
            Client.On<Response<IdHash>>(Command, resp => HandleAsync(resp.Body));
        }
    }
}
