using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Subscribers
{
    [Flags]
    public enum DeviceType
    {
        OTHER,

        BOT = 1,

        IPHONE = 5,

        IPAD = 6,

        ANDROID = 7,

        WEB = 8
    }
}
