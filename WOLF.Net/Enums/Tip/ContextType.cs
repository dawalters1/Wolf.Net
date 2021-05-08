using System;

namespace WOLF.Net.Enums.Tip
{
    [Flags]
    public enum ContextType
    {
        MESSAGE,
        [Obsolete("use MESSAGE")]
        Message = MESSAGE,

        STAGE,
        [Obsolete("use STAGE")]
        Stage = STAGE
    }
}
