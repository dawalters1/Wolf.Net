using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Entities.API
{
    public class FailedPermission
    {
        [Obsolete("This property will be deprecated soon, please use SourceSubscriberId", true)]
        public int UserId => SourceSubscriberId;

        public int SourceSubscriberId { get; set; }

        [Obsolete("This property will be deprecated soon, please use SourceTargetId", true)]
        public int ReturnAddress => SourceTargetId;

        public int SourceTargetId { get; set; }

        public bool AuthOnly { get; set; }

        public Capability Capabilities { get; set; }

        public Privilege[] Privileges = { };

        public string Language { get; set; }

        public bool IsGroup { get; set; }
    }
}
