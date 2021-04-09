using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Groups
{
    [Flags]
    public enum ActionType
    {
        /// <summary>
        /// Reset a subscriber so they are a regular subscriber
        /// </summary>
        REGULAR = 0,

        /// <summary>
        /// Make a subscriber an admin
        /// </summary>
        ADMIN = 1,

        /// <summary>
        /// Make a subscriber a moderator
        /// </summary>
        MOD = 2,

        /// <summary>
        /// Banned a subscriber from the group
        /// </summary>
        BAN = 4,

        /// <summary>
        /// Silence a subscriber so they cannot talk
        /// </summary>
        SILENCE = 8,

        /// <summary>
        /// Remove a subscriber from the group
        /// </summary>
        KICK = 16,

        /// <summary>
        /// Not a vaild action
        /// </summary>
        JOIN = 17,

        /// <summary>
        /// Not a vaild action
        /// </summary>
        LEAVE = 18,

        /// <summary>
        /// Not a vaild action
        /// </summary>
        OWNER = 32,
    }
}
