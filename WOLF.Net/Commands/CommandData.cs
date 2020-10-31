using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Group;
using WOLF.Net.Entities.Subscriber;
using WOLF.Net.Enums.Group;
using WOLF.Net.Enums.Message;

namespace WOLF.Net.Commands
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
    }
}
