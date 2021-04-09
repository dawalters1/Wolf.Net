using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Commands;

namespace WOLF.Net.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class RequiredMessageType : CustomAttribute
    {
        internal Enums.Messages.MessageType MessageType;

        public RequiredMessageType(Enums.Messages.MessageType messageType)
        {
            MessageType = messageType;
        }

        public override Task<bool> Validate(WolfBot bot, CommandData commandData)
        {
            if (MessageType == Enums.Messages.MessageType.BOTH)
                return Task.FromResult(true);

            return Task.FromResult(commandData.MessageType == MessageType);
        }
    }
}