using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Entities.Subscribers;
using WOLF.Net.Enums.Messages;

namespace WOLF.Net.Commands.Form
{
    public class FormData
    {
        public string Language { get; set; }

        public int SourceTargetId => IsGroup ? Group.Id : SourceSubscriberId;

        public int SourceSubscriberId => Subscriber != null ? Subscriber.Id : 0;

        public bool IsGroup { get; set; }

        public Group Group { get; set; }

        public Entities.Subscribers.Subscriber Subscriber { get; set; }

        public MessageType MessageType { get; set; }

        public FormData() { }

        public FormData(Message message)
        {
            MessageType = message.MessageType;
            IsGroup = message.IsGroup;
        }

        internal CommandData ToCommandData()
        {
            return new CommandData()
            {
                Subscriber = Subscriber,
                Group = Group,
                IsGroup = IsGroup,
                Language = Language,
                MessageType = MessageType
            };
        }
    }
}
