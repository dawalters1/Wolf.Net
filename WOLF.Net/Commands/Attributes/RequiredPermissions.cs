using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Commands.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class RequiredPermissions : CustomAttribute
    {
        internal bool AuthOnly = false;

        internal Capability Capability = Capability.NOT_MEMBER;

        internal Privilege[] Privileges = Array.Empty<Privilege>();

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
            if (capability == Capability.NOT_MEMBER || capability == Capability.BANNED || capability == Capability.SILENCED)
                throw new Exception($"Command capability cannot be {capability}");

            this.Capability = capability;
            this.Privileges = privileges ?? this.Privileges;
        }

        public override async Task<bool> Validate(WolfBot bot, CommandData commandData)
        {
            if (bot.Authorization().IsAuthorized(commandData.SourceSubscriberId))
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

                if (Capability == Capability.NOT_MEMBER)
                {
                    EmitFail(bot, commandData);
                    return false;
                }
            }

            if (Capability == Capability.REGULAR || Capability == Capability.NOT_MEMBER || (Capability == Capability.OWNER && commandData.Group != null && commandData.SourceSubscriberId == commandData.Group.Owner.Id))
                return true;
            else
            {
                var groupSubscriber = (await bot.Group().GetSubscribersListAsync(commandData.TargetGroupId)).FirstOrDefault(r => r.Id == commandData.SourceSubscriberId);

                if (groupSubscriber != null && ((Capability) switch
                {
                    Capability.OWNER => groupSubscriber.Capabilities == Capability.OWNER,
                    Capability.ADMIN => groupSubscriber.Capabilities == Capability.ADMIN || groupSubscriber.Capabilities == Capability.OWNER,
                    Capability.MOD => groupSubscriber.Capabilities == Capability.MOD || groupSubscriber.Capabilities == Capability.ADMIN || groupSubscriber.Capabilities == Capability.OWNER,
                    Capability.NOT_MEMBER => false,
                    Capability.REGULAR => true,
                    Capability.BANNED => false,
                    Capability.SILENCED => false,
                    _ => false,
                }))
                    return true;

                EmitFail(bot, commandData);

                return false;
            }
        }

        private void EmitFail(WolfBot bot, CommandData commandData) => bot.On.Emit(Internal.PERMISSIONS_FAILED, new FailedPermission(commandData.SourceSubscriberId, commandData.TargetGroupId, commandData.Language, Capability, Privileges, commandData.IsGroup, AuthOnly, false));
    }
}