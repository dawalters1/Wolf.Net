using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Enums.Subscribers
{
    public enum Relationship
    {
        /// <summary>
        /// User has not specified their relationship status
        /// </summary>
        NotSpecified = 0,

        /// <summary>
        /// User is single
        /// </summary>
        Single = 1,

        /// <summary>
        /// User is in a relationship
        /// </summary>
        Relationship = 2,

        /// <summary>
        /// User is engaged
        /// </summary>
        Engaged = 3,

        /// <summary>
        /// User is married
        /// </summary>
        Married = 4,

        /// <summary>
        /// Complicated? sucks to be you
        /// </summary>
        Compilicated = 5,

        /// <summary>
        /// User is in an open relationship
        /// </summary>
        Open = 6
    }
}