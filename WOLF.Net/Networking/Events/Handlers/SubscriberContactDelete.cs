using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Networking.Events.Handlers
{
    public class SubscriberContactDelete : BaseEvent<ContactUpdate>
    {
        public override string Command => Event.SUBSCRIBER_CONTACT_DELETE;
        public override bool ReturnBody => true;
        public override async void Handle(ContactUpdate data)
        {
            try
            {
                Bot.On.Emit(Command, await Bot.Contact().Process(data.Id));
            }
            catch (Exception d)
            {
                Bot._eventHandler.Emit(Internal.INTERNAL_ERROR, d.ToString());
            }
        }
    }
}
