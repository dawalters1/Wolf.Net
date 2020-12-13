using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WOLF.Net.Commands.Instances;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Commands.Commands
{
    [AttributeUsage(AttributeTargets.Method|AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class Command : Attribute
    {
        internal string Trigger { get; set; }

        // House methods
        internal List<MethodInstance<Command>> MethodInstances = new List<MethodInstance<Command>>();

        // House collections
        internal List<TypeInstance<Command>> TypeInstances = new List<TypeInstance<Command>>();

        public Command()
        {

        }

        public Command(string trigger)
        {
            Trigger = trigger;
        }
    }
}