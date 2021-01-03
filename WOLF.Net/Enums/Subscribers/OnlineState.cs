using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Subscribers
{
    [Flags]
    public enum OnlineState
    {
        Offline = 0,

        Online = 1,

        Away = 2,

        Invisible = 3,

        Busy = 5,

        Idle = 9,
    }
}
