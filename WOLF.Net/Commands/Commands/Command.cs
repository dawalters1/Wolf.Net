using System;
using System.Collections.Generic;
using WOLF.Net.Commands.Instances;

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