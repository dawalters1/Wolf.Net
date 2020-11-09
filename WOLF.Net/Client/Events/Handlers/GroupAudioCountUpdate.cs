using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Groups.Stages;

namespace WOLF.Net.Client.Events.Handlers
{
    public class GroupAudioCountUpdate : Event<GroupAudioCount>
    {
        public override string Command => Event.GROUP_AUDIO_COUNT_UPDATE;

        public override void HandleAsync(GroupAudioCount data)
        {
            throw new NotImplementedException();
        }
    }
}
