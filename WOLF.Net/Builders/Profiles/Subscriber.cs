using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Subscribers;
using WOLF.Net.Enums.Misc;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Builders.Profiles
{
    public class Subscriber
    {
        private WolfBot _bot { get; set; }

        private int id { get; set; }

        private string nickname { get; set; }

        private string status { get; set; }

        private Language language { get; set; }

        private Relationship relationship { get; set; }

        private Gender gender { get; set; }

        private List<string> urls { get; set; }

        private LookingFor lookingFor { get; set; }

        private string about { get; set; }

        private string name { get; set; }

        public Subscriber(WolfBot bot, Entities.Subscribers.Subscriber subscriber)
        {
            if (subscriber == null)
                throw new Exception("You must be logged in to update a profile");

            this._bot = bot;
            this.id = subscriber.Id;
            this.nickname = subscriber.Nickname;
            this.status = subscriber.Status;
            this.language = subscriber.Extended.Language;
            this.relationship = subscriber.Extended.Relationship;
            this.urls = subscriber.Extended.Urls;
            this.gender = subscriber.Extended.Gender;
            this.lookingFor = subscriber.Extended.LookingFor;
            this.about = subscriber.Extended.About;
            this.name = subscriber.Extended.Name;
        }

        public Subscriber SetNickname(string nickname)
        {
            this.nickname = nickname;

            return this;
        }

        public Subscriber SetStatus(string status)
        {
            this.status = status;

            return this;
        }

        public Subscriber SetLanguage(Language language)
        {
            this.language = language;

            return this;
        }

        public Subscriber SetRelationship(Relationship relationship)
        {
            this.relationship = relationship;

            return this;
        }

        public Subscriber SetUrls(params string[] urls)
        {
            this.urls = urls.ToList();

            return this;
        }

        public Subscriber ClearUrls()
        {
            urls = new List<string>();

            return this;
        }

        public Subscriber AddUrl(string url)
        {
            urls.Add(url);

            return this;
        }

        public Subscriber RemoveUrl(string url)
        {
            urls.Remove(url);

            return this;
        }

        public Subscriber SetGender(Gender gender)
        {
            this.gender = gender;

            return this;
        }

        public Subscriber SetLookingFor(params LookingFor[] lookingFor)
        {
            this.lookingFor = (LookingFor)lookingFor.Sum(r => (int)r);

            return this;
        }

        public Subscriber SetAbout(string about)
        {
            this.about = about;

            return this;
        }

        public Subscriber SetName(string name)
        {
            this.name = name;

            return this;
        }

        public async Task<Response> Save() => await _bot._webSocket.Emit<Response>(Request.SUBSCRIBER_PROFILE_UPDATE, new Entities.Subscribers.Subscriber()
        {
            Id = id,
            Extended = new Extended()
            {
                About = about,
                Gender = gender,
                Language = language,
                LookingFor = lookingFor,
                Name = name,
                Relationship = relationship,
                Urls = urls,
            },
            Nickname = nickname,
            Status = status
        });
    }
}