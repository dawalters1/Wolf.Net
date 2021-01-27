using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Messages
{
    [Flags]
    public enum MessageEditType
    {
        Delete = 1,

        Restore = 2,

        Edit = 4
    }
}
