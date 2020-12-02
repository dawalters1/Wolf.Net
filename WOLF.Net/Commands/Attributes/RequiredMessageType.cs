using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Commands;

namespace WOLF.Net.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class RequiredMessageType : Attribute
    {
        internal Enums.Messages.MessageType _messageType;

        public RequiredMessageType(Enums.Messages.MessageType messageType)
        {
            _messageType = messageType;
        }
    }
}
