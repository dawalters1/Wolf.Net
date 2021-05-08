using System;

namespace WOLF.Net.Enums.Messages
{
    [Flags]
    public enum ContentType
    {
        UNKNOWN = 0,
        [Obsolete("use UNKNOWN")]
        Unknown = UNKNOWN,

        TEXT = 1,
        [Obsolete("use TEXT")]
        Text = TEXT,

        IMAGE_LINK = 2,
        [Obsolete("use IMAGE_LINK")]
        Image = IMAGE_LINK,

        IMAGE_GIF = 4,

        IMAGE_JPEG = 8,

        MESSAGE_PACK = 16,
        [Obsolete("use MESSAGE_PACK")]
        MessagePack = MESSAGE_PACK,

        VOICE_MESSAGE = 32,
        [Obsolete("use VOICE_MESSAGE")]
        VoiceMessage = VOICE_MESSAGE,

        GROUP_ACTION = 64,
        [Obsolete("use GROUP_ACTION")]
        GroupAction = GROUP_ACTION,

        PRIVATE_REQUEST_RESPONSE = 128,
        [Obsolete("use PRIVATE_REQUEST_RESPONSE")]
        PrivateRequestResponse = PRIVATE_REQUEST_RESPONSE,
    }
}
