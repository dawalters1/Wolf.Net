using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Entities.Subscribers;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Messages;

namespace WOLF.Net.Commands.Commands
{
    public class CommandData
    {
        public string Language { get; set; }

        public int SourceTargetId { get; set; }

        public int SourceSubscriberId { get; set; }

        public bool IsGroup { get; set; }

        public string Argument { get; set; }

        public Group Group { get; set; }

        public Subscriber Subscriber { get; set; }

        public MessageType MessageType { get; set; }

        public bool IsTranslation => Language != null;

        public CommandData(int sourceTargetId, int sourceSubscriberId, string content, bool isGroup)
        {
            SourceSubscriberId = sourceSubscriberId;
            SourceTargetId = sourceTargetId;
            Argument = content;
            IsGroup = isGroup;
            MessageType = isGroup ? MessageType.Group : MessageType.Private;
        }

        internal CommandData Clone()
        {
            return new CommandData(SourceTargetId, SourceSubscriberId, Argument, IsGroup);
        }
    }
}
