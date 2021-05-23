using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Enums.Groups;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        public List<Group> Groups { get; internal set; } = new List<Group>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Group profile builder</returns>
        public Builders.Profiles.Group CreateGroupAsync() => new Builders.Profiles.Group(this);

        /// <summary>
        /// Get a group by ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="requestNew"></param>
        /// <returns>Group</returns>
        public async Task<Group> GetGroupAsync(int groupId, bool requestNew = false) => (await GetGroupsAsync(new List<int>() { groupId }, requestNew)).FirstOrDefault();

        /// <summary>
        /// Get a list of groups by ID
        /// </summary>
        /// <param name="groupIds"></param>
        /// <param name="requestNew"></param>
        /// <returns>List of groups</returns>
        public async Task<List<Group>> GetGroupsAsync(List<int> groupIds, bool requestNew = false) => await GetGroupsAsync(groupIds.ToArray(), requestNew);

        /// <summary>
        /// Get a list of groups by ID
        /// </summary>
        /// <param name="groupIds"></param>
        /// <param name="requestNew"></param>
        /// <returns>List of groups</returns>
        public async Task<List<Group>> GetGroupsAsync(int[] groupIds, bool requestNew = false)
        {
            groupIds = groupIds.Distinct().ToArray();

            List<Group> groups = new List<Group>();

            if (!requestNew)
                groups.AddRange(Groups.Where((group) => groupIds.Any((groupId) => groupId == group.Id)).ToList());

            if (groups.Count != groupIds.Length)
            {
                foreach (var batchGroupIdList in groupIds.Where((groupId) => !groups.Any((group) => group.Id == groupId)).ToList().ChunkBy(50))
                {
                    var result = await _webSocket.Emit<Response<Dictionary<int, Response<GroupEntity>>>>(Request.GROUP_PROFILE, new
                    {
                        headers = new
                        {
                            version = 4
                        },
                        body = new
                        {
                            idList = batchGroupIdList,
                            subscribe = true,
                            entities = new List<string>() { "base", "extended", "audioCounts", "audioConfig" }
                        }
                    });

                    if (result.Success)
                        foreach (var group in result.Body)
                            groups.Add(ProcessGroup(group.Value.Success ? group.Value.Body.Compile() : new Group(group.Key)));
                    else
                        groups.AddRange(groupIds.Where((groupId) => !groups.Any((group) => group.Id == groupId)).ToList().Select(r => new Group(r)).ToList());
                }
            }

            return groups;
        }

        /// <summary>
        /// Get a group by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="requestNew"></param>
        /// <returns>Group</returns>
        public async Task<Group> GetGroupAsync(string name, bool requestNew = false)
        {
            if (!requestNew && Groups.Any((group) => group.Name.IsEqual(name)))
                return Groups.FirstOrDefault((group) => group.Name.IsEqual(name));

            var result = await _webSocket.Emit<Response<GroupEntity>>(Request.GROUP_PROFILE, new
            {
                headers = new
                {
                    version = 4
                },
                body = new
                {
                    name,
                    subscribe = true,
                    entities = new List<string>() { "base", "extended", "audioCounts", "audioConfig" }
                }
            });

            return ProcessGroup(result.Success ? result.Body.Compile() : new Group(name));
        }

        /// <summary>
        /// Get a groups subscriber list (member list)
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="requestNew"></param>
        /// <returns>List of subscribers</returns>
        public async Task<List<Subscriber>> GetGroupSubscribersListAsync(int groupId, bool requestNew = false)
        {
            var group = await GetGroupAsync(groupId);

            if (!group.InGroup)
                return new List<Subscriber>();

            if (!requestNew && group.Subscribers.Count == group.Members)
                return group.Subscribers.ToList();

            var result = await _webSocket.Emit<Response<List<Subscriber>>>(Request.GROUP_MEMBER_LIST, new
            {
                headers = new
                {
                    version = 3
                },
                body = new
                {
                    id = groupId,
                    subscribe = true
                }
            });

            if (result.Success)
                group.Subscribers = result.Body.ToList();

            return group.Subscribers;
        }

        /// <summary>
        /// Join a group by ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="password"></param>
        /// <returns>Response</returns>
        public async Task<Response> JoinGroupAsync(int groupId, string password = null) => await _webSocket.Emit<Response>(Request.GROUP_MEMBER_ADD, new { groupId, password });

        /// <summary>
        /// Join a group by name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns>Response</returns>
        public async Task<Response> JoinGroupAsync(string name, string password = null) => await _webSocket.Emit<Response>(Request.GROUP_MEMBER_ADD, new { name = name.ToLowerInvariant(), password });

        /// <summary>
        /// Leave a group by ID
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>Response</returns>
        public async Task<Response> LeaveGroupAsync(int groupId) => await _webSocket.Emit<Response>(Request.GROUP_MEMBER_DELETE, new { groupId });

        /// <summary>
        /// Leave a group by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Response</returns>
        public async Task<Response> LeaveGroupAsync(string name) => await _webSocket.Emit<Response>(Request.GROUP_MEMBER_DELETE, new { groupId = (await GetGroupAsync(name)).Id });

        /// <summary>
        /// Get group activity statistics
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns>Response<Stats></returns>
        public async Task<Response<Stats>> GetGroupStatsAsync(int groupId) => await _webSocket.Emit<Response<Stats>>(Request.GROUP_STATS, new { id = groupId });

        /// <summary>
        /// Update a group subscribers role
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="subscriberId"></param>
        /// <param name="capability"></param>
        /// <returns>Response</returns>
        public async Task<Response> UpdateGroupSubscriberAsync(int groupId, int subscriberId, ActionType capability) => await _webSocket.Emit<Response>(Request.GROUP_MEMBER_UPDATE, new { groupId, id = subscriberId, capabilities = (int)capability });

        /// <summary>
        /// Update a groups profile
        /// </summary>
        /// <param name="group"></param>
        /// <returns>Group profile builder</returns>
        public Builders.Profiles.Group UpdateGroupAsync(Group group) => new Builders.Profiles.Group(this, group);

        /// <summary>
        /// Get all the groups that a bot has requested
        /// </summary>
        /// <param name="joinedOnly"></param>
        /// <param name="requestNew"></param>
        /// <returns>List of groups</returns>
        public async Task<List<Group>> GetJoinedGroupsAsync(bool joinedOnly = false, bool requestNew = false)
        {
            if (Groups.Where(r => r.InGroup).Count() <= 0 || requestNew)
            {
                var joinedGroups = await _webSocket.Emit<Response<List<SubscriberGroup>>>(Request.SUBSCRIBER_GROUP_LIST, new
                {
                    subscribe = true
                });

                if (joinedGroups.Success)
                {
                    var groups = await GetGroupsAsync(joinedGroups.Body.Select(r => r.Id).ToList(), requestNew);

                    foreach (var group in groups)
                    {
                        group.MyCapabilities = joinedGroups.Body.FirstOrDefault(r => r.Id == group.Id).Capabilities;
                        group.InGroup = true;
                        if (requestNew && group.Subscribers.Count > 0)
                            await GetGroupSubscribersListAsync(group.Id);
                    }

                    return groups;
                }
                else
                    return new List<Group>();
            }

            return joinedOnly ? this.Groups.Where(r => r.InGroup).ToList() : this.Groups.ToList();
        }

        internal Group ProcessGroup(Group group)
        {
            group.Bot = this;

            if (group.Exists)
            {
                if (Groups.Any((sub) => sub.Id == group.Id))
                    Groups.FirstOrDefault((sub) => sub.Id == group.Id).Update(group);
                else
                    Groups.Add(group);
            }

            return group;
        }
    }
}