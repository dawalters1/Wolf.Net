using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Groups.Stages;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        public List<GroupStage> Stages = new List<GroupStage>();

        public async Task<List<GroupStage>> GetStagesAsync(bool requestNew = false)
        {
            if (!requestNew && Stages.Count > 0)
            {
                return Stages;
            }

            var result = await WolfClient.Emit<List<GroupStage>>(Request.STAGE_LIST, new { });

            if (result.Success)
                Stages = result.Body;

            return Stages;
        }
    }
}
