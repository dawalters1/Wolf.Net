using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Subscribers
{
    [Flags]
    public enum Relationship
    {
        /// <summary>
        /// Subscriber has not specified their relationship status
        /// </summary>
        NOT_SPECIFIED = 0,

        /// <summary>
        /// Subscriber is single
        /// </summary>
        SINGLE = 1,

        /// <summary>
        /// Subscriber is in a relationship
        /// </summary>
        RELATIONSHIP = 2,

        /// <summary>
        /// Subscriber is engaged
        /// </summary>
        ENGAGED = 3,

        /// <summary>
        /// Subscriber is married
        /// </summary>
        MARRIED = 4,

        /// <summary>
        /// Complicated? sucks to be you
        /// </summary>
        COMPLICATED = 5,

        /// <summary>
        /// Subscriber is in an open relationship
        /// </summary>
        OPEN = 6
    }
}
