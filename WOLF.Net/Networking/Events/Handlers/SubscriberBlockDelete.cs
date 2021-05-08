using System;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Networking.Events.Handlers
{
    public class SubscriberBlockDelete : BaseEvent<ContactUpdate>
    {
        public override string Command => Event.SUBSCRIBER_BLOCK_DELETE;
        public override bool ReturnBody => true;

        public override async void Handle(ContactUpdate data)
        {
            try
            {
                Bot.On.Emit(Command, await Bot.ProcessBlocked(data.Id));
            }
            catch (Exception d)
            {
                Bot.On.Emit(Internal.ERROR, d.ToString());
            }
        }
    }
}
