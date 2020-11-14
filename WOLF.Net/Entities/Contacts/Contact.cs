using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Subscribers;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Entities.Contacts
{
    public class Contact
    {
        internal Contact() { }

        internal Contact(Subscriber subscriber, bool isBlocked = false)
        {
            Id = subscriber.Id;
            AdditionalInfo = new AdditionalInfo()
            {
                Hash = subscriber.Hash,
                Nickname = subscriber.Nickname,
                OnlineState = subscriber.OnlineState,
                Privileges = subscriber.Privileges
            };
            IsBlocked = isBlocked;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("additionalInfo")]
        public AdditionalInfo AdditionalInfo { get; set; }

        public bool IsBlocked { get; internal set; } = false;

        internal void Update(Contact contact)
        {
            Id = contact.Id;
            AdditionalInfo = contact.AdditionalInfo;
        }
        internal void Update(Entities.Subscribers.Subscriber subscriber)
        {
            Id = subscriber.Id;
            AdditionalInfo.Update(subscriber);
        }
    }

    public class AdditionalInfo
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("nicknameShort")]
        public string Nickname { get; set; }

        [JsonProperty("onlineState")]
        public OnlineState OnlineState { get; set; }

        [JsonProperty("privileges")]
        public long Privileges { get; set; }

        internal void Update(OnlineState onlineState)
        {
            OnlineState = onlineState;
        }
        internal void Update(Entities.Subscribers.Subscriber subscriber)
        {
            Hash = subscriber.Hash;
            Nickname = subscriber.Nickname;
            Privileges = subscriber.Privileges;
            OnlineState = subscriber.OnlineState;
        }
    }
}
