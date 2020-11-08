using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Commands;

namespace WOLF.Net.Commands.Attributes
{
    public class MessageType : BaseAttribute
    {
        private Enums.Messages.MessageType _messageType;

        public MessageType(Enums.Messages.MessageType messageType)
        {
            _messageType = messageType;
        }
        public override bool Validate(WolfBot bot, CommandData command)
        {
            if (_messageType == Enums.Messages.MessageType.Both)
                return true;

            return _messageType.HasFlag(command.MessageType);
        }
    }
}
