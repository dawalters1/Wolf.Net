using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Entities.Groups.Subscribers
{
    public class GroupSubscriber
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonIgnore]
        public int GroupId { get; set; }

        [JsonProperty("additionalInfo")]
        public AdditionalInfo AdditionalInfo { get; set; }

        [JsonProperty("capabilities")]
        public Capability Capabilities { get; set; }

        internal void Update(Entities.Subscribers.Subscriber subscriber)
        {
            Id = subscriber.Id;
            AdditionalInfo.Update(subscriber);
        }

        internal void Update(GroupSubscriber groupSubscriber)
        {
            Id = groupSubscriber.Id;
            AdditionalInfo = groupSubscriber.AdditionalInfo;
        }
    }

    public class AdditionalInfo
    {
        public AdditionalInfo() { }

        public AdditionalInfo(Entities.Subscribers.Subscriber subscriber)
        {
            Hash = subscriber.Hash;
            Nickname = subscriber.Nickname;
            Privileges = subscriber.Privileges;
            OnlineState = subscriber.OnlineState;
        }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("nicknameShort")]
        public string Nickname { get; set; }

        [JsonProperty("privileges")]
        public Privilege Privileges { get; set; }

        [JsonProperty("onlineState")]
        public OnlineState OnlineState { get; set; }

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
