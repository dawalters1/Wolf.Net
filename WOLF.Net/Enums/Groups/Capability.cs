using System;

namespace WOLF.Net.Enums.Groups
{
    [Flags]
    public enum Capability
    {
        /// <summary>
        /// Subscriber is not in the group
        /// </summary>
        NOT_MEMBER = -1,
        [Obsolete("use NOT_MEMBER")]
        None = NOT_MEMBER,

        /// <summary>
        /// Subscriber is a regular group subscriber
        /// </summary>
        REGULAR = 0,
        [Obsolete("use REGULAR")]
        Regular = REGULAR,

        /// <summary>
        /// Subscriber is an admin
        /// </summary>
        ADMIN = 1,
        [Obsolete("use ADMIN")]
        Admin = ADMIN,

        /// <summary>
        /// Subscriber is a mod
        /// </summary>
        MOD = 2,
        [Obsolete("use MOD")]
        Mod = MOD,

        /// <summary>
        /// Subscriber is banned from the group
        /// </summary>
        BANNED = 4,
        [Obsolete("use BANNED")]
        Banned = BANNED,

        /// <summary>
        /// Subscriber is silenced
        /// </summary>
        SILENCED = 8,
        [Obsolete("use SILENCED")]
        Silenced = SILENCED,

        /// <summary>
        /// Subscriber owns the group
        /// </summary>
        OWNER = 32,
        [Obsolete("use OWNER")]
        Owner = OWNER
    }
}