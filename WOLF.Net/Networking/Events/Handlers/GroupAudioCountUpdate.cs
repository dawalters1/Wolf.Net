using System;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Groups;

namespace WOLF.Net.Networking.Events.Handlers
{
    public class GroupAudioCountUpdate : BaseEvent<AudioCount>
    {
        public override string Command => Event.GROUP_AUDIO_COUNT_UPDATE;

        public override bool ReturnBody => true;

        public override void Handle(AudioCount data)
        {
            try
            {

            }
            catch (Exception d)
            {
                Bot.On.Emit(Internal.ERROR, d.ToString());
            }
        }
    }
}