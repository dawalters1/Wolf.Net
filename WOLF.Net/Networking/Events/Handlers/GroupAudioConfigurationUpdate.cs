using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
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
                var group = Bot.Group().cache.FirstOrDefault(r => r.Id == data.Id);

                if (group == null)
                    return;

                group.AudioConfiguration = data;

                Bot.On.Emit(Command, group, data);
            }
            catch (Exception d)
            {
                Bot.On.Emit(Internal.ERROR, d.ToString());
            }
        }

  
    }
}
