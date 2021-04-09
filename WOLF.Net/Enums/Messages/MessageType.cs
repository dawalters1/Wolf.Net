using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Messages
{
    public enum MessageType
    {
        GROUP = 1,
        PRIVATE = 2,
        BOTH = GROUP|PRIVATE
    }
}
