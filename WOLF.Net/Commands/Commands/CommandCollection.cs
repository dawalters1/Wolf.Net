using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using WOLF.Net.Commands.Attributes;
using WOLF.Net.Commands.Instances;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Enums.Subscribers;
using WOLF.Net.Utilities;

namespace WOLF.Net.Commands.Commands
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandCollection : Attribute
    {
        internal bool AuthOnly { get; set; }

        internal string Trigger { get; set; }

        internal MessageType MessageType { get; set; }

        internal Capability Capability { get; set; } 

        internal List<Privilege> Privileges { get; set; }

        internal MethodInstance<Command> Default { get; set; } = null;

        internal List<MethodInstance<Command>> ChildrenCommands { get; set; } 

        internal List<TypeInstance<CommandCollection>> ChildrenCollections { get; set; }

        public CommandCollection(string trigger)
        {
            Trigger = trigger;
        }

        internal CommandCollection(string trigger, MessageType messageType = MessageType.Both, Capability capability = Capability.None, List<Privilege> privileges = null, bool authOnly = false, List<MethodInstance<Command>> childrenCommands = null, List<TypeInstance<CommandCollection>> childrenCollections = null)
        {
            Trigger = trigger;
            AuthOnly = authOnly;
            MessageType = messageType;
            Capability = capability;
            Privileges = privileges;
            ChildrenCommands = childrenCommands != null ? childrenCommands.Where(r=>!string.IsNullOrWhiteSpace(r.Value.Trigger)).ToList() : new List<MethodInstance<Command>>();
            ChildrenCollections = childrenCollections != null ? childrenCollections : new List<TypeInstance<CommandCollection>>();
            Default = childrenCommands.FirstOrDefault(r => string.IsNullOrWhiteSpace(r.Value.Trigger));
        }
    }
}