using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Client.Events
{

    public interface IEvent
    {
        WolfBot Bot { get; set; }

        WolfClient Client { get; set; }

        string Command { get; }

        void Register();
    }
}
