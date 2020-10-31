using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Subscriber
{
    public enum Privilege
    {
        /// <summary>
        /// Has an account duh
        /// </summary>
        SUBSCRIBER = 1, 
        
        /// <summary>
        /// User is a bot tester
        /// </summary>
        BOT_TESTER = 1 << 1, 

        /// <summary>
        /// User is a game tester (Obs)
        /// </summary>
        GAME_TESTER = 1 << 2,

        /// <summary>
        /// User submits content
        /// </summary>
        CONTENT_SUBMITER = 1 << 3,

        /// <summary>
        /// 
        /// </summary>
        SELECT_CLUB_1 = 1 << 4,

        // 1 << 5 - Unimportant tag not supporting

        /// <summary>
        /// Users whos level rank 1 through 3, Cannot be kicked silenced or banned unless the action is done by staff
        /// </summary>
        ELITE_CLUB_1 = 1 << 6,

        // 1 << 7 - Unimportant tag not supporting

        // 1 << 8 - Unimportant tag not supporting

        /// <summary>
        /// User is a volunteer
        /// </summary>
        VOLUNTEER = 1 << 9, 

        /// <summary>
        /// 
        /// </summary>
        SELECT_CLUB_2 = 1 << 10, 

        /// <summary>
        /// User is a client tester
        /// </summary>
        ALPHA_TESTER = 1 << 11, 

        /// <summary>
        /// User is a staff member
        /// </summary>
        STAFF = 1 << 12,

        /// <summary>
        /// User is a translator
        /// </summary>
        TRANSLATOR = 1 << 13,

        /// <summary>
        /// User is a developer
        /// </summary>
        DEVELOPER = 1 << 14,

        // 1 << 15 - Unimportant tag not supporting

        // 1 << 16 - Unimportant tag not supporting

        /// <summary>
        /// Users whos level ransk 4 through 10
        /// </summary>
        ELITE_CLUB_2 = 1 << 17,

        /// <summary>
        /// User is a pest
        /// </summary>
        PEST = 1 << 18, // pest
        
        /// <summary>
        /// User has validated their email
        /// </summary>
        VALID_EMAIL = 1 << 19,

        /// <summary>
        /// User has a premium account
        /// </summary>
        PREMIUM_ACCT = 1 << 20,

        /// <summary>
        /// User is a vip
        /// </summary>
        VIP = 1 << 21,

        /// <summary>
        /// Users whos account rank 11 through 20
        /// </summary>
        ELITE_CLUB_3 = 1 << 22,

        // 1 << 23 - Unimportant tag not supporting

        /// <summary>
        /// ??
        /// </summary>
        USER_ADMIN = 1 << 24, 

        /// <summary>
        /// User is 'super admin'
        /// </summary>
        GROUP_ADMIN = 1 << 25, 

        /// <summary>
        /// User is a bot
        /// </summary>
        BOT = 1 << 26,

        // 1 << 27 - Unimportant tag not supporting

        // 1 << 28 - Unimportant tag not supporting

        /// <summary>
        /// User is an 'entertainer'
        /// </summary>
        ENTERTAINER = 1 << 29,

        /// <summary>
        /// User is shadow banned
        /// </summary>
        SHADOW_BANNED = 1 << 30, 
    }
}