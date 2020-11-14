using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;

namespace WOLF.Net.Commands.Attributes
{
    public class AuthOnly : BaseAttribute
    {
        public override async Task<bool> Validate(WolfBot bot, CommandData command)
        {
            var result = bot.IsAuthorized(command.SourceSubscriberId);

            if (!result)
                bot.On.Emit(InternalEvent.PERMISSIONS_FAILED, new FailedPermission()
                {
                    SourceTargetId = command.SourceTargetId,
                    SourceSubscriberId = command.SourceSubscriberId,
                    Language = command.Language,
                    AuthOnly = true
                });

            return result;
        }
    }
}