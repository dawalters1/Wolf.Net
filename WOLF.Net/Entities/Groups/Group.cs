using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Groups.Subscribers;

namespace WOLF.Net.Entities.Groups
{
    public class Group
    {
        public int Id { get; set; }

        public List<Subscribers.Subscriber> Users = new List<Subscribers.Subscriber>();
    }
}
