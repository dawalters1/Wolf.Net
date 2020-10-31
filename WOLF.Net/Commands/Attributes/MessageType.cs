using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WOLF.Net.Commands.Attributes
{
    public class MessageType : BaseAttribute
    {
        private Enums.Message.MessageType _messageType;

        public MessageType(Enums.Message.MessageType messageType)
        {
            _messageType = messageType;
        }
        public override bool Validate(WolfBot bot, CommandData command)
        {
            if (_messageType == Enums.Message.MessageType.Both)
                return true;

            return _messageType.HasFlag(command.MessageType);
        }
    }
}
