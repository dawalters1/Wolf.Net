using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WOLF.Net.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public abstract class BaseAttribute : Attribute
    {
        public abstract bool Validate(WolfBot bot, CommandData command);
    }
}
