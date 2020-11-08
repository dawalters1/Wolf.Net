using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WOLF.Net.Commands.Commands
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited =true)]
    public class Command : Attribute
    {
        public string Trigger { get; set; }
        public Command(string trigger)
        {
            Trigger = trigger;
        }

        internal Command Clone(string trigger)
        {
            return new Command(trigger);
        }
    }
}
