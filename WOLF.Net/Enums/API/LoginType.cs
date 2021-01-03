using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.API
{
    [Flags]
    public enum LoginType
    {
        Email = 1,
        Google = 2,
        Facebook = 3,
        Twitter = 4,
        SnapChat = 5,
        Apple = 6
    }
}