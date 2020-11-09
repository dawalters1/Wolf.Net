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
        public long Privileges
        {
            get { return (int)PrivilegeTags; }
            set { PrivilegeTags = (Privilege)value; }
        }

        [JsonProperty("extended")]
        public Extended Extended { get; set; }

        [JsonIgnore]
        public Privilege PrivilegeTags { get; set; }

        [JsonIgnore]
        public bool Exists { get; set; }

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
        public int UtcOffset { get; private set; }

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
