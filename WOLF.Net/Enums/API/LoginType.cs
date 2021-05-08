using System;

namespace WOLF.Net.Enums.API
{
    [Flags]
    public enum LoginType
    {
        EMAIL = 1,
        [Obsolete("use EMAIL")]
        Email = EMAIL,

        GOOGLE = 2,
        [Obsolete("use GOOGLE")]
        Google = GOOGLE,

        FACEBOOK = 3,
        [Obsolete("use FACEBOOK")]
        Facebook = FACEBOOK,

        TWITTER = 4,
        [Obsolete("use TWITTER")]
        Twitter = TWITTER,

        SNAPCHAT = 5,
        [Obsolete("use SNAPCHAT")]
        SnapChat = SNAPCHAT,

        APPLE = 6,
        [Obsolete("use APPLE")]
        Apple = APPLE
    }
}
