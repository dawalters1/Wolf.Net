using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Subscribers;
using WOLF.Net.Enums.Misc;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Helpers.ProfileBuilders
{
    public class Subscriber
    {
        private WolfBot Bot { get; set; }       
        private int Id { get; set; }

        private string Nickname { get; set; }

        private string Status { get; set; }

        private Language Language { get; set; }

        private Relationship Relationship { get; set; }

        private Gender Gender { get; set; }

        private List<string> Urls { get; set; }

        private LookingFor LookingFor { get; set; }

        private string About { get; set; }

        private string Name { get; set; }

        public Subscriber(WolfBot bot, Entities.Subscribers.Subscriber subscriber)
        {
            Bot = bot;
            Id = subscriber.Id;
            Nickname = subscriber.Nickname;
            Status = subscriber.Status;
            Language = subscriber.Extended.Language;
            Relationship = subscriber.Extended.Relationship;
            Urls = subscriber.Extended.Urls;
            Gender = subscriber.Extended.Gender;
            LookingFor = subscriber.Extended.LookingFor;
            About = subscriber.Extended.About;
            Name = subscriber.Extended.Name;
        }

        public Subscriber SetNickname(string nickname)
        {
            Nickname = nickname;

            return this;
        }

        public Subscriber SetStatus(string status)
        {
            Status = status;

            return this;
        }

        public Subscriber SetLanguage(Language language)
        {
            Language = language;

            return this;
        }

        public Subscriber SetRelationship(Relationship relationship)
        {
            Relationship = relationship;

            return this;
        }

        public Subscriber SetUrls(params string[] urls)
        {
            Urls = urls.ToList();

            return this;
        }

        public Subscriber ClearUrls()
        {
            Urls = new List<string>();

            return this;
        }

        public Subscriber AddUrl(string url)
        {
            Urls.Add(url);

            return this;
        }

        public Subscriber RemoveUrl(string url)
        {
            Urls.Remove(url);

            return this;
        }

        public Subscriber SetGender(Gender gender)
        {
            Gender = gender;

            return this;
        }

        public Subscriber SetLookingFor(params LookingFor[] lookingFor)
        {
            LookingFor = (LookingFor)lookingFor.Sum(r => (int)r);

            return this;
        }

        public Subscriber SetAbout(string about)
        {
            About = about;

            return this;
        }

        public Subscriber SetName(string name)
        {
            Name = name;

            return this;
        }

        public async Task<Response> Save()
        {
            return await Bot.WolfClient.Emit(Request.SUBSCRIBER_PROFILE_UPDATE, new Entities.Subscribers.Subscriber()
            {
                Id = Id,
                Extended = new Extended()
                {
                    About = About,
                    Gender = Gender,
                    Language = Language,
                    LookingFor = LookingFor,
                    Name = Name,
                    Relationship = Relationship,
                    Urls = Urls,
                },
                Nickname = Nickname,
                Status = Status
            });
        }
    }
}
