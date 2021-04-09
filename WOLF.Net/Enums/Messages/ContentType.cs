using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Messages
{
    [Flags]
    public enum ContentType
    {
        UNKNOWN = 0,

        TEXT = 1,

        IMAGE = 2,

        MESSAGE_PACK = 4,

        VOICE_MESSAGE = 8,

        GROUP_ACTION = 16,

        PRIVATE_REQUEST_RESPONSE = 32
    }
}
