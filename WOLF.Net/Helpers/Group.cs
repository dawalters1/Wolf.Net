using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Entities.Groups.Stats;
using WOLF.Net.Entities.Groups.Subscribers;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Utilities;

namespace WOLF.Net
{
    //TO-DO:
    /*
     * Update profile
     */
    public partial class WolfBot
    {
        public List<Group> Groups = new List<Group>();

        /// <summary>
        /// Create a group with name and tagline
        /// </summary>
        /// <param name="name">What you wish to name the group</param>
        /// <param name="tagLine">A short description about the group</param>
        /// <returns></returns>
        public async Task<Response> CreateGroupAsync(string name, string tagLine)
        {
            return await WolfClient.Emit(Request.GROUP_CREATE, new
            {
                name,
                description = tagLine
            });
        }

        /// <summary>
        /// Get the stats for a group (Top users, Actions, Etc)
        /// </summary>
        /// <param name="groupId">The id of the group</param>
        /// <returns></returns>
        public async Task<Response<GroupStats>> GroupStatsAsync(int groupId)
        {
            return await WolfClient.Emit<GroupStats>(Request.GROUP_STATS, new
            {
                id = groupId
            });
        }

        /// <summary>
        /// Gets the group members list
        /// </summary>
        /// <param name="groupId">The id of the group</param>
        /// <returns></returns>
        public async Task<List<GroupSubscriber>> GetGroupSubscribersListAsync(int groupId)
        {
            var group = await GetGroupAsync(groupId);

            if (group.Users.Count == group.Members)
                return group.Users;

            var result = await WolfClient.Emit<List<GroupSubscriber>>(Request.GROUP_MEMBER_LIST, new
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
            {
                var groupSubscribers = result.Body.ToList();

                group.Users.RemoveAll(r => !groupSubscribers.Any(s => s.Id == r.Id));

                foreach (var groupSubscriber in groupSubscribers)
                {
                    if (group.Users.Any(r => r.Id == groupSubscriber.Id))
                        group.Users.FirstOrDefault(r => r.Id == groupSubscriber.Id).Update(groupSubscriber);
                    else
                        group.Users.Add(groupSubscriber);
                }

                return group.Users;
            }

            return new List<GroupSubscriber>();
        }

        /// <summary>
        /// Perform an admin action
        /// </summary>
        /// <param name="groupId">The id of the group you wish to perform the action in</param>
        /// <param name="targetSubscriberId">The id of the subscriber you wish to perform the action on</param>
        /// <param name="action">The action you want to perform on the subscriber</param>
        /// <returns></returns>
        public async Task<Response> GroupActionAsync(int groupId, int targetSubscriberId, GroupActionType action)
        {
            return await WolfClient.Emit(Request.GROUP_MEMBER_UPDATE, new
            {
                groupId,
                id = targetSubscriberId,
                capabilities = (int)action
            });
        }

        /// <summary>
        /// Join a group by Name
        /// </summary>
        /// <param name="name">The name of the group</param>
        /// <param name="password">The password for the group (Optional)</param>
        /// <returns></returns>
        public async Task<Response> JoinGroupAsync(string name, string password=null)
        {
            return await WolfClient.Emit(Request.GROUP_MEMBER_ADD, new
            {
                name = name.ToLower(),
                password
            });
        }

        /// <summary>
        /// Join a group by Id
        /// </summary>
        /// <param name="groupId">The id of the group</param>
        /// <param name="password">The password for the group (Optional)</param>
        /// <returns></returns>
        public async Task<Response> JoinGroupAsync(int groupId, string password = null)
        {
            return await WolfClient.Emit(Request.GROUP_MEMBER_ADD, new
            {
                groupId,
                password
            });
        }

        /// <summary>
        /// Leave a group by Id
        /// </summary>
        /// <param name="groupId">The id of the group</param>
        /// <returns></returns>
        public async Task<Response> LeaveGroupAsync(int groupId)
        {
            return await WolfClient.Emit(Request.GROUP_MEMBER_DELETE, new
            {
                groupId
            });
        }

        /// <summary>
        /// Leave a group by name
        /// </summary>
        /// <param name="name">The name of the group</param>
        /// <returns></returns>
        public async Task<Response> LeaveGroupAsync(string name)
        {
            if (!Groups.Any(r => r.Name.IsEqual(name)))
                return new Response() { Code = 404, Headers = new Dictionary<string, string>() };

            return await LeaveGroupAsync(Groups.FirstOrDefault(r => r.Name.IsEqual(name)).Id);
        }

        /// <summary>
        /// Get a group by name
        /// </summary>
        /// <param name="name">The id of the group</param>
        /// <param name="requestNew">Used by the API when a group update event happens and hashes do not match</param>
        /// <returns></returns>
        public async Task<Group> GetGroupAsync(string name, bool requestNew = false)
        {
            if (!requestNew && Groups.Any(r => r.Name.IsEqual(name)))
            {
                return Groups.FirstOrDefault(r => r.Name.IsEqual(name));
            }

            var result = await WolfClient.Emit<GroupEntityResponse>(Request.GROUP_PROFILE, new
            {
                headers = new
                {
                    version = 4
                },
                body = new
                {
                    name,
                    subscribe = true,
                    entities = new List<string>() { "base", "extended", "audioCounts", "audioConfig" },
                }
            });

            if (result.Success)
            {
                var compiledGroup = result.Body.Compile();

                ProcessGroup(compiledGroup);

                return compiledGroup;
            }

            return new Group(name);
        }

        /// <summary>
        /// Get a group by Id
        /// </summary>
        /// <param name="groupId">The id of the group</param>
        /// <param name="requestNew">Used by the API when a group update event happens and hashes do not match</param>
        /// <returns></returns>
        public async Task<Group> GetGroupAsync(int groupId, bool requestNew = false)
        {
            if (!requestNew && Groups.Any(r => r.Id == groupId))
            {
                return Groups.FirstOrDefault(r => r.Id == groupId);
            }

            var result = await WolfClient.Emit<Dictionary<int, Response<GroupEntityResponse>>>(Request.GROUP_PROFILE, new
            {
                headers = new
                {
                    version = 4
                },
                body = new
                {
                    idList = new List<int>() { groupId },
                    subscribe = true,
                    entities = new List<string>() { "base", "extended", "audioCounts", "audioConfig" },
                }
            });

            var group = result.Body[groupId];

            if (group.Success)
            {
                var compiledGroup = group.Body.Compile();

                ProcessGroup(compiledGroup);

                return compiledGroup;
            }

            return new Group(groupId);
        }

        /// <summary>
        /// Bulk get group
        /// </summary>
        /// <param name="groupIds">A list of group ids</param>
        /// <param name="requestNew">Used by the API when a group update event happens and hashes do not match</param>
        /// <returns></returns>
        public async Task<List<Group>> GetGroupsAsync(List<int> groupIds, bool requestNew = false)
        {
            List<Group> groups = new List<Group>();

            if (!requestNew)
            {
                foreach (var groupId in groupIds)
                    if (Groups.Any(r => r.Id == groupId))
                    {
                        groups.Add(Groups.FirstOrDefault(r => r.Id == groupId));
                        groupIds.RemoveAll(r => r == groupId);
                    }
            }

            if (groupIds.Count == 0)
                return groups;

            var results = await WolfClient.Emit<Dictionary<int, Response<GroupEntityResponse>>>(Request.GROUP_PROFILE, new
            {
                headers = new
                {
                    version = 4
                },
                body = new
                {
                    idList = groupIds,
                    subscribe = true,
                    entities = new List<string>() { "base", "extended", "audioCounts", "audioConfig" },
                }
            });

            foreach (var group in results.Body)
            {
                if (group.Value.Success)
                {
                    var compiledGroup = group.Value.Body.Compile();

                    ProcessGroup(compiledGroup);

                    groups.Add(compiledGroup);
                }
                else
                    groups.Add(new Group(group.Key));
            }

            return groups;
        }



        internal void ProcessGroup(Group group)
        {
            if (Groups.Any(r => r.Id == group.Id))
                Groups.FirstOrDefault(r => r.Id == group.Id).Update(group);
            else
                Groups.Add(group);
        }
    }
}