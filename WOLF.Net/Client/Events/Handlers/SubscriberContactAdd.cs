using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Contacts;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Client.Events.Handlers
{
    public class SubscriberContactAdd : Event<ContactUpdate>
    {
        public override string Command => Event.SUBSCRIBER_CONTACT_ADD;

        public override async void HandleAsync(ContactUpdate data)
        {
            var subscriber = await Bot.GetSubscriberAsync(data.Id);

            Bot.ProcessContact(new Contact(subscriber, false));

            Bot.On.Emit(Command, subscriber);
        }
    }
}
