using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Subscribers
{
    [Flags]
    public enum DeviceType
    {
        Other,

        Bot = 1,

        iPhone = 5,

        iPad = 6,

        Android = 7,

        Web = 8
    }
}
