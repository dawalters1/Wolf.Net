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
    public class RequiredPermissions : BaseAttribute
    {
        private Enums.Groups.Capability _capability = Capability.NotGroupSubscriber;

        private List<Privilege> _privileges { get; set; }

        public RequiredPermissions(Privilege privilege)
        {
            _privileges = new List<Privilege>() { privilege };
        }
        public RequiredPermissions(params Privilege[] privileges)
        {
            if (privileges.Length == 0)
                throw new Exception("Privileges length must be larger than 0");

            _privileges = privileges.ToList();
        }

        public RequiredPermissions(Enums.Groups.Capability capability, [Optional]params Privilege[] privileges)
        {
            if (capability == Enums.Groups.Capability.NotGroupSubscriber || capability == Enums.Groups.Capability.Silenced || capability == Enums.Groups.Capability.Banned)
                throw new Exception($"Capability cannot be set to {capability.ToString().ToUpper()}");

            _capability = capability;
        }


        public override bool Validate(WolfBot bot, CommandData command)
        {
            var groupSubscriber = command.Group.Users.FirstOrDefault(r => r.Id == command.SourceSubscriberId);

            if (_privileges.Count > 0)
            {
                bool hasPrivs = _privileges.Any(r => groupSubscriber.AdditionalInfo.Privileges.HasFlag(r));

                if (hasPrivs)
                    return true;

                if (!hasPrivs && _capability == Capability.NotGroupSubscriber)
                {
                    bot.On.Emit(InternalEvent.PERMISSIONS_FAILED, new FailedPermission()
                    {
                        Language = command.Language,
                        Privileges = _privileges
                    });
                    return false;
                }
            }

            var validateResult = ValidateCapability(groupSubscriber.Capabilities);

            if (!validateResult)
                bot.On.Emit(InternalEvent.PERMISSIONS_FAILED, new FailedPermission()
                {
                    Capabilities = _capability,
                    Language = command.Language,
                    Privileges = _privileges ?? new List<Privilege>()
                });

            return validateResult;
        }


        private bool ValidateCapability(Enums.Groups.Capability sourceSubscriberCapability)
        {
            switch (_capability)
            {
                case  Enums.Groups.Capability.Owner:
                    return sourceSubscriberCapability == Enums.Groups.Capability.Owner;
                case Enums.Groups.Capability.Admin:
                    return sourceSubscriberCapability == Enums.Groups.Capability.Admin || sourceSubscriberCapability == Enums.Groups.Capability.Owner;
                case Enums.Groups.Capability.Mod:
                    return sourceSubscriberCapability == Enums.Groups.Capability.Mod || sourceSubscriberCapability == Enums.Groups.Capability.Admin || sourceSubscriberCapability == Enums.Groups.Capability.Owner;
                default:
                    {
                        if (sourceSubscriberCapability == Enums.Groups.Capability.NotGroupSubscriber || sourceSubscriberCapability == Enums.Groups.Capability.Banned || sourceSubscriberCapability == Enums.Groups.Capability.Silenced)
                            return false;

                        return true;
                    }
            }
        }
    }
}
