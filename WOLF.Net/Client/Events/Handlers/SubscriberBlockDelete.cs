using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Contacts;

namespace WOLF.Net.Client.Events.Handlers
{
    public class SubscriberBlockDelete : Event<ContactUpdate>
    {
        public override string Command => Event.SUBSCRIBER_BLOCK_DELETE;

        public override async void HandleAsync(ContactUpdate data)
        {
            var subscriber = await Bot.GetSubscriberAsync(data.Id);

            if (Bot.Contacts.Any(r => r.Id == data.Id))
                Bot.Contacts.RemoveAll(r => r.Id == data.Id);

            Bot.On.Emit(Command, subscriber);
        }

    }
}
