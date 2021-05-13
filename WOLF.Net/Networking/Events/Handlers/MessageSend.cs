using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using WOLF.Net.Constants;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Messages;

namespace WOLF.Net.Networking.Events.Handlers
{
    public class MessageSend : BaseEvent<BaseMessage>
    {
        /// <summary>
        /// Because why not, dont like it? Fuck off :)
        /// </summary>
        private readonly Dictionary<string, List<string>> secrets = new Dictionary<string, List<string>>()
        {
            {">reveal your secrets", new List<string>()
                {
                    "I'd love to stay and chat, but I'm lying.\nWDN: {0}",
                    "Hey, I found your nose... It was in my Business\nWDN: {0}",
                    "In my defense, I was left unsupervised\nWDN: {0}",
                    "I am a bot using\nWDN: {0}",
                    "Maybe you should get your own life and stop interfering in mine\nWDN: {0}",
                    "Nothing will bring you greater peace than minding your own business.\nWDN: {0}",
                    "I am who I am, your approval isnt needed\nWDN: {0}"
                }
            },
            {">sırlarını ifşala", new List<string>()
                {
                    "Kalıp sizinle sohbet etmek istiyorum derdim ama, yalan olur.\nWDN: {0}",
                    "Ayy burnunu buldum… Benim işlerimin arasından çıktı.\nWDN: {0}",
                    "Kendimi savunmak için diyorum, gözetimsiz bırakılmıştım\nWDN: {0}",
                    "Güzel selfi çekmek için 10 resim çekiyorsan, çirkinsin; bunun ötesi berisi yok.\nWDN: {0}",
                    "Gidince arkasından üzüleceğim tek şey, internet.\nWDN: {0}"
                }
            },
        };
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
                            var group = await Bot.GetGroupAsync(fixedMessage.TargetGroupId);

                            var subscriber = await Bot.GetSubscriberAsync(fixedMessage.SourceSubscriberId);

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

                                        await Bot.GroupUnsubscribeAsync(group.Id);
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
                            Bot.On.Emit(Internal.PRIVATE_MESSAGE_ACCEPT_RESPONSE, await Bot.GetSubscriberAsync(fixedMessage.SourceSubscriberId));
                        }
                        break;
                }

                Bot._currentMessageSubscriptions.Where(r => r.Key(fixedMessage)).ToList().ForEach((match) => { Bot._currentMessageSubscriptions.Remove(match.Key); match.Value.SetResult(fixedMessage); });

                if (Bot.Configuration.IgnoreOfficialBots && (await Bot.GetSubscriberAsync(fixedMessage.SourceSubscriberId)).HasPrivilege(Enums.Subscribers.Privilege.BOT))
                    return;

                if (fixedMessage.SourceSubscriberId == Bot.CurrentSubscriber.Id || Bot.IsBanned(fixedMessage.SourceSubscriberId))
                    return;

                if (secrets.Keys.Any((key) => Regex.IsMatch(fixedMessage.Content, $@"\A{key}\b")))
                {
                    var phrases = secrets.Where((secret) => Regex.IsMatch(fixedMessage.Content, $@"\A{secret.Key}\b")).FirstOrDefault().Value;

                    var subscriber = await Bot.GetSubscriberAsync(fixedMessage.SourceSubscriberId);
                    if (subscriber.Privileges.HasFlag(Enums.Subscribers.Privilege.VOLUNTEER) || subscriber.Privileges.HasFlag(Enums.Subscribers.Privilege.STAFF))
                    {
                        await Bot.SendMessageAsync(fixedMessage.IsGroup ? fixedMessage.TargetGroupId : fixedMessage.SourceSubscriberId, string.Format(phrases.OrderBy((secret) => Guid.NewGuid()).FirstOrDefault(), Assembly.GetExecutingAssembly().GetName().Version), fixedMessage.MessageType);
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