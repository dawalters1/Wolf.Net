using System;
using WOLF.Net.Constants;

namespace WOLF.Net.Networking.Events.Handlers
{
    public class MessageUpdate : BaseEvent<Entities.Messages.MessageUpdate>
    {
        public override string Command => Event.MESSAGE_UPDATE;

        public override bool ReturnBody => true;

        public override void Handle(Entities.Messages.MessageUpdate data)
        {
            try
            {
                Bot.On.Emit(Event.MESSAGE_UPDATE, data);
            }
            catch (Exception d)
            {
                Bot.On.Emit(Internal.ERROR, d.ToString());
            }
        }
    }
}