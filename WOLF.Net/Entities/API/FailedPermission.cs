using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Entities.API
{
    public class FailedPermission
    {
        public int SourceSubscriberId { get; set; }

        public int SourceTargetId { get; set; }

        public bool AuthOnly { get; set; }

        public Capability Capabilities { get; set; }

        public List<Privilege> Privileges { get; set; } = new List<Privilege>();

        public string Language { get; set; }
    }
}
