using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.API
{
    [Flags]
    public enum LoginType
    {
        EMAIL = 1,
        GOOGLE = 2,
        FACEBOOK = 3,
        TWITTER = 4,
        SNAPCHAT = 5,
        APPLE = 6
    }
}
