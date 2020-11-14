using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Charms;
using WOLF.Net.Enums.Misc;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Entities.Subscribers
{
    public class Subscriber
    {
        [JsonIgnore]
        public Action Updated = delegate { };

        internal Subscriber() { }

        internal Subscriber(int subscriberId)
        {
            Id = subscriberId;
            Nickname = $"<ID{subscriberId}>";
            Exists = false;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("charms")]
        public SubscriberCharm Charms { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("reputation")]
        public double Reputation { get; set; }

        [JsonIgnore]
        public int Level => int.Parse(Reputation.ToString().Split('.')[0]);

        [JsonIgnore]
        public double Percentage => Level == 0 ? 0 : double.Parse(Reputation.ToString("N4").Split('.')[1].Insert(2, "."));

        [JsonProperty("icon")]
        public int Icon { get; set; }

        [JsonProperty("onlineState")]
        public OnlineState OnlineState { get; set; }

        [JsonProperty("deviceType")]
        public DeviceType DeviceType { get; set; }

        [JsonProperty("privileges")]
        public Privilege Privileges { get; set; }

        [JsonProperty("extended")]
        public Extended Extended { get; set; }

    
        [JsonIgnore]
        public bool Exists { get; private set; } = true;

        internal void Update(Subscriber subscriber)
        {
            Id = subscriber.Id;
            Hash = subscriber.Hash;
            Nickname = subscriber.Nickname;
            Icon = subscriber.Icon;
            Charms = subscriber.Charms;
            Status = subscriber.Status;
            OnlineState = subscriber.OnlineState;
            DeviceType = subscriber.DeviceType;
            Reputation = subscriber.Reputation;
            Privileges = subscriber.Privileges;
            Extended = subscriber.Extended;

            Exists = true;

            Updated();
        }

        internal void Update(PresenceUpdate presenceUpdate)
        {
            OnlineState = presenceUpdate.OnlineState;
            DeviceType = presenceUpdate.DeviceType;

            Updated();
        }
    }

    public class Extended
    {
        [JsonProperty("language")]
        public Language Language { get; set; }

        [JsonProperty("relationship")]
        public Relationship Relationship { get; set; }

        [JsonProperty("gender")]
        public Gender Gender { get; set; }

        [JsonProperty("utcOffset")]
        public int UtcOffset { get;set; }

        [JsonProperty("urls")]
        public List<string> Urls { get; set; }

        [JsonProperty("lookingFor")]
        public LookingFor LookingFor { get; set; }

        // [JsonIgnore]
        //  public List<LookingFor> LookingFors => LookingFor.Dating.AllFlags().Where(t => LookingFor.HasFlag(t)).ToList();

        [JsonProperty("about")]
        public string About { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
