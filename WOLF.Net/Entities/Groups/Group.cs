using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Groups.Stages;
using WOLF.Net.Entities.Groups.Subscribers;
using WOLF.Net.Entities.Misc;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Misc;

namespace WOLF.Net.Entities.Groups
{
    public class Group
    {
        [JsonIgnore]
        public Action Updated = delegate { };

        internal Group() { }

        internal Group(int groupId)
        {
            Id = groupId;
            Name = $"<ID{groupId}>";
            Exists = false;
        }

        internal Group(string name)
        {
            Id = 0;
            Name = name;
            Exists = false;
        }

        internal Group(Group group, GroupAudioConfiguration groupAudioConfiguration, GroupAudioCount groupAudioCount, Extended extended)
        {
            Update(group);
            AudioConfiguration = groupAudioConfiguration;
            AudioCount = groupAudioCount;
            Extended = extended;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("icon")]
        public int Icon { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("reputation")]
        public float Reputation { get; set; }

        [JsonProperty("members")]
        public int Members { get; internal set; }

        /// <summary>
        /// A list of the members in the group
        /// <remarks>This is only requested when a user uses a valid command, to load the group members list any other time  <see cref="GetMembersListAsync"/> </remarks>
        /// </summary>
        [JsonIgnore]
        public List<GroupSubscriber> Users { get; internal set; } = new List<GroupSubscriber>();

        [JsonProperty("official")]
        public bool Official { get; set; }

        [JsonProperty("owner")]
        public IdHash Owner { get; set; }

        [JsonProperty("peekable")]
        public bool Peekable { get; set; }

        [JsonProperty("extended")]
        public Extended Extended { get; set; }

        [JsonIgnore]
        public GroupAudioConfiguration AudioConfiguration { get; set; }

        [JsonIgnore]
        public GroupAudioCount AudioCount { get; set; }

        /// <summary>
        /// The current user is in the group
        /// </summary>
        [JsonIgnore]
        public bool InGroup { get; internal set; }

        /// <summary>
        /// The group exists
        /// </summary>
        [JsonIgnore]
        public bool Exists { get; set; }

        [JsonIgnore]
        public Capability MyCapabilities { get; internal set; } = Capability.None;

        internal void Update(Group group)
        {
            Id = group.Id;
            Hash = group.Hash;
            Icon = group.Icon;
            Name = group.Name;
            Description = group.Description;
            Reputation = group.Reputation;
            Official = group.Official;
            Peekable = group.Peekable;
            Members = group.Members;
            Extended = group.Extended;
            Owner = group.Owner;

            Exists = true;

            Updated();
        }

        public string ToDisplayName(bool withId = true) => withId ? $"[{Name}] ({Id})" : $"[{Name}]";

        public Helpers.ProfileBuilders.GroupUpdateBuilder UpdateProfile(WolfBot bot) => new Helpers.ProfileBuilders.GroupUpdateBuilder(bot, this);

        public Helpers.ProfileBuilders.StageUpdateBuilder UpdateStage(WolfBot bot) =>
            new Helpers.ProfileBuilders.StageUpdateBuilder(bot, this.AudioConfiguration);
    }
    

    public class Extended
    {
        /// <summary>
        /// The group can be seen in the discovery list
        /// </summary>
        [JsonProperty("discoverable")]
        public bool Discoverable { get; set; }

        /// <summary>
        /// Admins have full powers
        /// </summary>
        [JsonProperty("advancedAdmin")]
        public bool AdvancedAdmin { get; set; }

        /// <summary>
        /// The group has been locked
        /// </summary>
        [JsonProperty("locked")]
        public bool Locked { get; set; }

        /// <summary>
        /// The group has been flagged as questionable
        /// </summary>
        [JsonProperty("questionable")]
        public bool Questionable { get; set; }

        /// <summary>
        /// The level required to join the group
        /// </summary>
        [JsonProperty("entryLevel")]
        public int EntryLevel { get; set; }

        /// <summary>
        /// The group has a password
        /// </summary>
        [JsonProperty("passworded")]
        public bool Passworded { get; set; }

        /// <summary>
        /// The language of the group
        /// </summary>
        [JsonProperty("language")]
        public Language Language { get; set; }


        [JsonProperty("category")]
        public Category Category { get; set; }


        [JsonProperty("longDescription")]
        public string LongDescription { get; set; }

        /// <summary>
        /// The ID of the group
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
