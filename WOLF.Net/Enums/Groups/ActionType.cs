using System;

namespace WOLF.Net.Enums.Groups
{
    [Flags]
    public enum ActionType
    {
        /// <summary>
        /// Reset a subscriber so they are a regular subscriber
        /// </summary>
        REGULAR = 0,
        [Obsolete("use REGULAR")]
        Regular = REGULAR,

        /// <summary>
        /// Make a subscriber an admin
        /// </summary>
        ADMIN = 1,
        [Obsolete("use ADMIN")]
        Admin = ADMIN,

        /// <summary>
        /// Make a subscriber a moderator
        /// </summary>
        MOD = 2,
        [Obsolete("use MOD")]
        Mod = MOD,

        /// <summary>
        /// Banned a subscriber from the group
        /// </summary>
        BAN = 4,
        [Obsolete("use BAN")]
        Ban = BAN,

        /// <summary>
        /// Silence a subscriber so they cannot talk
        /// </summary>
        SILENCE = 8,
        [Obsolete("use SILENCE")]
        Silence = SILENCE,

        /// <summary>
        /// Remove a subscriber from the group
        /// </summary>
        KICK = 16,
        [Obsolete("use KICK")]
        Kick = KICK,

        /// <summary>
        /// Not a vaild action
        /// </summary>
        JOIN = 17,
        [Obsolete("use JOIN")]
        Join = JOIN,

        /// <summary>
        /// Not a vaild action
        /// </summary>
        LEAVE = 18,
        [Obsolete("use LEAVE")]
        Leave = LEAVE,

        /// <summary>
        /// Not a vaild action
        /// </summary>
        OWNER = 32,
        [Obsolete("use OWNER")]
        Owner = OWNER
    }
}
