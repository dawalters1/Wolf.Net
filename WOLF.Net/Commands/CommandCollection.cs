using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandCollection:Attribute
    {
        internal string Language { get; set; } = null;

        public string Trigger { get; set; }     
        
        public CommandCollection(string trigger)
        {
            Trigger = trigger;
        }

        internal CommandCollection Clone(string trigger, string language = null)
        {
            return new CommandCollection(trigger)
            {
                Language = language
            };
        }
    }
}
