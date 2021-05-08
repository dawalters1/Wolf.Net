using System;

namespace WOLF.Net.Enums.Subscribers
{
    [Flags]
    public enum LookingFor
    {
        NOT_SPECIFIED = 0,
        [Obsolete("use NOT_SPECIFIED")]
        NotSpecified = NOT_SPECIFIED,

        FRIENDSHIP = 1,
        [Obsolete("use FRIENDSHIP")]
        Friendship = FRIENDSHIP,

        DATING = 2,
        [Obsolete("use DATING")]
        Dating = DATING,

        RELATIONSHIP = 4,
        [Obsolete("use RELATIONSHIP")]
        Relationship = RELATIONSHIP,

        NETWORKING = 8,
        [Obsolete("use NETWORKING")]
        Networking = NETWORKING
    }
}