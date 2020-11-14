using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Groups.Subscribers;

namespace WOLF.Net.Client.Events.Handlers
{
    public class GroupMemberUpdate : Event<GroupSubscriberUpdate>
    {
        public override string Command => Event.GROUP_MEMBER_UPDATE;

        public override async void HandleAsync(GroupSubscriberUpdate data)
        {
            var group = await Bot.GetGroupAsync(data.GroupId);

            if (group == null || !group.InGroup)
                return;

            if (group.Users.Count > 0)
                group.Users.FirstOrDefault(r => r.Id == data.SubscriberId).Capabilities = data.Capabilities;

            if (data.SubscriberId == Bot.CurrentSubscriber.Id)
                group.MyCapabilities = data.Capabilities;
        }
    }
}
