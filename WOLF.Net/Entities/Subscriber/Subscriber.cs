using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Subscriber;

namespace WOLF.Net.Entities.Subscriber
{
    public class Subscriber
    {
        public int Id { get; set; }
        public Privilege Privileges { get; set; }
    }
}
