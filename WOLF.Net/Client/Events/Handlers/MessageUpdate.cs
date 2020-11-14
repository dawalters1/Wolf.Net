﻿using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Messages;

namespace WOLF.Net.Client.Events.Handlers
{
    public class MessageUpdate : Event<Entities.Messages.MessageUpdate>
    {
        public override string Command => Event.MESSAGE_UPDATE;

        public override void HandleAsync(Entities.Messages.MessageUpdate data)
        {
            throw new NotImplementedException();
        }
        public override void Register()
        {
            Client.On<Response<Entities.Messages.MessageUpdate>>(Command, resp => HandleAsync(resp.Body));
        }
    }
}
