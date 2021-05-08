using System;

namespace WOLF.Net.Enums.Subscribers
{
    [Flags]
    public enum OnlineState
    {
        /// <summary>
        /// Subscriber is offline
        /// </summary>
        OFFLINE = 0,
        [Obsolete("use OFFLINE")]
        Offline = OFFLINE,

        /// <summary>
        /// Subscriber is online
        /// </summary>
        ONLINE = 1,
        [Obsolete("use ONLINE")]
        Online = ONLINE,

        /// <summary>
        /// Subscriber is away from the app
        /// </summary>
        AWAY = 2,
        [Obsolete("use AWAY")]
        Away = AWAY,

        /// <summary>
        /// Subscriber is online, but appearing offline
        /// </summary>
        INVISIBLE = 3,
        [Obsolete("use INVISIBLE")]
        Invisible = INVISIBLE,

        /// <summary>
        /// Subscriber is busy
        /// </summary>
        BUSY = 5,
        [Obsolete("use BUSY")]
        Busy = BUSY,

        /// <summary>
        /// Pointless ass state
        /// </summary>
        IDLE = 9,
        [Obsolete("use IDLE")]
        Idle = IDLE
    }
}