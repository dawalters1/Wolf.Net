using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Commands.Form;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Charms;
using WOLF.Net.Entities.Misc;
using WOLF.Net.Entities.Store;
using WOLF.Net.Entities.Subscribers;
using WOLF.Net.Enums.API;
using WOLF.Net.Enums.Misc;
using WOLF.Net.Enums.Subscribers;
using WOLF.Net.Helper;
using WOLF.Net.Networking;

namespace WOLF.Net
{
    public class WolfBot
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
        public EndpointConfig Endpoints { get; internal set; }

        public Cognito Cognito { get; internal set; }

        public Configuration Configuration { get; private set; } = new Configuration();
        /// <summary>
        /// The current subscriber logged in
        /// </summary>
        public Subscriber CurrentSubscriber { get; internal set; }

        internal AchievementHelper _achievement;
        internal AuthorizationHelper _authorization;
        internal BannedHelper _banned;
        internal BlockHelper _blocked;
        internal ContactHelper _contact;
        internal SubscriberHelper _subscriber;
        internal GroupHelper _group;
        internal CharmHelper _charm;
        internal MessagingHelper _messaging;
        internal PhraseHelper _phrase;
        internal NotificationHelper _notification;
        internal TipHelper _tip;
        internal Networking.Events.EventHandler _eventHandler;

        /// <summary>
        /// Houses an manages the bots [Command] commands
        /// </summary>
        public CommandManager CommandManager { get; internal set; }

        /// <summary>
        /// Houses an manages the bots [Form] commands
        /// </summary>
        public FormManager FormManager { get; internal set; }

        public AchievementHelper Achievement() => _achievement;
        /// <summary>
        /// Used to bypass permissions checks for specific users
        /// </summary>
        /// <returns></returns>
        public AuthorizationHelper Authorization() => _authorization;

        /// <summary>
        /// Bans users from interacting with the bot
        /// </summary>
        /// <returns></returns>
        public BannedHelper Banned() => _banned;

        /// <summary>
        /// Used to manage your blocked list
        /// </summary>
        /// <returns></returns>
        public BlockHelper Blocked() => _blocked;

        /// <summary>
        /// Used to manage your contacts list
        /// </summary>
        /// <returns></returns>
        public ContactHelper Contact() => _contact;

        /// <summary>
        /// Used to retrieve subscriber information
        /// </summary>
        /// <returns></returns>
        public SubscriberHelper Subscriber() => _subscriber;

        /// <summary>
        /// Used to retrieve group information
        /// </summary>
        /// <returns></returns>
        public GroupHelper Group() => _group;

        /// <summary>
        /// Used to retrieve charm information
        /// </summary>
        /// <returns></returns>
        public CharmHelper Charm() => _charm;

        /// <summary>
        /// Used to manage and send messages
        /// </summary>
        /// <returns></returns>
        public MessagingHelper Messaging() => _messaging;

        /// <summary>
        /// Used to manage and retrieve phrases
        /// </summary>
        /// <returns></returns>
        public PhraseHelper Phrase() => _phrase;

        /// <summary>
        /// used to manage and retrieve notifications
        /// </summary>
        /// <returns></returns>
        public NotificationHelper Notification() => _notification;

        /// <summary>
        /// Used to retrieve tip information or to send tips to users via message or stage
        /// </summary>
        /// <returns></returns>
        public TipHelper Tip() => _tip;

        /// <summary>
        /// Houses all events the bot can have
        /// </summary>
        public Networking.Events.EventHandler On => _eventHandler;

        /// <summary>
        /// Log the bot in
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="loginDevice"></param>
        /// <param name="onlineState"></param>
        /// <param name="loginType"></param>
        /// <param name="token"></param>
        /// <returns></returns>
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

            _eventHandler.Unregister();

