using System;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Entities.API
{
    public class FailedPermission
    {
        public int SourceSubscriberId { get; set; }

        public int TargetGroupId { get; set; }

        public bool IsAuthOnly { get; set; }

        public Capability Capabilities { get; set; }

        public Privilege[] Privileges = Array.Empty<Privilege>();

        public string Language { get; set; }

        public bool IsGroup { get; set; }

        internal FailedPermission(int sourceSubscriberId, int sourceTargetGroupId, string language, Capability capabilities, Privilege[] privileges, bool isGroup, bool isAuthOnly)
        {
            this.SourceSubscriberId = sourceSubscriberId;
            this.TargetGroupId = sourceTargetGroupId;
            this.Language = language;
            this.Capabilities = capabilities;
            this.Privileges = privileges;
            this.IsGroup = isGroup;
            this.IsAuthOnly = isAuthOnly;
        }
    }
}