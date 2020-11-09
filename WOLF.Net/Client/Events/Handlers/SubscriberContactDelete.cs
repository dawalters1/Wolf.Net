using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Contacts;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Client.Events.Handlers
{
    public class SubscriberContactDelete : Event<ContactUpdate>
    {
        public override string Command => Event.SUBSCRIBER_CONTACT_DELETE;

        public override void HandleAsync(ContactUpdate data)
        {
            throw new NotImplementedException();
        }

    }
}
