using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Groups.Subscribers;

namespace WOLF.Net.Client.Events.Handlers
{
    public class GroupMemberDelete : Event<GroupSubscriberUpdate>
    {
        public override string Command => Event.GROUP_MEMBER_DELETE;

        public override async void HandleAsync(GroupSubscriberUpdate data)
        {
            var group = await Bot.GetGroupAsync(data.GroupId);

            if (group == null || !group.InGroup)
                return;

            if (group.Users.Count > 0)
                group.Users.RemoveAll(r => r.Id == data.SubscriberId);

            if (data.SubscriberId == Bot.CurrentSubscriber.Id)
            {
                group.MyCapabilities = Enums.Groups.Capability.NotGroupSubscriber;
                group.InGroup = false;
                group.Users.Clear();

                await Bot.GroupMessageUnsubscribeAsync(group.Id);

                Bot.On.Emit(InternalEvent.LEFT_GROUP, group);
            }
            else
                Bot.On.Emit(Command, group, await Bot.GetSubscriberAsync(data.SubscriberId));
        }
    }
}
