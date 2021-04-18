using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Networking.Events.Handlers
{
    public class GroupUpdate : BaseEvent<IdHash>
    {
        public override string Command => Event.GROUP_UPDATE;

        public override bool ReturnBody => true;

        public override void Handle(IdHash data)
        {
            try
            {

            }
            catch (Exception d)
            {
                Bot._eventHandler.Emit(Internal.ERROR, d.ToString());
            }
        }
    }
}