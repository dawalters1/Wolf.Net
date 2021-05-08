using System;

namespace WOLF.Net.Enums.Subscribers
{
    [Flags]
    public enum Gender
    {
        NOT_SPECIFIED = 0,
        [Obsolete("use NOT_SPECIFIED")]
        NotSpecified = NOT_SPECIFIED,

        MALE = 1,
        [Obsolete("use MALE")]
        Male = MALE,

        FEMALE = 2,
        [Obsolete("use FEMALE")]
        Female = FEMALE,
    }
}
