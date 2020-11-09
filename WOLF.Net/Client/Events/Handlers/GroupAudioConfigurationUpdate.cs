using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Groups.Stages;

namespace WOLF.Net.Client.Events.Handlers
{
    public class GroupAudioConfigurationUpdate : Event<GroupAudioConfiguration>
    {
        public override string Command => Event.GROUP_AUDIO_CONFIGURATION_UPDATE;

        public override void HandleAsync(GroupAudioConfiguration data)
        {
            throw new NotImplementedException();
        }
    }
}
