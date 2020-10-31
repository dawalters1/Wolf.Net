using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Enums.Group;

namespace WOLF.Net.Commands.Attributes
{
    public class GroupRole : BaseAttribute
    {
        private Enums.Group.Capability _capability;

        public GroupRole(Enums.Group.Capability capability)
        {
            if (capability == Enums.Group.Capability.NotUser || capability == Enums.Group.Capability.Silenced || capability == Enums.Group.Capability.Banned)
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


        private bool ValidateCapability(Enums.Group.Capability sourceSubscriberCapability)
        {
            switch (_capability)
            {
                case  Enums.Group.Capability.Owner:
                    return sourceSubscriberCapability == Enums.Group.Capability.Owner;
                case Enums.Group.Capability.Admin:
                    return sourceSubscriberCapability == Enums.Group.Capability.Admin || sourceSubscriberCapability == Enums.Group.Capability.Owner;
                case Enums.Group.Capability.Mod:
                    return sourceSubscriberCapability == Enums.Group.Capability.Mod || sourceSubscriberCapability == Enums.Group.Capability.Admin || sourceSubscriberCapability == Enums.Group.Capability.Owner;
                default:
                    {
                        if (sourceSubscriberCapability == Enums.Group.Capability.NotUser || sourceSubscriberCapability == Enums.Group.Capability.Banned || sourceSubscriberCapability == Enums.Group.Capability.Silenced)
                            return false;

                        return true;
                    }
            }
        }
    }
}
