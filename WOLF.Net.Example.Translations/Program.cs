﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Commands.Instances;
using WOLF.Net.Entities.Phrases;
using WOLF.Net.Example.Entities;
using WOLF.Net.ExampleBot.Flows;
using WOLF.Net.Utilities;

namespace WOLF.Net.ExampleBot
{
    class Program
    {
        /// <summary>
        /// Create a new bot instance, with bool true
        /// </summary>
        public static WolfBot Bot = new WolfBot(true);

        public static Cache Cache = new Cache();
        public static void Main(string[] args)
            => new Program().Main().GetAwaiter().GetResult();

        public async Task Main()
        {
            //JSON or Database call would go here to load translations
            Bot.LoadPhrases(new List<Phrase>()
            {
                new Phrase()
                {
                     Name = "example",
                     Value = "!example",
                     Language = "en"
                },
                new Phrase()
                {
                     Name = "help",
                     Value = "help",
                     Language = "en"
                },
                new Phrase()
                {
                     Name = "help_message",
                     Value = "Welcome to the example bot\n\n!example help - Will display this message\n!example get subscriber <subscriberId> - get a subscriber profile\n!example get group <groupId> - get a group profile",
                     Language = "en"
                },
                new Phrase()
                {
                     Name = "get",
                     Value = "get",
                     Language = "en"
                },
                new Phrase()
                {
                     Name = "subscriber",
                     Value = "subscriber",
                     Language = "en"
                },
                new Phrase()
                {
                     Name = "no_such_subscriber_exists_message",
                     Value = "No such subscriber exists with this ID",
                     Language = "en"
                },
                new Phrase()
                {
                     Name = "group",
                     Value = "group",
                     Language = "en"
                },
                new Phrase()
                {
                     Name = "no_such_group_exists_message",
                     Value = "No such group exists with this ID",
                     Language = "en"
                },
                new Phrase()
                {
                    Name = "permissions_failed_message",
                    Value = "You do not have the proper permissions to use this command",
                    Language = "en"
                },
                new Phrase()
                {
                    Name = "start",
                    Value = "start",
                    Language = "en"
                },
                new Phrase()
                {
                    Name = "form_exists_self_message",
                    Value = "(N) You've already started a form!",
                    Language = "en"
                },
                new Phrase()
                {
                    Name = "form_exists_other_message",
                    Value = "(N) {0} has already started a form!",
                    Language = "en"
                },
                new Phrase()
                {
                    Name = "send_age_message",
                    Value = "How old are you?",
                    Language = "en"
                },
                new Phrase()
                {
                    Name = "cancel",
                    Value = "cancel",
                    Language = "en"
                },
                new Phrase()
                {
                    Name = "no_form_exists_message",
                    Value = "(N) Theres nothing to cancel",
                    Language = "en"
                },
                new Phrase()
                {
                    Name = "form_cancelled_message",
                    Value = "(Y) Form Cancelled",
                    Language = "en"
                },
                new Phrase()
                {
                    Name = "doubt_you_are_that_young_message",
                    Value = "I find it highly unlikely that you are that young",
                    Language = "en"
                },
                new Phrase()
                {
                    Name = "doubt_you_are_that_old_message",
                    Value = "I find it highly unlikely that you are that old",
                    Language = "en"
                },
                new Phrase()
                {
                    Name = "send_name_message",
                    Value = "Alright then, what is your name?",
                    Language = "en"
                },
                new Phrase()
                {
                    Name = "invalid_number_message",
                    Value = "That is not a number!",
                    Language = "en"
                },
                new Phrase()
                {
                    Name = "details_message",
                    Value = "You are {0} years old and your name is {1}",
                    Language = "en"
                }
            });

            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}/Phrases.json", JsonConvert.SerializeObject(Bot.Phrases, Formatting.Indented));

            #region WS events

            Bot.On.Connecting += () => Console.WriteLine($"[Connecting]: Connecting to V3 servers");
            Bot.On.Connected += () => Console.WriteLine($"[Connected]: Connected to V3 servers");
            Bot.On.Disconnected += reason => Console.WriteLine($"[Disconnected]: Disconnected from V3 servers - reason: {reason}");
            Bot.On.ConnectionError += error => Console.WriteLine($"[Connection Error]: Connection error has occurred :( - error: {error}");
            Bot.On.Reconnected += () => Console.WriteLine($"[Reconnected]: Reconnected to V3 servers");
            Bot.On.Reconnecting += () => Console.WriteLine($"[Reconnecting]: Reconnecting to V3 servers");
            Bot.On.Welcomed += welcome => Console.WriteLine($"[Welcomed]: Welcomed by V3 servers");

            #endregion

            #region Account Events

