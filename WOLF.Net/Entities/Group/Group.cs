using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Group.Subscriber;

namespace WOLF.Net.Entities.Group
{
    public class Group
    {
        public int Id { get; set; }

        public List<Subscriber.Subscriber> Users = new List<Subscriber.Subscriber>();
    }
}
