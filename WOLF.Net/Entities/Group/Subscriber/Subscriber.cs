using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Group;

namespace WOLF.Net.Entities.Group.Subscriber
{
    public class Subscriber
    {
        public int SubscriberId { get; set; }

        public Capability Capability { get; set; }
    }
}
