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

        IMAGE_LINK = 2,

        IMAGE_GIF = 4,

        IMAGE_JPEG = 8,

        MESSAGE_PACK = 16,

        VOICE_MESSAGE = 32,

        GROUP_ACTION = 64,

        PRIVATE_REQUEST_RESPONSE = 128
    }
}
