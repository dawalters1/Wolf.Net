using System;

namespace WOLF.Net.Enums.Subscribers
{
    [Flags]
    public enum DeviceType
    {
        OTHER,
        [Obsolete("use OTHER")]
        Other = OTHER,

        BOT = 1,
        [Obsolete("use BOT")]
        Bot = BOT,

        IPHONE = 5,
        [Obsolete("use IPHONE")]
        iPhone = IPHONE,

        IPAD = 6,
        [Obsolete("use IPAD")]
        iPad = IPAD,

        ANDROID = 7,
        [Obsolete("use ANDROID")]
        Android = ANDROID,

        WEB = 8,
        [Obsolete("use WEB")]
        Web = WEB,
    }
}
