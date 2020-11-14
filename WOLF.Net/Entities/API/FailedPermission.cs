using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Entities.API
{
    public class FailedPermission
    {
        public Capability Capabilities { get; set; }

        public List<Privilege> Privileges { get; set; }

        public string Language { get; set; }
    }
}
