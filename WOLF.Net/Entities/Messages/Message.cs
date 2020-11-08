using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Messages;

namespace WOLF.Net.Entities.Messages
{
    public class Message
    {
        public MessageType MessageType { get; set; }

        public int SourceSubscriberId { get; set; }

        public int SourceTargetId { get; set; }

        public string Content { get; set; }
    }
}
