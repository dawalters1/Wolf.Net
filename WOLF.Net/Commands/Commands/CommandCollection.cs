using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WOLF.Net.Commands.Instances;
using WOLF.Net.Utilities;

namespace WOLF.Net.Commands.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandCollection : Attribute
    {
        internal string Language { get; set; } = "en";

        public string Trigger { get; set; }

        internal MethodInstance<Command> Default { get; set; } = null;
        internal List<MethodInstance<Command>> Commands { get; set; } = new List<MethodInstance<Command>>();

        internal List<TypeInstance<CommandCollection>> SubCollections { get; set; } = new List<TypeInstance<CommandCollection>>();

        public CommandCollection(string trigger)
        {
            Trigger = trigger;
        }
    }
}