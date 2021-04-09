using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Subscribers
{
    [Flags]
    public enum OnlineState
    {
        /// <summary>
        /// Subscriber is offline
        /// </summary>
        OFFLINE = 0,

        /// <summary>
        /// Subscriber is online
        /// </summary>
        ONLINE = 1,

        /// <summary>
        /// Subscriber is away from the app
        /// </summary>
        AWAY = 2,

        /// <summary>
        /// Subscriber is online, but appearing offline
        /// </summary>
        INVISIBLE = 3,

        /// <summary>
        /// Subscriber is busy
        /// </summary>
        BUSY = 5,

        /// <summary>
        /// Pointless ass state
        /// </summary>
        IDLE = 9,
    }
}
