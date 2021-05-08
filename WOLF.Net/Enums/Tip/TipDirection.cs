using System;

namespace WOLF.Net.Enums.Tip
{
    [Flags]
    public enum TipDirection
    {
        SENT,
        [Obsolete("use SENT")]
        Sent = SENT,

        RECEIVED,
        [Obsolete("use RECIEVED")]
        Recieved = RECEIVED
    }
}
