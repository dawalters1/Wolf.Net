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
        internal Enums.Groups.Capability Capability = Capability.None;

        internal List<Privilege> Privileges { get; set; } = new List<Privilege>();

        public RequiredPermissions(Privilege privilege)
        {
            Privileges = new List<Privilege>() { privilege };
        }

        public RequiredPermissions(params Privilege[] privileges)
        {
            if (privileges.Length == 0)
                throw new Exception("Privileges length must be larger than 0");

            this.Privileges = privileges.ToList();
        }

        public RequiredPermissions(Enums.Groups.Capability capability, [Optional] params Privilege[] privileges)
        {
            if (capability == Enums.Groups.Capability.None || capability == Enums.Groups.Capability.Silenced || capability == Enums.Groups.Capability.Banned)
                throw new Exception($"Capability cannot be set to {capability.ToString().ToUpper()}");

            this.Capability = capability;
            this.Privileges = privileges.ToList();
        }

        private void EmitFail(WolfBot bot, CommandData commandData)
        {
            bot.On.Emit(InternalEvent.PERMISSIONS_FAILED, new FailedPermission()
            {
                SourceTargetId = commandData.SourceTargetId,
                SourceSubscriberId = commandData.SourceSubscriberId,
                Capabilities = Capability,
                Language = commandData.Language,
                Privileges = Privileges,
                IsGroup = commandData.IsGroup
            });
        }

        public override async Task<bool> Validate(WolfBot bot, CommandData commandData)
        {
            if (Privileges.Count > 0)
            {
                bool hasPrivs = Privileges.Any(r => commandData.Subscriber.Privileges.HasFlag(r));

                if (hasPrivs)
                    return true;

                if (!hasPrivs && Capability == Capability.None)
                {
                    EmitFail(bot, commandData);
                    return false;
                }
            }

            bool validateResult = true;

            if (bot.IsAuthorized(commandData.SourceSubscriberId))
                return true;

            else if (Capability == Capability.Regular || Capability == Capability.None)
                return true;

            else if (Capability == Capability.Owner&& commandData.Group != null && commandData.SourceSubscriberId == commandData.Group.Owner.Id)
                return true;

            else
            {
                var subscribersList = await bot.GetGroupSubscribersListAsync(commandData.SourceTargetId);
                var groupSubscriber = subscribersList.FirstOrDefault(r => r.Id == commandData.SourceSubscriberId);

                validateResult = groupSubscriber == null ? false : (Capability) switch
                {
                    Capability.Owner => groupSubscriber.Capabilities == Capability.Owner,
                    Capability.Admin => groupSubscriber.Capabilities == Capability.Admin || groupSubscriber.Capabilities == Capability.Owner,
                    Capability.Mod => groupSubscriber.Capabilities == Capability.Mod || groupSubscriber.Capabilities == Capability.Admin || groupSubscriber.Capabilities == Capability.Owner,
                    Capability.None => false,
                    Capability.Regular => true,
                    Capability.Banned => false,
                    Capability.Silenced => false,
                    _ => false,
                };
            }

            if (!validateResult)
                EmitFail(bot, commandData);

            return validateResult;
        }
    }
}