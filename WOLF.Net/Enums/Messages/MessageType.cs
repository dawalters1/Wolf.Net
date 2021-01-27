using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Messages
{
    [Flags]
    public enum MessageType
    {
        Group = 1,
        Private = 2,
        Both = 4,
    }
}
