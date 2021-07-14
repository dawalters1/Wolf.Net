using System.Collections.Generic;
using System.Threading.Tasks;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Commands.Form;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Misc;
using WOLF.Net.Entities.Store;
using WOLF.Net.Entities.Subscribers;
using WOLF.Net.Enums.API;
using WOLF.Net.Enums.Subscribers;
using WOLF.Net.Networking;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        /// <summary>
        /// The websocket instance for the bot
        /// </summary>
        public WebSocket _webSocket { get; private set; }

        /// <summary>
        /// The login data for the bot
        /// </summary>
        public LoginSetting LoginSettings { get; private set; }

        /// <summary>
        /// Houses the avatar end point and MMS endpoint
        /// </summary>
        public EndpointConfig EndPoints { get; private set; }

        public Configuration Configuration { get; private set; } = new Configuration();
        /// <summary>
        /// The current subscriber logged in
        /// </summary>
        public Subscriber CurrentSubscriber { get; internal set; }

        internal CommandManager CommandManager { get; set; }

        public FormManager FormManager { get; set; }

        public Networking.Events.EventHandler On { get; private set; }


        /// <summary>
        /// Create an instance of a bot
        /// </summary>
        /// <param name="usingTranslations">Set to true if you plan on using <see cref="LoadPhrases(List{Entities.Phrases.Phrase})"/></param>
        public WolfBot(Configuration configuration = null)
        {
            _webSocket = new WebSocket(this);

            Configuration = configuration ?? this.Configuration;

            CommandManager = new CommandManager(this);
            FormManager = new FormManager(this);

            On = new Networking.Events.EventHandler(this, _webSocket);

            On.MessageReceived += async msg =>
            {
                if (!await FormManager.ProcessMessage(msg))
                    await CommandManager.ProcessMessage(msg);
            };
        }

        public async Task LoginAsync(string email, string password, LoginDevice loginDevice = LoginDevice.ANDROID, OnlineState onlineState = OnlineState.ONLINE, LoginType loginType = LoginType.EMAIL, string token = null)
        {
            LoginSettings = new LoginSetting(email, password, loginDevice, loginType, onlineState, token);
            CommandManager.Load();
            FormManager.Load();
            await _webSocket.CreateSocket();
        }

        /// <summary>
        /// Log the bot out
        /// </summary>
        /// <returns></returns>
        public async Task LogoutAsync()
        {
            await _webSocket.Emit<Response>(Request.SECURITY_LOGOUT);

            await _webSocket._socket.DisconnectAsync();

            On.Unregister();

            _cleanUp();
        }


        internal void _cleanUp()
        {
            CurrentSubscriber = null;
            Groups.Clear();
            Subscribers.Clear();
            contactCache.Clear();
            blockedCache.Clear();
            charmCache.Clear();
            achievementCache.Clear();
            categoryCache.Clear();
        }
    }
}