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
    public class RequiredPermissions : BaseAttribute
    {
        private Enums.Groups.Capability _capability = Capability.NotGroupSubscriber;

        private List<Privilege> _privileges { get; set; } = new List<Privilege>();

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
            _privileges = privileges.ToList();
        }


        public override async Task<bool> Validate(WolfBot bot, CommandData command)
        {
            if (_privileges.Count > 0)
            {
                bool hasPrivs = _privileges.Any(r => command.Subscriber.Privileges.HasFlag(r));

                if (hasPrivs)
                    return true;

                if (!hasPrivs && _capability == Capability.NotGroupSubscriber)
                {
                    bot.On.Emit(InternalEvent.PERMISSIONS_FAILED, new FailedPermission()
                    {
                        SourceTargetId = command.SourceTargetId,
                        SourceSubscriberId = command.SourceSubscriberId,
                        Language = command.Language,
                        Privileges = _privileges ?? new List<Privilege>(),
                        IsGroup = command.IsGroup
                    });
                    return false;
                }
            }

            bool validateResult = true;

            if (bot.IsAuthorized(command.SourceSubscriberId))
                return true;

            else if (_capability == Capability.Regular)
                return true;

            else if (command.Group != null && command.SourceSubscriberId == command.Group.Owner.Id)
                return true;

            else
            {
                var subscribersList = await bot.GetGroupSubscribersListAsync(command.SourceTargetId);

                validateResult = ValidateCapability(subscribersList.FirstOrDefault(r => r.Id == command.SourceSubscriberId));
            }

            if (!validateResult)
                bot.On.Emit(InternalEvent.PERMISSIONS_FAILED, new FailedPermission()
                {
                    SourceTargetId = command.SourceTargetId,
                    SourceSubscriberId = command.SourceSubscriberId,
                    Capabilities = _capability,
                    Language = command.Language,
                    Privileges = _privileges ?? new List<Privilege>(),
                    IsGroup = command.IsGroup
                });

            return validateResult;
        }


        private bool ValidateCapability(GroupSubscriber groupSubscriber)
        {
            if (groupSubscriber == null)
            {
                Console.WriteLine("NULL");
                return false;
            }
            switch (_capability)
            {
                case  Enums.Groups.Capability.Owner:
                    return groupSubscriber.Capabilities == Enums.Groups.Capability.Owner;
                case Enums.Groups.Capability.Admin:
                    return groupSubscriber.Capabilities == Enums.Groups.Capability.Admin || groupSubscriber.Capabilities == Enums.Groups.Capability.Owner;
                case Enums.Groups.Capability.Mod:
                    return groupSubscriber.Capabilities == Enums.Groups.Capability.Mod || groupSubscriber.Capabilities == Enums.Groups.Capability.Admin || groupSubscriber.Capabilities == Enums.Groups.Capability.Owner;
                default:
                    {
                        if (groupSubscriber.Capabilities == Enums.Groups.Capability.NotGroupSubscriber || groupSubscriber.Capabilities == Enums.Groups.Capability.Banned || groupSubscriber.Capabilities == Enums.Groups.Capability.Silenced)
                            return false;

                        return true;
                    }
            }
        }
    }
}
