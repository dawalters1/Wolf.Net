using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Tipping;

namespace WOLF.Net.Client.Events.Handlers
{
    public class Tip : Event<Entities.Tipping.Tip>
    {
        public override string Command => Event.TIP_ADD;

        public override void HandleAsync(Entities.Tipping.Tip data) => Bot.On.Emit(Request.TIP_ADD, data);

        public override void Register()
        {
            Client.On<Response<Entities.Tipping.Tip>>(Command, resp => HandleAsync(resp.Body));
        }
    }
}