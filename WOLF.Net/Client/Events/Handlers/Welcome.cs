using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;

namespace WOLF.Net.Client.Events.Handlers
{
    public class Welcome : Event<Entities.API.Welcome>
    {
        public override string Command => Event.WELCOME;

        public override void HandleAsync(Entities.API.Welcome data)
        {
            throw new NotImplementedException();
        }
    }
}
