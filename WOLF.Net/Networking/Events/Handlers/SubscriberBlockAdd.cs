using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Networking.Events.Handlers
{
    public class SubscriberBlockAdd : BaseEvent<ContactUpdate>
    {
        public override string Command => Event.SUBSCRIBER_BLOCK_ADD;
        public override bool ReturnBody => true;

        public override async void Handle(ContactUpdate data)
        {
            try
            {
                Bot.On.Emit(Command, await Bot.Blocked().Process(data.Id));
            }
            catch (Exception d)
            {
                Bot._eventHandler.Emit(Internal.INTERNAL_ERROR, d.ToString());
            }
        }
    }
}