            _cleanUp();
        }

        /// <summary>
        /// Update the bots profile
        /// </summary>
        /// <returns>Subscriber profile builder</returns>
        public Builders.Profiles.Subscriber UpdateProfile() => new Builders.Profiles.Subscriber(this, this.CurrentSubscriber);

        /// <summary>
        /// Set the online state for the bot
        /// </summary>
        /// <param name="onlineState"></param>
        /// <returns></returns>
        public async Task<Response> SetOnlineStateAsync(OnlineState onlineState) => await _webSocket.Emit<Response>(Request.SUBSCRIBER_SETTINGS_UPDATE, new { state = new { state = (int)onlineState } /* State inside state? wtf is this shit...*/ });

        /// <summary>
        /// Set charms for the bots profile
        /// </summary>
        /// <param name="charms"></param>
        /// <returns></returns>
        public async Task<Response> SetCharmsAsync(params SelectedCharm[] charms) => await _webSocket.Emit<Response>(Request.CHARM_SUBSCRIBER_SET_SELECTED, new
        {
            selectedList = charms
        });

        /// <summary>
        /// Delete charms from the bots account
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<Response> DeleteCharmsAsync(params int[] ids) => await _webSocket.Emit<Response>(Request.CHARM_SUBSCRIBER_DELETE, new
        {
            idList = ids
        });

        /// <summary>
        /// Get bot message settings
        /// </summary>
        /// <returns></returns>
        public async Task<Response<MessageSetting>> GetMessageSettingsAsync() => await _webSocket.Emit<Response<MessageSetting>>(Request.MESSAGE_SETTING);

        /// <summary>
        /// Update bot message settings
        /// </summary>
        /// <param name="messageFilterType"></param>
        /// <returns></returns>
        public async Task<Response> UpdateMessageSettingsAsync(MessageFilterType messageFilterType) => await _webSocket.Emit<Response>(Request.MESSAGE_SETTING_UPDATE, new
        {
            spamFilter = new
            {
                enabled = messageFilterType != MessageFilterType.OFF,
                tier = (int)messageFilterType
            },
        });

        /// <summary>
        /// Search for groups or subscribers 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<Response<List<Search>>> Search(string query)
        {
            return await this._webSocket.Emit<Response<List<Search>>>(Request.SEARCH, new
            {
                query,
                types = new List<string>() { "related", "groups" }
            });
        }

        /// <summary>
        /// Retrive the credit balance for your account
        /// </summary>
        /// <returns></returns>
        public async Task<Response<CreditBalance>> GetCreditbalance() => await this._webSocket.Emit<Response<CreditBalance>>(Request.STORE_CREDIT_BALANCE);


        internal void _cleanUp()
        {
            CurrentSubscriber = null;
            _subscriber.cache.Clear();
            _group.cache.Clear();
            _charm.cache.Clear();
            _notification.cache.Clear();
            _tip.cache.Clear();
            _messaging.cache.Clear();
            _achievement._achievements.Clear();
            _achievement._categories.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usingTranslations">If you are using Bot.Phrase()</param>
        /// <param name="ignoreBots">If you wish for Official bot messages not to be processed</param>
        public WolfBot(Configuration configuration = null)
        {
            _webSocket = new WebSocket(this);
            _achievement = new AchievementHelper(this, _webSocket);
            _authorization = new AuthorizationHelper(this, _webSocket);
            _banned = new BannedHelper(this, _webSocket);
            _blocked = new BlockHelper(this, _webSocket);
            _contact = new ContactHelper(this, _webSocket);
            _subscriber = new SubscriberHelper(this, _webSocket);
            _group = new GroupHelper(this, _webSocket);
            _charm = new CharmHelper(this, _webSocket);
            _messaging = new MessagingHelper(this, _webSocket);
            _phrase = new PhraseHelper(this, _webSocket);
            _notification = new NotificationHelper(this, _webSocket);
            _tip = new TipHelper(this, _webSocket);
            _eventHandler = new Networking.Events.EventHandler(this, _webSocket);

            Configuration = configuration ?? this.Configuration;

            CommandManager = new CommandManager(this);
            FormManager = new FormManager(this);

            _eventHandler = new Networking.Events.EventHandler(this, _webSocket);

            On.MessageReceived += async msg =>
            {
                if (!await FormManager.ProcessMessage(msg))
                    await CommandManager.ProcessMessage(msg);
            };
        }
    }
}