using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<string> CommandLanguages { get; set; } = new List<string>();

        public string SubLanguage => CommandLanguages.LastOrDefault();

        public int TargetGroupId => IsGroup && Group!=null? Group.Id : 0;

        public int SourceSubscriberId => Subscriber != null ? Subscriber.Id : 0;

        public bool IsGroup { get; set; }

        public string Argument { get; set; }

        public Group Group { get; set; }

        public Entities.Subscribers.Subscriber Subscriber { get; set; }

        public MessageType MessageType { get; set; }
       
        internal CommandData() { }

        internal CommandData(Message message)
        {
            MessageType = message.MessageType;
            Argument = message.Content.Trim();
            IsGroup = message.IsGroup;
        }
    }
}
