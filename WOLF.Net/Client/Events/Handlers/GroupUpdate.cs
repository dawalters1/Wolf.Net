using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Client.Events.Handlers
{
    public class GroupUpdate : Event<IdHash>
    {
        public override string Command => Event.GROUP_UPDATE;

        public override async void HandleAsync(IdHash data)
        {
            Bot.On.Emit(Command, await Bot.GetGroupAsync(data.Id, data.Hash != Bot.Groups.FirstOrDefault(r => r.Id == data.Id).Hash));
        }
    }
}
