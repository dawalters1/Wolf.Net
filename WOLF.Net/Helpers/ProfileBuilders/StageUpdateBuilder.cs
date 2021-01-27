using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Groups.Stages;

namespace WOLF.Net.Helpers.ProfileBuilders
{
    public class StageUpdateBuilder
    {
       private WolfBot Bot { get; set; }

       private int Id { get; set; }

        private int MinRepLevel { get; set; }

        private int StageId { get; set; }

        private bool Enabled { get; set; }

        public StageUpdateBuilder(WolfBot bot, GroupAudioConfiguration groupAudioConfiguration)
        {
            Bot = bot;
            Id = groupAudioConfiguration.Id;
            MinRepLevel = groupAudioConfiguration.MinRepLevel;
            StageId = groupAudioConfiguration.StageId;
            Enabled = groupAudioConfiguration.Enabled;
        }

        public StageUpdateBuilder SetMinRepLevel(int minRepLevel)
        {
            MinRepLevel = minRepLevel;

            return this;
        }

        public StageUpdateBuilder SetStageThemeId(int stageThemeId)
        {
            StageId = stageThemeId;

            return this;
        }

        public StageUpdateBuilder SetStageEnabled(bool isStageEnabled)
        {
            Enabled = isStageEnabled;

            return this;
        }

        public async Task<Response> Save()
        {
            return await Bot.WolfClient.Emit(Request.GROUP_AUDIO_UPDATE, new
            {
                stageId = StageId,
                minRepLevel =MinRepLevel,
                id = Id,
                enabled = Enabled
            });
        }
    }
}
