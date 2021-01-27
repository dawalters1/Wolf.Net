using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Groups.Stages;

namespace WOLF.Net.Client.Events.Handlers
{
    public class GroupAudioCountUpdate : Event<GroupAudioCount>
    {
        public override string Command => Event.GROUP_AUDIO_COUNT_UPDATE;

        public override async void HandleAsync(GroupAudioCount data)
        {
            var group = await Bot.GetGroupAsync(data.Id);

            if (group == null)
                return;

            group.AudioCount = data;

            Bot.On.Emit(Command, group, data);
        }
        public override void Register()
        {
            Client.On<Response<GroupAudioCount>>(Command, resp => HandleAsync(resp.Body));
        }
    }
}
