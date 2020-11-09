using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Messages;

namespace WOLF.Net.Client.Events.Handlers
{
    public class MessageSend : Event<Message>
    {
        public override string Command => Event.MESSAGE_SEND;

        public override void HandleAsync(Message data)
        {
            throw new NotImplementedException();
        }
    }
}
