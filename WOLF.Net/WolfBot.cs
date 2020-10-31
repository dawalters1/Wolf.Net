using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Helpers;

namespace WOLF.Net
{
    public class WolfBot
    {
        public Subscriber Subscriber() => new Subscriber(this);

        public Group Group() => new Group(this);

        public Phrase Phrase() => new Phrase(this);
    }
}
