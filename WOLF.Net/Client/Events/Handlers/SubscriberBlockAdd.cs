using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Contacts;

namespace WOLF.Net.Client.Events.Handlers
{
    public class SubscriberBlockAdd : Event<ContactUpdate>
    {
        public override string Command => Event.SUBSCRIBER_BLOCK_ADD;

        public override async void HandleAsync(ContactUpdate data)
        {
            var subscriber = await Bot.GetSubscriberAsync(data.Id);

            Bot.ProcessContact(new Contact(subscriber, true));

            Bot.On.Emit(Command, subscriber);
        }

    }
}
