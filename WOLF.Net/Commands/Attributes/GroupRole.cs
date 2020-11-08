using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Enums.Groups;

namespace WOLF.Net.Commands.Attributes
{
    public class GroupRole : BaseAttribute
    {
        private Enums.Groups.Capability _capability;

        public GroupRole(Enums.Groups.Capability capability)
        {
            if (capability == Enums.Groups.Capability.NotUser || capability == Enums.Groups.Capability.Silenced || capability == Enums.Groups.Capability.Banned)
                throw new Exception($"Capability cannot be set to {capability.ToString().ToUpper()}");

            _capability = capability;
        }


        public override bool Validate(WolfBot bot, CommandData command)
        {
            var validateResult = ValidateCapability(command.Group.Users.FirstOrDefault(r=>r.SubscriberId ==command.SourceSubscriberId).Capability);

            //If false trigger failed perms;

            Console.WriteLine(validateResult);

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
                        if (sourceSubscriberCapability == Enums.Groups.Capability.NotUser || sourceSubscriberCapability == Enums.Groups.Capability.Banned || sourceSubscriberCapability == Enums.Groups.Capability.Silenced)
                            return false;

                        return true;
                    }
            }
        }
    }
}
