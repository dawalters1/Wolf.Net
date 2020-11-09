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

        public override void HandleAsync(GroupSubscriberUpdate data)
        {
            throw new NotImplementedException();
        }
    }
}