            Bot.On.LoginSuccess += subscriber => Console.WriteLine($"[Login]: Logged in as {subscriber.ToDisplayName()}");
            Bot.On.LoginFailed += reason => Console.WriteLine($"[Login Failed]: Login failed :( - reason: {JsonConvert.SerializeObject(reason, Formatting.Indented)}");
            Bot.On.JoinedGroup += group => Console.WriteLine($"[Joined Group]: Joined group {group.ToDisplayName()}");
            Bot.On.LeftGroup += group => Console.WriteLine($"[Left Group]: Left group {group.ToDisplayName()}");
            Bot.On.ContactAdded += subscriber => Console.WriteLine($"[Contact Added]: {subscriber.ToDisplayName()} has been added as a contact");
            Bot.On.ContactRemoved += subscriber => Console.WriteLine($"[Contact Removed]: {subscriber.ToDisplayName()} has been removed as a contact");
            Bot.On.SubscriberBlocked += subscriber => Console.WriteLine($"[Subscriber Blocked]: {subscriber.ToDisplayName()} has been added as a contact");
            Bot.On.SubscriberUnblocked += subscriber => Console.WriteLine($"[Subscriber Unblocked]: {subscriber.ToDisplayName()} has been removed as a contact");

            Bot.On.Ready += async () =>
            {
                Console.WriteLine("[Ready]: Bot is ready for use");

                //Updating subscriber profiles
                await Bot.CurrentSubscriber.UpdateProfile(Bot).SetNickname("Update Example Nickname").SetStatus("Update Example Status").Save();
            };

            #endregion

            #region Messages Events

            Bot.On.MessageReceived += async message =>
            {
                Console.WriteLine($"[Message Received]: Received {(message.IsGroup ? "group" : "private")} message");

                if (message.IsCommand)
                    return;

                if (message.IsGroup)
                {
                    if (await Cache.ExistsAsync(message.SourceTargetId))
                    {
                        var flow = await Cache.GetAsync<FormData>(message.SourceTargetId);

                        if (flow.SourceSubscriberId == message.SourceSubscriberId)
                            await FormExample.Handle(Bot, message, Cache, flow);
                    }
                }
            };
            Bot.On.MessageUpdated += message => Console.WriteLine($"[Message Updated]: Message has been udpated");

            #endregion

            #region Group Events

            Bot.On.GroupAudioConfigurationUpdated += (group, config) => Console.WriteLine($"[Group Audio Config Update]: Group {group.ToDisplayName()} group audio config has been updated");
            Bot.On.GroupAudioCountUpdated += (group, count) => Console.WriteLine($"[Group Audio Count Update]: Group {group.ToDisplayName()} group audio count has been updated");
            Bot.On.GroupMemberUpdated += (group, action) => Console.WriteLine($"[Group Member Update]: {JsonConvert.SerializeObject(action, Formatting.Indented)}");
            Bot.On.GroupUpdated += group => Console.WriteLine($"[Group Profile Update]: Group {group.ToDisplayName()} profile has been updated");
            Bot.On.SubscriberJoined += (group, subscriber) => Console.WriteLine($"[Subscriber Joined]: {subscriber.ToDisplayName()} joined group {group.ToDisplayName()}");
            Bot.On.SubscriberLeft += (group, subscriber) => Console.WriteLine($"[Subscriber Left]: {subscriber.ToDisplayName()} left group {group.ToDisplayName()}");

            #endregion

            #region Subscriber Events

            Bot.On.SubscriberUpdated += subscriber => Console.WriteLine($"[Subscriber Profile Updated]: {subscriber.ToDisplayName()} updated their profile");
            Bot.On.PresenceUpdate += (subscriber, presence) => Console.WriteLine($"[Subscriber Presence Updated]: {subscriber.ToDisplayName()} presence changed - device: {presence.DeviceType} - onlineState: {presence.OnlineState}");

            #endregion

            #region Command Events

            Bot.On.PermissionFailed += async permFailure =>
            {
                if (permFailure.IsGroup)
                    await Bot.SendGroupMessageAsync(permFailure.SourceTargetId, Bot.GetPhraseByName(permFailure.Language, "permissions_failed_message"));
                else
                    await Bot.SendPrivateMessageAsync(permFailure.SourceTargetId, Bot.GetPhraseByName(permFailure.Language, "permissions_failed_message"));
            };

            #endregion

            #region API Events

            Bot.On.Log += log => Console.WriteLine($"[Log]: {log}");
            Bot.On.InternalError += error => Console.WriteLine($"[Internal Error]: {error}");

            #endregion

            await Bot.LoginAsync("example@email.xyz", "examplePassword");

            await Task.Delay(-1);
        }
    }
}