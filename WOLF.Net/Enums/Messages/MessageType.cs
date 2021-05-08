using System;

namespace WOLF.Net.Enums.Messages
{
    public enum MessageType
    {
        GROUP = 1,
        [Obsolete("use GROUP")]
        Group = GROUP,

        PRIVATE = 2,
        [Obsolete("use PRIVATE")]
        Private = PRIVATE,

        BOTH = GROUP | PRIVATE,
        [Obsolete("use BOTH")]
        Both = BOTH
    }
}