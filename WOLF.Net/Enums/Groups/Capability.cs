using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Groups
{
    [Flags]
    public enum Capability
    {
        /// <summary>
        /// Subscriber is not in the group
        /// </summary>
        NOT_MEMBER = -1,

        /// <summary>
        /// Subscriber is a regular group subscriber
        /// </summary>
        REGULAR = 0,

        /// <summary>
        /// Subscriber is an admin
        /// </summary>
        ADMIN = 1,

        /// <summary>
        /// Subscriber is a mod
        /// </summary>
        MOD = 2,

        /// <summary>
        /// Subscriber is banned from the group
        /// </summary>
        BANNED = 4,

        /// <summary>
        /// Subscriber is silenced
        /// </summary>
        SILENCED = 8,

        /// <summary>
        /// Subscriber owns the group
        /// </summary>
        OWNER = 32
    }
}
