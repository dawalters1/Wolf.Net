using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Commands.Commands
{ 
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited =true)]
    public class Command : Attribute
    {
        internal bool AuthOnly { get; set; }

        internal string Trigger { get; set; }

        internal MessageType MessageType { get; set; }

        internal Capability Capability { get; set; }

        internal List<Privilege> Privileges { get; set; }

        public Command()
        {

        }

        public Command(string trigger)
        {
            Trigger = trigger;
        }

        internal Command(string trigger, MessageType messageType = MessageType.Both, Capability capability = Capability.None, List<Privilege> privileges = null, bool authOnly = false)
        {
            Trigger = trigger;
            MessageType = messageType;
                Capability = capability;
            Privileges = privileges;
        }
    }
}
