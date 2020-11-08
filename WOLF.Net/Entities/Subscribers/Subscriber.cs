using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Entities.Subscribers
{
    public class Subscriber
    {
        public int Id { get; set; }
        public Privilege Privileges { get; set; }
    }
}
