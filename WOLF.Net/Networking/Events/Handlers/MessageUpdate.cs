using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Messages;

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
                Bot._eventHandler.Emit(Internal.ERROR, d.ToString());
            }
        }
    }
}