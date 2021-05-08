using System;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Groups;

namespace WOLF.Net.Networking.Events.Handlers
{
    public class GroupAudioConfigurationUpdate : BaseEvent<AudioConfiguration>
    {
        public override string Command => Event.GROUP_AUDIO_CONFIGURATION_UPDATE;

        public override bool ReturnBody => true;

        public override void Handle(AudioConfiguration data)
        {
            try
            {

            }
            catch(Exception d)
            {
                Bot.On.Emit(Internal.ERROR, d.ToString());
            }
        }

  
    }
}
