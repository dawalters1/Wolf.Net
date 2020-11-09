using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Client;
using WOLF.Net.Client.Events;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        public WolfClient WolfClient { get; private set; }

        public EventManager On { get; private set; }
        
        public WolfBot(){
            //Check to see if Config/Config.xyz exists

            WoflClient = new WolfClient();
            On = new EventManager();
        }
    }
}
