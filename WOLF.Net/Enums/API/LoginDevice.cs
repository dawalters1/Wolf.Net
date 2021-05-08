using System;

namespace WOLF.Net.Enums.API
{
    [Flags]
    public enum LoginDevice
    {
        IPHONE = 5,
        [Obsolete("use IPHONE")]
        iPhone = IPHONE,

        IPAD = 6,
        [Obsolete("use IPHONE")]
        iPad = IPAD,

        ANDROID = 7,
        [Obsolete("use ANDROID")]
        Android = ANDROID,

        WEB = 8,
        [Obsolete("use WEB")]
        Web = WEB,
    }
}