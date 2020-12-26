using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Groups.Subscribers;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class RequiredPermissions : CustomAttribute
    {
        internal bool AuthOnly = false;

        internal Capability Capability = Capability.None;

        internal Privilege[] Privileges = { };

        public RequiredPermissions(bool authOnly)
        {
            AuthOnly = authOnly;
        }

        public RequiredPermissions(params Privilege[] privileges)
        {
            Privileges = privileges;
        }

        public RequiredPermissions(Capability capability, [Optional] params Privilege[] privileges)
        {
            if (capability == Capability.None || capability == Capability.Banned || capability == Capability.Silenced)
                throw new Exception($"Command capability cannot be {capability}");

            Capability = capability;
            Privileges = privileges ?? (new Privilege[] { });
        }

        public override async Task<bool> Validate(WolfBot bot, CommandData commandData)
        {
            if (bot.IsAuthorized(commandData.SourceSubscriberId))
                return true;

            if (AuthOnly)
            {
                EmitFail(bot, commandData);
                return false;
            }
            if (Privileges.Length > 0)
            {
                if (Privileges.Any(r => commandData.Subscriber.Privileges.HasFlag(r)))
                    return true;

                if (Capability == Capability.None)
                {
                    EmitFail(bot, commandData);
                    return false;
                }
            }

            if (Capability == Capability.Regular || Capability == Capability.None || (Capability == Capability.Owner && commandData.Group != null && commandData.SourceSubscriberId == commandData.Group.Owner.Id))
                return true;
            else
            {
                var groupSubscriber = (await bot.GetGroupSubscribersListAsync(commandData.SourceTargetId)).FirstOrDefault(r => r.Id == commandData.SourceSubscriberId);

                if (groupSubscriber != null && ((Capability) switch
                {
                    Capability.Owner => groupSubscriber.Capabilities == Capability.Owner,
                    Capability.Admin => groupSubscriber.Capabilities == Capability.Admin || groupSubscriber.Capabilities == Capability.Owner,
                    Capability.Mod => groupSubscriber.Capabilities == Capability.Mod || groupSubscriber.Capabilities == Capability.Admin || groupSubscriber.Capabilities == Capability.Owner,
                    Capability.None => false,
                    Capability.Regular => true,
                    Capability.Banned => false,
                    Capability.Silenced => false,
                    _ => false,
                }))
                    return true;

                EmitFail(bot, commandData);

                return false;
            }
        }

        private void EmitFail(WolfBot bot, CommandData commandData)
        {
            bot.On.Emit(InternalEvent.PERMISSIONS_FAILED, new FailedPermission()
            {
                AuthOnly = AuthOnly,
                SourceTargetId = commandData.SourceTargetId,
                SourceSubscriberId = commandData.SourceSubscriberId,
                Capabilities = Capability,
                Language = commandData.Language,
                Privileges = Privileges,
                IsGroup = commandData.IsGroup
            });
        }

    }
}