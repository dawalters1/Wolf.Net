﻿using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Client;
using WOLF.Net.Client.Events;
using WOLF.Net.Entities.API;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        public WolfClient WolfClient { get; private set; }

        public EventManager On { get; private set; }
        
        public LoginData LoginData { get; private set; }

        public WolfBot(){
            //Check to see if Config/Config.xyz exists

            On = new EventManager();
        }
    }
}
