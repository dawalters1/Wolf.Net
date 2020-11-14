using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Groups.Subscribers;

namespace WOLF.Net.Client.Events.Handlers
{
    public class GroupMemberAdd : Event<GroupSubscriberUpdate>
    {
        public override string Command => Event.GROUP_MEMBER_ADD;

        public override async void HandleAsync(GroupSubscriberUpdate data)
        {
            var group = await Bot.GetGroupAsync(data.GroupId);

            if (group == null)
                return;

            var subscriber = await Bot.GetSubscriberAsync(data.SubscriberId);

            group.Users.Add(new GroupSubscriber()
            {
                Id = data.SubscriberId,
                GroupId = data.GroupId,
                Capabilities = data.Capabilities,
                AdditionalInfo = new AdditionalInfo()
                {
                    Hash = subscriber.Hash,
                    Nickname = subscriber.Nickname,
                    OnlineState = subscriber.OnlineState,
                    Privileges = subscriber.Privileges
                }
            });

            if (data.SubscriberId == Bot.CurrentSubscriber.Id)
            {
                group.MyCapabilities = data.Capabilities;
                group.InGroup = true;

                Bot.On.Emit(InternalEvent.JOINED_GROUP, group);
            }
            else
                Bot.On.Emit(Command, group, subscriber);
        }
    }
}
