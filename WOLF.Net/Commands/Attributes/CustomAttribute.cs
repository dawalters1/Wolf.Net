using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Commands;

namespace WOLF.Net.Commands.Attributes
{
    /// <summary>
    /// Use this for custom command attribute checks
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public abstract class CustomAttribute : Attribute
    {
        public abstract Task<bool> Validate(WolfBot bot, CommandData commandData);
    }
}
