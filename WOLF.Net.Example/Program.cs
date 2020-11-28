using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Commands.Instances;
using WOLF.Net.Entities.Phrases;
using WOLF.Net.Example.Entities;
using WOLF.Net.ExampleBot.Flows;

namespace WOLF.Net.ExampleBot
{
    class Program
    {
        public static Cache Cache = new Cache(); 
        public static void Main(string[] args)
            => new Program().Main().GetAwaiter().GetResult();

        public async Task Main()
        {
            var bot = new WolfBot();

            //JSON or Database call would go here to load translations
            bot.LoadPhrases(new List<Phrase>()
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
                    Name = "flow_exists_message",
                    Value = "Theres already a detail process in progress",
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
                    Name = "no_flow_exists_message",
                    Value = "Theres no detail process to cancel",
                    Language = "en"
                },
                new Phrase()
                {
                    Name = "flow_cancelled_message",
                    Value = "Process Cancelled (Y)",
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

            File.WriteAllText($"{AppDomain.CurrentDomain.BaseDirectory}/Phrases.json", JsonConvert.SerializeObject(bot.Phrases, Formatting.Indented));
            #region WS events

            bot.On.Connecting += () => Console.WriteLine($"[Connecting]: Connecting to V3 servers");
            bot.On.Connected += () => Console.WriteLine($"[Connected]: Connected to V3 servers");
            bot.On.Disconnected += reason => Console.WriteLine($"[Disconnected]: Disconnected from V3 servers - reason: {reason}");
            bot.On.ConnectionError += error => Console.WriteLine($"[Connection Error]: Connection error has occurred :( - error: {error}");
            bot.On.Reconnected += () => Console.WriteLine($"[Reconnected]: Reconnected to V3 servers");
            bot.On.Reconnecting += () => Console.WriteLine($"[Reconnecting]: Reconnecting to V3 servers");
            bot.On.Welcomed += welcome => Console.WriteLine($"[Welcomed]: Welcomed by V3 servers");

            #endregion

            #region Account Events

            bot.On.LoginSuccess += subscriber => Console.WriteLine($"[Login]: Logged in as {subscriber.ToDisplayName()}");
            bot.On.LoginFailed += reason => Console.WriteLine($"[Login Failed]: Login failed :( - reason: {JsonConvert.SerializeObject(reason, Formatting.Indented)}");
            bot.On.JoinedGroup += group => Console.WriteLine($"[Joined Group]: Joined group {group.ToDisplayName()}");
            bot.On.LeftGroup += group => Console.WriteLine($"[Left Group]: Left group {group.ToDisplayName()}");
            bot.On.ContactAdded += subscriber => Console.WriteLine($"[Contact Added]: {subscriber.ToDisplayName()} has been added as a contact");
            bot.On.ContactRemoved += subscriber => Console.WriteLine($"[Contact Removed]: {subscriber.ToDisplayName()} has been removed as a contact");
            bot.On.SubscriberBlocked += subscriber => Console.WriteLine($"[Subscriber Blocked]: {subscriber.ToDisplayName()} has been added as a contact");
            bot.On.SubscriberUnblocked += subscriber => Console.WriteLine($"[Subscriber Unblocked]: {subscriber.ToDisplayName()} has been removed as a contact");

            #endregion


            #region Messages Events

            bot.On.MessageReceived += async message =>
            {
                if (message.IsCommand)
                    return;

                if (message.IsGroup)
                {
                    if (await Cache.ExistsAsync(message.SourceTargetId))
                    {
                        var flow = await Cache.GetAsync<FlowData>(message.SourceTargetId);

                        if (flow.SourceSubscriberId == message.SourceSubscriberId)
                            await FlowExample.Handle(bot, message, Cache, flow);
                    }
                }
                Console.WriteLine($"[Message Received]: Received {(message.IsGroup ? "group" : "private")} message");
            };
            bot.On.MessageUpdated += message => Console.WriteLine($"[Message Updated]: Message has been udpated");

            #endregion

            #region Group Events

            bot.On.GroupAudioConfigurationUpdated += (group, config) => Console.WriteLine($"[Group Audio Config Update]: Group {group.ToDisplayName()} group audio config has been updated");
            bot.On.GroupAudioCountUpdated += (group, count) => Console.WriteLine($"[Group Audio Count Update]: Group {group.ToDisplayName()} group audio count has been updated");
            bot.On.GroupMemberUpdated += (group, action) => Console.WriteLine($"[Group Member Update]: {JsonConvert.SerializeObject(action, Formatting.Indented)}");
            bot.On.GroupUpdated += group => Console.WriteLine($"[Group Profile Update]: Group {group.ToDisplayName()} profile has been updated");
            bot.On.SubscriberJoined += (group, subscriber) => Console.WriteLine($"[Subscriber Joined]: {subscriber.ToDisplayName()} joined group {group.ToDisplayName()}");
            bot.On.SubscriberLeft += (group, subscriber) => Console.WriteLine($"[Subscriber Left]: {subscriber.ToDisplayName()} left group {group.ToDisplayName()}");

            #endregion

            #region Subscriber Events

            bot.On.SubscriberUpdated += subscriber => Console.WriteLine($"[Subscriber Profile Updated]: {subscriber.ToDisplayName()} updated their profile");
            bot.On.PresenceUpdate += (subscriber, presence) => Console.WriteLine($"[Subscriber Presence Updated]: {subscriber.ToDisplayName()} presence changed - device: {presence.DeviceType} - onlineState: {presence.OnlineState}");

            #endregion

            #region Command Events

            bot.On.PermissionFailed += async permFailure =>
            {
                if (permFailure.IsGroup)
                    await bot.SendGroupMessageAsync(permFailure.SourceTargetId, bot.GetPhraseByName(permFailure.Language, "permissions_failed_message"));
                else
                    await bot.SendPrivateMessageAsync(permFailure.SourceTargetId, bot.GetPhraseByName(permFailure.Language, "permissions_failed_message"));
            };

            #endregion

            #region API Events

            bot.On.Log += log => Console.WriteLine($"[LOG]: {log}");
            bot.On.InternalError += error => Console.WriteLine($"[INTERNAL ERROR]: {error}");
            #endregion

            await bot.LoginAsync("example@email.xyz", "examplePassword");

            await Task.Delay(-1);
        }
    }
}