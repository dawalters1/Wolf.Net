using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Entities.Achievements;
using WOLF.Net.Entities.Charms;
using WOLF.Net.Entities.Contacts;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Entities.Phrases;
using WOLF.Net.Entities.Subscribers;

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
 
        private void ProcessContact(Contact contact)
        {
            if (Contacts.Any(r => r.Id == contact.Id))
                Contacts.FirstOrDefault(r=>r.Id==contact.Id).Update(contact);
            else
                Contacts.Add(contact);
        }

        private void ProcessSubscriber(Subscriber subscriber)
        {
            if (Subscribers.Any(r => r.Id == subscriber.Id))
                Subscribers.FirstOrDefault(r => r.Id == subscriber.Id).Update(subscriber);
            else
                Subscribers.Add(subscriber);
        }

        private void ProcessGroup(Group group)
        {
            if (Groups.Any(r => r.Id == group.Id))
                Groups.FirstOrDefault(r => r.Id == group.Id).Update(group);
            else
                Groups.Add(group);
        }
    }
}