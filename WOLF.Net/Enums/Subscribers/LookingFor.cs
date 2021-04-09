using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Subscribers
{
    [Flags]
    public enum LookingFor
    {
        NOT_SPECIFIED = 0,

        FRIENDSHIP = 1,

        DATING = 2,

        RELATIONSHIP = 4,

        NETWORKING = 8
    }
}
