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

        public override void HandleAsync(ContactUpdate data)
        {
            throw new NotImplementedException();
        }

        public override void Register()
        {
            Client.On<ContactUpdate>(Command, resp => HandleAsync(resp));
        }
    }
}
