﻿using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Misc;

namespace WOLF.Net.Client.Events.Handlers
{
    public class GroupUpdate : Event<IdHash>
    {
        public override string Command => Event.GROUP_UPDATE;

        public override void HandleAsync(IdHash data)
        {
            throw new NotImplementedException();
        }
    }
}
