using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Example.Entities
{
    public class FormData
    {
        public FormData(int sourceTargetId, int sourceSubscriberId, string language)
        {
            SourceTargetId = sourceTargetId;
            SourceSubscriberId = sourceSubscriberId;
            Language = language;
        }

        public int SourceTargetId { get; set; }

        public int SourceSubscriberId { get; set; }

        public string Language { get; set; }

        public int Stage { get; set; } = 1;

        public dynamic Data { get; set; } = new { };
    }
}
