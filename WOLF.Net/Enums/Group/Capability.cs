using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Group
{
    public enum Capability
    {
        NotUser = -1,

        Regular = 0,

        Admin = 1,

        Mod = 2,

        Banned = 4,

        Silenced= 8,

        Owner = 16
    }
}
