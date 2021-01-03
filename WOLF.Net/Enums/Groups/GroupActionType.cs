using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Groups
{
    [Flags]
    public enum GroupActionType
    {
        Regular = 0,

        Admin = 1,

        Mod = 2,

        Ban = 4,

        Silence = 8,

        Kick = 16,

        Join = 17,

        Leave = 18,

        Owner = 32,
    }
}