using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Messages;
namespace WOLF.Net.Networking.Events.Handlers
{
    public class MessageSend : BaseEvent<BaseMessage>
    {
        public override string Command => Event.MESSAGE_SEND;

        public override bool ReturnBody => true;

        public override async void Handle(BaseMessage data)
        {
            var fixedMessage = data.NormalizeMessage(Bot);

            try
            {
                switch (fixedMessage.ContentType)
                {
                    case ContentType.GROUP_ACTION:
                        {
                            var group = await Bot.Group().GetByIdAsync(fixedMessage.TargetGroupId);

                            var subscriber = await Bot.Subscriber().GetByIdAsync(fixedMessage.SourceSubscriberId);

                            var action = JsonConvert.DeserializeObject<MessageAction>(fixedMessage.Content);

                            if (action.Action == ActionType.JOIN)
                            {
                                group.Subscribers.Add(new Entities.Groups.Subscriber()
                                {
                                    Id = fixedMessage.SourceSubscriberId,
                                    GroupId = fixedMessage.TargetGroupId,
                                    Capabilities = group.Owner.Id == subscriber.Id ? Capability.OWNER : Capability.REGULAR,
                                    AdditionalInfo = new AdditionalInfo(subscriber)
                                });

                                if (fixedMessage.SourceSubscriberId == Bot.CurrentSubscriber.Id)
                                {
                                    group.MyCapabilities = group.Owner.Id == subscriber.Id ? Capability.OWNER : Capability.REGULAR;
                                    group.InGroup = true;

                                    Bot.On.Emit(Internal.JOINED_GROUP, group);
                                }
                                else
                                    Bot.On.Emit(Event.GROUP_MEMBER_ADD, group, subscriber);
                            }
                            else
                            {
                                if (action.Action == ActionType.LEAVE && action.InstigatorId != 0)
                                    action.Type = "kick"; // fix for events

                                if (fixedMessage.SourceSubscriberId == Bot.CurrentSubscriber.Id)
                                {
                                    group.MyCapabilities = action.Role;
                                    if (action.Role == Capability.NOT_MEMBER)
                                    {
                                        group.MyCapabilities = Capability.NOT_MEMBER;
                                        group.InGroup = false;
                                        group.Subscribers.Clear();

                                        await Bot.Messaging().GroupUnsubscribeAsync(group.Id);
                                    }
                                }
                                else
                                {
                                    if (action.Action == ActionType.LEAVE || action.Action == ActionType.KICK)
                                        group.Subscribers.RemoveAll(r => r.Id == fixedMessage.SourceSubscriberId);
                                    else if (group.Subscribers.Any(r => r.Id == fixedMessage.SourceSubscriberId))
                                        group.Subscribers.FirstOrDefault(r => r.Id == fixedMessage.SourceSubscriberId).Capabilities = action.Role;

                                    if (action.Action == ActionType.OWNER)
                                        group.Owner = new Entities.Misc.IdHash(subscriber.Id, subscriber.Hash);
                                }

                                if (action.Action == ActionType.LEAVE)
                                    Bot.On.Emit(fixedMessage.SourceSubscriberId == Bot.CurrentSubscriber.Id ? Internal.LEFT_GROUP : Event.GROUP_MEMBER_DELETE, group, subscriber);
                                else
                                    Bot.On.Emit(Event.GROUP_MEMBER_UPDATE, group, new GroupAction(group.Id, action.InstigatorId, subscriber.Id, action.Type));
                            }
                        }
                        break;
                    case ContentType.PRIVATE_REQUEST_RESPONSE:
                        {
                            Bot.On.Emit(Internal.PRIVATE_MESSAGE_ACCEPT_RESPONSE, await Bot.Subscriber().GetByIdAsync(fixedMessage.SourceSubscriberId));
                        }
                        break;
                }

                Bot.Messaging()._currentMessageSubscriptions.Where(r => r.Key(fixedMessage)).ToList().ForEach((match) => { Bot.Messaging()._currentMessageSubscriptions.Remove(match.Key); match.Value.SetResult(fixedMessage); });

                if (Bot.Configuration.IgnoreOfficialBots && (await Bot.Subscriber().GetByIdAsync(fixedMessage.SourceSubscriberId)).HasPrivilege(Enums.Subscribers.Privilege.BOT))
                    return;

                if (fixedMessage.SourceSubscriberId == Bot.CurrentSubscriber.Id || Bot.Banned().IsBanned(fixedMessage.SourceSubscriberId))
                    return;

                if (fixedMessage.Content.IsEqual(">reveal your secrets"))
                {
                    var subscriber = await Bot.Subscriber().GetByIdAsync(fixedMessage.SourceSubscriberId);
                    if (subscriber.Privileges.HasFlag(Enums.Subscribers.Privilege.VOLUNTEER) || subscriber.Privileges.HasFlag(Enums.Subscribers.Privilege.STAFF))
                    {
                        await Bot.Messaging().SendMessageAsync(fixedMessage.IsGroup ? fixedMessage.TargetGroupId : fixedMessage.SourceSubscriberId, $"I'd love to stay and chat, but I'm lying.\nWDN: {Assembly.GetExecutingAssembly().GetName().Version}", fixedMessage.MessageType);
                        return;
                    }
                }

                Bot.On.Emit(Command, fixedMessage);
            }
            catch (Exception d)
            {
                Bot.On.Emit(Internal.ERROR, d.ToString());
            }
        }
    }
}