using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;

namespace WOLF.Net.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class BotOwnerOnly : CustomAttribute
    {

        public override Task<bool> Validate(WolfBot bot, CommandData commandData)
        {
            if ( bot.Configuration.BotOwnerId == commandData.SourceSubscriberId)
                return Task.FromResult(true);

            EmitFail(bot, commandData);

            return Task.FromResult(false);
        }

        private void EmitFail(WolfBot bot, CommandData commandData) => bot.On.Emit(Internal.PERMISSIONS_FAILED, new FailedPermission(commandData.SourceSubscriberId, commandData.TargetGroupId, commandData.Language, Enums.Groups.Capability.NOT_MEMBER, null, commandData.IsGroup, false, true));

    }
}
