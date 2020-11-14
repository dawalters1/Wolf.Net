using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Client;
using WOLF.Net.Client.Events;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Subscribers;
using WOLF.Net.Enums.API;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        private CommandManager CommandManager { get; set; }

        public Subscriber CurrentSubscriber { get; internal set; }

        public WolfClient WolfClient { get; private set; }

        public EventManager On { get; private set; }

        public LoginData LoginData { get; private set; }

        public WolfBot()
        {
            WolfClient = new WolfClient(this);

            CommandManager = new CommandManager(this);

            On = new EventManager();

            On.MessageReceived += async msg =>
            {
                await CommandManager.ProcessMessage(msg);

                foreach (var subscription in currentMessageSubscriptions.ToList())
                {
                    if (!subscription.Key(msg))
                        continue;

                    currentMessageSubscriptions.Remove(subscription.Key);

                    subscription.Value.SetResult(msg);
                }
            };
        }

        public async Task LoginAsync(string email, string password, LoginDevice loginDevice = LoginDevice.Android, OnlineState onlineState = OnlineState.Online)
        {
            LoginData = new LoginData(email, password, loginDevice, LoginType.Email, onlineState);

            await WolfClient.CreateSocket();
        }

        public async Task LogoutAsync()
        {
            await InternalLogoutAsync();

            await WolfClient.Socket.DisconnectAsync();

            On.UnregisterEvents(this);

            WolfClient.Socket = null;
        }
    }
}