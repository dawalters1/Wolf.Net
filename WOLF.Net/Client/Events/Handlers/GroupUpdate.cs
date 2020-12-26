﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Client.Events.Handlers
{
    public class GroupUpdate : Event<IdHash>
    {
        public override string Command => Event.GROUP_UPDATE;

        public override async void HandleAsync(IdHash data)
        {
            var group = await Bot.GetGroupAsync(data.Id);

            if (group == null)
                return;

            Bot.On.Emit(Command, await Bot.GetGroupAsync(data.Id, true));
        }
        public override void Register()
        {
            Client.On<Response<IdHash>>(Command, resp => HandleAsync(resp.Body));
        }
    }
}
