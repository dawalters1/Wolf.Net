using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Enums.Misc;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Entities.Misc;
using System.Drawing;
using System.Linq;

namespace WOLF.Net.Entities.Groups
{
    public class Group
    {
        [JsonIgnore]
        public Action Updated = delegate { };

        [JsonIgnore]
        internal WolfBot Bot;

        internal Group() { }

        internal Group(int groupId)
        {
            this.Id = groupId;
            this.Name = $"<ID:{groupId}>";
            this.Exists = false;
        }

        internal Group(string name)
        {
            this.Id = 0;
            this.Name = name;
            this.Exists = false;
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

        [JsonIgnore]
        internal List<Subscriber> Subscribers { get; set; } = new List<Subscriber>();

        [JsonProperty("official")]
        public bool Official { get; set; }

        [JsonProperty("owner")]
        public IdHash Owner { get; set; }

        [JsonProperty("peekable")]
        public bool Peekable { get; set; }

        [JsonProperty("extended")]
        public Extended Extended { get; set; }

        [JsonIgnore]
        public AudioConfiguration AudioConfiguration { get; set; }

        [JsonIgnore]
        public AudioCount AudioCount { get; set; }

        /// <summary>
        /// The current user is in the group
        /// </summary>
        [JsonIgnore]
        public bool InGroup { get; internal set; }

        /// <summary>
        /// The group exists
        /// </summary>
        [JsonIgnore]
        public bool Exists { get; set; } = true;

        [JsonIgnore]
        public Capability MyCapabilities { get; internal set; } = Capability.NOT_MEMBER;

        internal void Update(Group group)
        {
            this.Id = group.Id;
            this.Hash = group.Hash;
            this.Icon = group.Icon;
            this.Name = group.Name;
            this.Description = group.Description;
            this.Reputation = group.Reputation;
            this.Official = group.Official;
            this.Peekable = group.Peekable;
            this.Members = group.Members;
            this.Extended = group.Extended;
            this.Owner = group.Owner;

            this.Exists = true;

            Updated();
        }

        public string ToDisplayName(bool withId = true) => withId ? $"[{Name}] ({Id})" : $"[{Name}]";

        public Builders.Profiles.Group UpdateProfile() => new Builders.Profiles.Group(Bot, this);

        public async Task<Response> JoinAsync(string password = null) => await Bot.JoinGroupAsync(Id, password);

        public async Task<Response> LeaveAsync() => await Bot.LeaveGroupAsync(Id);

        public async Task<Response<MessageResponse>> SendMessageAsync(object content) => await Bot.SendGroupMessageAsync(Id, content);

        public async Task<Response> ActionAsync(int targetSubscriberId, ActionType actionType) => await Bot.UpdateGroupSubscriberAsync(Id, targetSubscriberId, actionType);

        public async Task<Response<Stats>> GetStats() => await Bot.GetGroupStatsAsync(Id);

        public async Task<List<Subscriber>> GetSubscriberListAsync() => await Bot.GetGroupSubscribersListAsync(Id);

        public async Task<Bitmap> GetAvatar(int size = 640, bool placeholder = false)
        {
            var tsk = new TaskCompletionSource<Bitmap>();
            try
            {
                tsk.SetResult(await (placeholder ? $"https://s3-eu-west-1.amazonaws.com/content-assets.palringo.aws/profiles/avatars/group/placeholder_${Id.ToString().LastOrDefault()}.jpg" : $"{Bot.EndPoints.AvatarEndpoint}/FileServerSpring/group/avatar/{Id}?size={size}").DownloadImageFromUrl());
            }
            catch (Exception error)
            {
                if (placeholder)
                    tsk.SetException(error);
                else
                    return await GetAvatar(size, true);
            }

            return await tsk.Task;
        }
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