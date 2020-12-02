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
    public class RequiredPermissions : Attribute
    {
        internal Enums.Groups.Capability _capability = Capability.None;

        internal List<Privilege> _privileges { get; set; } = new List<Privilege>();

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

        public RequiredPermissions(Enums.Groups.Capability capability, [Optional] params Privilege[] privileges)
        {
            if (capability == Enums.Groups.Capability.None || capability == Enums.Groups.Capability.Silenced || capability == Enums.Groups.Capability.Banned)
                throw new Exception($"Capability cannot be set to {capability.ToString().ToUpper()}");

            _capability = capability;
            _privileges = privileges.ToList();
        }

        /* switch (_capability)
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
        */

    }
}