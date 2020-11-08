using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Groups;

namespace WOLF.Net.Entities.Groups.Subscribers
{
    public class Subscriber
    {
        public int SubscriberId { get; set; }

        public Capability Capability { get; set; }
    }
}
