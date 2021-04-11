using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using WOLF.Net;
using WOLF.Net.Entities.API;

namespace Wolf.Net.Example
{
    public class Program
    {
        /// <summary>
        /// Create a new bot instance
        /// </summary>
        public static WolfBot Bot = new WolfBot(true, true);

        public static void Main(string[] args)
            => new Program().Main().GetAwaiter().GetResult();

#pragma warning disable CA1822 // Mark members as static
        public async Task Main()
#pragma warning restore CA1822 // Mark members as static
        {
            Bot.Phrase().Load(JsonConvert.DeserializeObject<Phrase[]>(File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}/Phrases/en.json")));

            #region WS events

            Bot.On.Connecting += () => Console.WriteLine($"[Connecting]: Connecting to V3 servers");
            Bot.On.Connected += () => Console.WriteLine($"[Connected]: Connected to V3 servers");
            Bot.On.Disconnected += reason => Console.WriteLine($"[Disconnected]: Disconnected from V3 servers - reason: {reason}");
            Bot.On.ConnectionError += error => Console.WriteLine($"[Connection Error]: Connection error has occurred :( - error: {error}");
            Bot.On.Reconnected += () => Console.WriteLine($"[Reconnected]: Reconnected to V3 servers");
            Bot.On.Reconnecting += () => Console.WriteLine($"[Reconnecting]: Reconnecting to V3 servers");
            Bot.On.ReconnectFailed += error => Console.WriteLine($"[Reconnection Failed]: Failed to reconnect to V3 servers");
            Bot.On.Welcomed += welcome => Console.WriteLine($"[Welcomed]: Welcomed by V3 servers");
            Bot.On.Ping += () => Console.WriteLine($"[PING]");
            Bot.On.Pong += ts => Console.WriteLine($"[PONG]: {ts.Milliseconds}ms");

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
            Bot.On.PrivateMessageRequestAccepted += (subscriber) => Console.WriteLine($"[Subscriber Private Message Request Response]: {subscriber.Nickname} has accepted your private message request");

            #endregion

            #region Messages Events

            Bot.On.MessageReceived += async message =>
            {
                Console.WriteLine($"[Message Received]: Received {(message.IsGroup ? "group" : "private")} message [isCommand: {(message.IsCommand ? "Yes" : "No")}]");

                if (message.IsCommand)
                    return;

                if (!message.IsGroup && !Bot.FormManager.HasPrivateForm(message.SourceSubscriberId))
                    await Bot.Messaging().SendPrivateMessageAsync(message.SourceSubscriberId, Bot.Phrase().GetByName("en", "example_help_message"));
            };

            Bot.On.MessageUpdated += message => Console.WriteLine($"[Message Updated]: Message has been udpated");

            Bot.On.TipAdded += tip => Console.WriteLine($"[Tip Added]: {JsonConvert.SerializeObject(tip, Formatting.Indented)}");

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
                    await Bot.Messaging().SendGroupMessageAsync(permFailure.TargetGroupId, "You do not have the proper permissions to use this command");
                else
                    await Bot.Messaging().SendPrivateMessageAsync(permFailure.SourceSubscriberId, "You do not have the proper permissions to use this command");
            };

            #endregion

            #region API Events

            Bot.On.Log += log => Console.WriteLine($"[Log]: {log}");
            Bot.On.Error += error => Console.WriteLine($"[Internal Error]: {error}");

            Bot.On.Ready += async () =>
            {
                Console.WriteLine("[Ready]: Bot is ready for use");

                //Updating subscriber profiles
                await Bot.UpdateProfile().SetNickname(Bot.CurrentSubscriber.Nickname).SetStatus(Bot.CurrentSubscriber.Status).Save();
            };
            #endregion

            await Bot.LoginAsync("email@xyz.com", "password");

            await Task.Delay(-1);
        }
    }
}