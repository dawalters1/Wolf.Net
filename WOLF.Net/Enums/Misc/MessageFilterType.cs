using System;

namespace WOLF.Net.Enums.Misc
{
    public enum MessageFilterType
    {
        OFF = 0,
        [Obsolete("use OFF")]
        Off = OFF,

        RELAXED = 3,
        [Obsolete("use RELAXED")]
        Relaxed = RELAXED,

        RECOMMENDED = 2,
        [Obsolete("use RECOMMENDED")]
        Recommended = RECOMMENDED,

        STRICT = 1,
        [Obsolete("use STRICT")]
        Strict = STRICT,
    }
}