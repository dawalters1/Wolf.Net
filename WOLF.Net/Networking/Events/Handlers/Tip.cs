using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;

namespace WOLF.Net.Networking.Events.Handlers
{
    public class Tip : BaseEvent<Entities.Tip.Tip>
    {
        public override string Command => Event.TIP_ADD;

        public override bool ReturnBody => true;

        public override void Handle(Entities.Tip.Tip data) => Bot.On.Emit(Request.TIP_ADD, data);
    }
}