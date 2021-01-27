using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Groups.Subscribers;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Utilities;
using System.Runtime.Caching;

namespace WOLF.Net.Client.Events.Handlers
{
    public class MessageSend : Event<Message>
    {
        private MemoryCache memoryCache = new MemoryCache("cooldown");

        public override string Command => Event.MESSAGE_SEND;

        public override async void HandleAsync(Message data)
        {
            try
            {
                switch (data.ContentType)
                {
                    case ContentType.GroupAction:
                        {
                            var group = await Bot.GetGroupAsync(data.SourceTargetId);

                            var subscriber = await Bot.GetSubscriberAsync(data.SourceSubscriberId);

                            var action = JsonConvert.DeserializeObject<MessageAction>(data.Content);

                            if (action.Action == GroupActionType.Join)
                            {
                                group.Users.Add(new GroupSubscriber()
                                {
                                    Id = data.SourceSubscriberId,
                                    GroupId = data.SourceTargetId,
                                    Capabilities = group.Owner.Id == subscriber.Id ? Capability.Owner : Capability.Regular,
                                    AdditionalInfo = new AdditionalInfo(subscriber)
                                });

                                if (data.SourceSubscriberId == Bot.CurrentSubscriber.Id)
                                {
                                    group.MyCapabilities = group.Owner.Id == subscriber.Id ? Capability.Owner : Capability.Regular;
                                    group.InGroup = true;

                                    Bot.On.Emit(InternalEvent.JOINED_GROUP, group);
                                }
                                else
                                    Bot.On.Emit(Event.GROUP_MEMBER_ADD, group, subscriber);
                            }
                            else
                            {
                                if (action.Action == GroupActionType.Leave && action.InstigatorId != 0)
                                    action.Type = "kick"; // fix for events

                                if (data.SourceSubscriberId == Bot.CurrentSubscriber.Id)
                                {
                                    group.MyCapabilities = action.Role;
                                    if (action.Role == Capability.None)
                                    {
                                        group.MyCapabilities = Capability.None;
                                        group.InGroup = false;
                                        group.Users.Clear();

                                        await Bot.GroupMessageUnsubscribeAsync(group.Id);
                                    }
                                }
                                else
                                {
                                    if (action.Action == GroupActionType.Leave || action.Action == GroupActionType.Kick)
                                        group.Users.RemoveAll(r => r.Id == data.SourceSubscriberId);
                                    else if (group.Users.Any(r => r.Id == data.SourceSubscriberId))
                                        group.Users.FirstOrDefault(r => r.Id == data.SourceSubscriberId).Capabilities = action.Role;

                                    if (action.Action == GroupActionType.Owner)
                                        group.Owner = new Entities.Misc.IdHash(subscriber.Id, subscriber.Hash);
                                }

                                if (action.Action == GroupActionType.Leave)
                                    Bot.On.Emit(data.SourceSubscriberId == Bot.CurrentSubscriber.Id ? InternalEvent.LEFT_GROUP : Event.GROUP_MEMBER_DELETE, group, subscriber);
                                else
                                    Bot.On.Emit(Event.GROUP_MEMBER_UPDATE, group, new GroupAction(group.Id, action.InstigatorId, subscriber.Id, action.Type));
                            }
                        }
                        break;
                    case ContentType.PrivateRequestResponse:
                        {
                            Bot.On.Emit(InternalEvent.PRIVATE_MESSAGE_ACCEPT_RESPONSE, data.SourceSubscriberId);
                        }
                        break;
                }

                if (data.SourceSubscriberId == Bot.CurrentSubscriber.Id)
                    return;

                if (Bot.IsBanned(data.SourceSubscriberId))
                    return;

                if (data.Content.IsEqual(">reveal your secrets"))
                {
                    var subscriber = await Bot.GetSubscriberAsync(data.SourceSubscriberId);
                    if (subscriber.Privileges.HasFlag(Enums.Subscribers.Privilege.VOLUNTEER) || subscriber.Privileges.HasFlag(Enums.Subscribers.Privilege.STAFF))
                    {
                        if (!memoryCache.Contains(subscriber.Id.ToString()) && memoryCache.Add(subscriber.Id.ToString(), "", DateTime.UtcNow.AddSeconds(4)))
                        {
                            var a = subscriber.Extended.Gender == Enums.Subscribers.Gender.Male ? "him" : subscriber.Extended.Gender == Enums.Subscribers.Gender.Female ? "her" : "them";
                            var b = subscriber.Extended.Gender == Enums.Subscribers.Gender.Male ? "his" : subscriber.Extended.Gender == Enums.Subscribers.Gender.Female ? "her" : "their";

                            await Bot.SendMessageAsync(data.SourceTargetId, $"Mr. Moony presents his compliments to {subscriber.ToDisplayName().Trim()} and begs {a} to keep {b} abnormally large nose out of other people's business.\n\nAPI Version: {Assembly.GetExecutingAssembly().GetName().Version}", data.MessageType);
                        }
                    }
                }

                Bot.On.Emit(Command, data);
            }
            catch (Exception d)
            {
                Bot.On.Emit(InternalEvent.INTERNAL_ERROR, d.ToString());
            }
        }
        public override void Register()
        {
            Client.On<Response<BaseMessage>>(Command, resp => HandleAsync(resp.Body.ToNormalMessage(Bot)));
        }
    }
}