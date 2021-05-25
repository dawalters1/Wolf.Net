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

        public override async void Handle(IdHash data)
        {
            try
            {
                var group = Bot.Group().cache.FirstOrDefault(r => r.Id == data.Id);

                if (group == null || data.Hash == group.Hash)
                    return;

                Bot.On.Emit(Command, await Bot.Group().GetByIdAsync(data.Id, true));
            }
            catch (Exception d)
            {
                Bot.On.Emit(Internal.ERROR, d.ToString());
            }
        }
    }
}