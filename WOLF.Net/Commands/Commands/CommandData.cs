using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Entities.Subscribers;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Messages;

namespace WOLF.Net.Commands.Commands
{
    public class CommandData
    {
        public string Language { get; set; }


        [Obsolete("This property will be deprecated soon, please use SourceTargetId", true)]
        public int ReturnAddres => SourceTargetId;

        public int SourceTargetId => IsGroup ? Group.Id : SourceSubscriberId;

        [Obsolete("This property will be deprecated soon, please use SourceSubscriberId", true)]
        public int UserId => SourceSubscriberId;

        public int SourceSubscriberId => Subscriber != null ? Subscriber.Id : 0;

        public bool IsGroup { get; set; }

        public string Argument { get; set; }

        public Group Group { get; set; }

        public Subscriber Subscriber { get; set; }

        public MessageType MessageType { get; set; }
       
        public CommandData() { }

        public CommandData(Message message)
        {
            MessageType = message.MessageType;
            Argument = message.Content;
            IsGroup = message.IsGroup;
        }
    }
}
