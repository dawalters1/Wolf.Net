using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Messages
{
    public enum ContentType
    {
        Unknown = 0,

        Text = 1,

        Image = 2,

        MessagePack = 4,

        VoiceMessage = 8,

        GroupAction = 16,

        PrivateRequestResponse = 32
    }
}
