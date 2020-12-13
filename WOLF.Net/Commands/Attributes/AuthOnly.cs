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
    public class AuthOnly : CustomAttribute
    {
        public override Task<bool> Validate(WolfBot bot, CommandData commandData)
        {
            return Task.FromResult(bot.IsAuthorized(commandData.SourceSubscriberId));
        }
    }
}