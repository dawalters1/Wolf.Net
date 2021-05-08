using System;

namespace WOLF.Net.Enums.Tip
{
    [Flags]
    public enum TipType
    {
        SUBSCRIBER,
        [Obsolete("use SUBSCRIBER")]
        Subscriber = SUBSCRIBER,

        CHARM,
        [Obsolete("use CHARM")]
        Charm = CHARM,

        GROUP,
        [Obsolete("use GROUP")]
        Group = GROUP,
    }
}