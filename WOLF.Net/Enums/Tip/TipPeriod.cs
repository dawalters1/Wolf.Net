using System;

namespace WOLF.Net.Enums.Tip
{
    [Flags]
    public enum TipPeriod
    {
        ALLTIME,
        [Obsolete("use ALLTIME")]
        AllTime = ALLTIME,

        DAY,
        [Obsolete("use DAY")]
        Day = DAY,

        WEEK,
        [Obsolete("use WEEK")]
        Week = WEEK,

        MONTH,
        [Obsolete("use MONTH")]
        Month = MONTH
    }
}