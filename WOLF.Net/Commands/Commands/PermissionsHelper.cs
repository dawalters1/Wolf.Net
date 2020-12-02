using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Attributes;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net.Commands.Commands
{
    internal static class PermissionsHelper
    {
        #region Command Collection
        internal static MessageType GetMessageTypeOrDefault(this Type type)
        {
            var messageType = type.GetCustomAttribute<RequiredMessageType>();

            if (messageType == null)
                return MessageType.Both;

            return messageType._messageType;
        }

        internal static Capability GetRequiredCapabilityOrDefault(this Type type)
        {
            var capability = type.GetCustomAttribute<RequiredPermissions>();

            if (capability == null)
                return Capability.None;

            return capability._capability;
        }

        internal static List<Privilege> GetRequiredPrivilegesOrDefault(this Type type)
        {
            var capability = type.GetCustomAttribute<RequiredPermissions>();

            if (capability == null)
                return new List<Privilege>();

            return capability._privileges;
        }
        internal static bool GetIsAuthOnly(this Type type) => type.GetCustomAttribute<AuthOnly>() != null;

        #endregion

        #region Commands
        internal static MessageType GetMessageTypeOrDefault(this MethodInfo type)
        {
            var messageType = type.GetCustomAttribute<RequiredMessageType>();

            if (messageType == null)
                return MessageType.Both;

            return messageType._messageType;
        }

        internal static Capability GetRequiredCapabilityOrDefault(this MethodInfo type)
        {
            var capability = type.GetCustomAttribute<RequiredPermissions>();

            if (capability == null)
                return Capability.None;

            return capability._capability;
        }

        internal static List<Privilege> GetRequiredPrivilegesOrDefault(this MethodInfo type)
        {
            var capability = type.GetCustomAttribute<RequiredPermissions>();

            if (capability == null)
                return new List<Privilege>();

            return capability._privileges;
        }
        internal static bool GetIsAuthOnly(this MethodInfo type) => type.GetCustomAttribute<AuthOnly>() != null;


        #endregion

        internal static async Task<bool> ValidatePermissions(this WolfBot bot, CommandData command, Capability capability, List<Privilege> privileges)
        {
            if (privileges.Count > 0)
            {
                bool hasPrivs = privileges.Any(r => command.Subscriber.Privileges.HasFlag(r));

                if (hasPrivs)
                    return true;

                if (!hasPrivs && capability == Capability.None)
                {
                    bot.On.Emit(InternalEvent.PERMISSIONS_FAILED, new FailedPermission()
                    {
                        SourceTargetId = command.SourceTargetId,
                        SourceSubscriberId = command.SourceSubscriberId,
                        Language = command.Language,
                        Privileges = privileges,
                        IsGroup = command.IsGroup
                    });
                    return false;
                }
            }

            bool validateResult = true;

            if (bot.IsAuthorized(command.SourceSubscriberId))
                return true;

            else if (capability == Capability.Regular || capability == Capability.None)
                return true;

            else if (command.Group != null && command.SourceSubscriberId == command.Group.Owner.Id)
                return true;

            else
            {
                var subscribersList = await bot.GetGroupSubscribersListAsync(command.SourceTargetId);
                var groupSubscriber = subscribersList.FirstOrDefault(r => r.Id == command.SourceSubscriberId);

                validateResult = groupSubscriber == null ? false : (capability) switch
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
                bot.On.Emit(InternalEvent.PERMISSIONS_FAILED, new FailedPermission()
                {
                    SourceTargetId = command.SourceTargetId,
                    SourceSubscriberId = command.SourceSubscriberId,
                    Capabilities = capability,
                    Language = command.Language,
                    Privileges = privileges,
                    IsGroup = command.IsGroup
                });

            return validateResult;
        }
    }
}
