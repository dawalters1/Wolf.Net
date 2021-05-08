using System;

namespace WOLF.Net.Enums.Subscribers
{
    [Flags]
    public enum Relationship
    {
        /// <summary>
        /// Subscriber has not specified their relationship status
        /// </summary>
        NOT_SPECIFIED = 0,
        [Obsolete("use NOT_SPECIFIED")]
        NotSpecified = NOT_SPECIFIED,

        /// <summary>
        /// Subscriber is single
        /// </summary>
        SINGLE = 1,
        [Obsolete("use SINGLE")]
        Single = SINGLE,

        /// <summary>
        /// Subscriber is in a relationship
        /// </summary>
        RELATIONSHIP = 2,
        [Obsolete("use RELATIONSHIP")]
        Relationship = RELATIONSHIP,

        /// <summary>
        /// Subscriber is engaged
        /// </summary>
        ENGAGED = 3,
        [Obsolete("use ENGAGED")]
        Engaged = ENGAGED,

        /// <summary>
        /// Subscriber is married
        /// </summary>
        MARRIED = 4,
        [Obsolete("use MARRIED")]
        Married = MARRIED,

        /// <summary>
        /// Complicated? sucks to be you
        /// </summary>
        COMPLICATED = 5,
        [Obsolete("use COMPLICATED")]
        Complicated = COMPLICATED,

        /// <summary>
        /// Subscriber is in an open relationship
        /// </summary>
        OPEN = 6,
        [Obsolete("use OPEN")]
        Open = OPEN,
    }
}