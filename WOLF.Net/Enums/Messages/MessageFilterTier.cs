using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Messages
{
    [Flags]
    public enum MessageFilterTier
    {
        Off = 0,

        Strict = 1,

        Recommended = 2,

        Relaxed = 3
    }
}
