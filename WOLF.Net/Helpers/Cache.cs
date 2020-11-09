using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        public Subscriber CurrentSubscriber {get; private set;}

        public List<Phrase> Phrases = new List<Phrase>();

        //Add documentation stating that this includes BLOCKED list
        public List<Contact> Contacts = new List<Contact>();

        public List<Subscriber> Subscribers = new List<Subscriber>();

        public List<Group> Groups = new List<Group>();

        public List<Charm> Charms = new List<Charm>();

        public List<Achievement> Achievements = new List<Achievement>();
 
    }
}