using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Entities.Misc;
using WOLF.Net.Entities.Subscribers;
using WOLF.Net.Entities.Tip;

namespace WOLF.Net.Networking.Events
{
    public class EventHandler
    {
        private readonly WolfBot _bot;

        private readonly WebSocket _webSocket;

        #region Socket Events
        /// <summary>
        /// The websocket is connecting to the server
        /// </summary>
        public Action Connecting = delegate { };

        /// <summary>
        /// The websocket has connected to the server
        /// </summary>
        public Action Connected = delegate { };

        /// <summary>
        /// The websocket has encountered an error connecting to the server
        /// </summary>
        public Action<string> ConnectionError = delegate { };

        /// <summary>
        /// The websocket was disconnected from the server
        /// </summary>
        public Action<string> Disconnected = delegate { };

        /// <summary>
        /// The server has sent the welcome packet
        /// </summary>
        public Action<Welcome> Welcomed = delegate { };

        /// <summary>
        /// The account has successfully logged in
        /// </summary>
        public Action<Entities.Subscribers.Subscriber> LoginSuccess = delegate { };

        /// <summary>
        /// The account failed to log in
        /// </summary>
        public Action<Response> LoginFailed = delegate { };

        /// <summary>
        /// The websocket is reconnecting to the server
        /// </summary>
        public Action Reconnecting = delegate { };

        /// <summary>
        /// The websocket has connected successfully and the account has successfully logged in
        /// </summary>
        public Action Reconnected = delegate { };

        public Action<Exception> ReconnectFailed = delegate { };

        /// <summary>
        /// The bot is ready to use
        /// </summary>
        public Action Ready = delegate { };

        public Action Ping = delegate { };

        public Action<TimeSpan> Pong = delegate { };

        #endregion

        #region Group Events

        /// <summary>
        /// A groups audio configuration has been updated
        /// </summary>
        public Action<Group, AudioConfiguration> GroupAudioConfigurationUpdated = delegate { };

        /// <summary>
        /// A groups audio count has been updated
        /// </summary>
        public Action<Group, AudioCount> GroupAudioCountUpdated = delegate { };

        /// <summary>
        /// The logged in account has joined a group
        /// </summary>
        public Action<Group> JoinedGroup = delegate { };

        /// <summary>
        /// The logged in account has left a group
        /// </summary>
        public Action<Group> LeftGroup = delegate { };

        /// <summary>
        /// A groups profile has been updated
        /// </summary>
        public Action<Group> GroupUpdated = delegate { };

        /// <summary>
        /// A subscriber has joined a group
        /// </summary>
        public Action<Group, Entities.Subscribers.Subscriber> SubscriberJoined = delegate { };

        /// <summary>
        /// A subscriber has left a group
        /// </summary>
        public Action<Group, Entities.Subscribers.Subscriber> SubscriberLeft = delegate { };

        /// <summary>
        /// A subscribers capability has been updated
        /// </summary>
        public Action<Group, Entities.Groups.GroupAction> GroupMemberUpdated = delegate { };

        #endregion

        #region Internal

        /// <summary>
        /// An error has occurred internally (Please forward these to Dave (ID 29976610)
        /// </summary>
        public Action<string> Error = delegate { };

        /// <summary>
        /// Internal logging
        /// </summary>
        public Action<string> Log = delegate { };

        /// <summary>
        /// The websocket has received a packet
        /// </summary>
        public Action<string, object> PacketReceived = delegate { };

        /// <summary>
        /// The websocket has sent a packet
        /// </summary>
        public Action<string, object> PacketSent = delegate { };

        #endregion

        #region Subscriber

        /// <summary>
        /// A subscribers profile has been updated
        /// </summary>
        public Action<Entities.Subscribers.Subscriber> SubscriberUpdated = delegate { };

        /// <summary>
        /// A subscribers online state or device has changed
        /// </summary>
        public Action<Entities.Subscribers.Subscriber, PresenceUpdate> PresenceUpdate = delegate { };

        public Action<Entities.Subscribers.Subscriber> PrivateMessageRequestAccepted = delegate { };

        #endregion

        #region Commands

        /// <summary>
        /// A subscriber tried to use a command they dont have the permissions to use
        /// </summary>
        public Action<FailedPermission> PermissionFailed = delegate { };

        #endregion

        #region Messages

        /// <summary>
        /// A group or private message has been processed
        /// </summary>
        public Action<Message> MessageReceived = delegate { };

        /// <summary>
        /// A message has been updated (Deleted, Restored, TipAdded)
        /// </summary>
        public Action<MessageUpdate> MessageUpdated = delegate { };

        /// <summary>
        /// A tip has been added to a message
        /// </summary>
        public Action<Tip> TipAdded = delegate { };

        #endregion

        #region Contacts

        /// <summary>
        /// A contact has been added to your contacts list
        /// </summary>
        public Action<Entities.Subscribers.Subscriber> ContactAdded = delegate { };

        /// <summary>
        /// A contact has been removed from your contacts list
        /// </summary>
        public Action<Entities.Subscribers.Subscriber> ContactRemoved = delegate { };

        /// <summary>
        /// A subscriber has been added to your blocked list
        /// </summary>
        public Action<Entities.Subscribers.Subscriber> SubscriberBlocked = delegate { };

        /// <summary>
        /// a subscriber has been removed from your blocked list
        /// </summary>
        public Action<Entities.Subscribers.Subscriber> SubscriberUnblocked = delegate { };


        #endregion


        private void EmitEvent(string name, [Optional] object arg1, [Optional] object arg2)
        {
            try
            {
                if (_events.ContainsKey(name))
                    _events[name](arg1, arg2);
                else
                    _events[Internal.LOG]($"[INVALID EVENT]: {name}", null);
            }
            catch (Exception d)
            {
                _events[Internal.ERROR]($"Event \"{name}\" error ", d.ToString());
            }
        }

        public void Emit<T1, T2>(string eventString, T1 arg1, T2 arg2)
        {
            EmitEvent(eventString, arg1, arg2);
        }

        public void Emit<T1>(string eventString, T1 arg1)
        {
            EmitEvent(eventString, arg1);
        }

        public void Emit(string eventString)
        {
            EmitEvent(eventString);
        }

        private readonly Dictionary<string, IBaseEvent> _socketEvents = new Dictionary<string, IBaseEvent>();

        internal void Register()
        {
            foreach (var eventHandler in (from x in Assembly.GetExecutingAssembly().GetTypes() from z in x.GetInterfaces() let y = x.BaseType where (y != null && y.IsGenericType && typeof(IBaseEvent).IsAssignableFrom(y.GetGenericTypeDefinition())) || (z.IsGenericType && typeof(IBaseEvent).IsAssignableFrom(z.GetGenericTypeDefinition())) select x).Where(r => !r.IsAbstract).ToList())
            {
                IBaseEvent @event = Activator.CreateInstance(eventHandler) as IBaseEvent;

                @event.Bot = _bot;
                @event.Client = _webSocket;

                @event.Register();

                _socketEvents.Add(@event.Command, @event);

                Emit(Internal.LOG, $"[SERVER EVENT SUBSCRIPTION]: {@event.Command}");
            }
        }

        internal void Unregister()
        {
            foreach (var @event in _socketEvents)
            {
                _bot._webSocket._socket.Off(@event.Key);
                _socketEvents.Remove(@event.Key);

                Emit(Internal.LOG, $"[SERVER EVENT UNSUBSCRIPTION]: {@event.Key}");
            }
        }

        private readonly Dictionary<string, Action<object, object>> _events = new Dictionary<string, Action<object, object>>();

        internal EventHandler(WolfBot bot, WebSocket webSocket)
        {
            this._bot = bot;
            this._webSocket = webSocket;

            _events = new Dictionary<string, Action<object, object>>
            {
                [Internal.CONNECTING] = (a, b) => Connecting(),
                [Internal.CONNCETED] = (a, b) => Connected(),
                [Internal.CONNECTION_ERROR] = (a, b) => ConnectionError((string)a),
                [Internal.DISCONNECTED] = (a, b) => Disconnected((string)a),
                [Internal.RECONNECTING] = (a, b) => Reconnecting(),
                [Internal.RECONNECT_FAILED] = (a, b) => ReconnectFailed((Exception)a),
                [Internal.RECONNECTED] = (a, b) => Reconnected(),
                [Event.WELCOME] = (a, b) => Welcomed((Welcome)a),
                [Internal.LOGIN] = (a, b) => LoginSuccess((Entities.Subscribers.Subscriber)a),
                [Internal.LOGIN_FAILED] = (a, b) => LoginFailed((Response)a),
                [Request.MESSAGE_SEND] = (a, b) => MessageReceived((Message)a),
                [Request.MESSAGE_UPDATE] = (a, b) => MessageUpdated((MessageUpdate)a),
                [Event.GROUP_UPDATE] = (a, b) => GroupUpdated((Group)a),
                [Event.GROUP_AUDIO_CONFIGURATION_UPDATE] = (a, b) => GroupAudioConfigurationUpdated((Group)a, (AudioConfiguration)b),
                [Event.GROUP_AUDIO_COUNT_UPDATE] = (a, b) => GroupAudioCountUpdated((Group)a, (AudioCount)b),
                [Internal.READY] = (a, b) => Ready(),
                [Internal.JOINED_GROUP] = (a, b) => JoinedGroup((Group)a),
                [Internal.LEFT_GROUP] = (a, b) => LeftGroup((Group)a),
                [Event.GROUP_MEMBER_ADD] = (a, b) => SubscriberJoined((Group)a, (Entities.Subscribers.Subscriber)b),
                [Event.GROUP_MEMBER_DELETE] = (a, b) => SubscriberLeft((Group)a, (Entities.Subscribers.Subscriber)b),
                [Event.GROUP_MEMBER_UPDATE] = (a, b) => GroupMemberUpdated((Group)a, (Entities.Groups.GroupAction)b),
                [Event.SUBSCRIBER_UPDATE] = (a, b) => SubscriberUpdated((Entities.Subscribers.Subscriber)a),
                [Internal.LOG] = (a, b) => Log((string)a),
                [Internal.PACKET_RECEIVED] = (a, b) => PacketReceived((string)a, b),
                [Internal.PACKET_SENT] = (a, b) => PacketSent((string)a, b),
                [Internal.PERMISSIONS_FAILED] = (a, b) => PermissionFailed((FailedPermission)a),
                [Event.PRESENCE_UPDATE] = (a, b) => PresenceUpdate((Entities.Subscribers.Subscriber)a, (PresenceUpdate)b),
                [Request.SUBSCRIBER_CONTACT_ADD] = (a, b) => ContactAdded((Entities.Subscribers.Subscriber)a),
                [Request.SUBSCRIBER_CONTACT_DELETE] = (a, b) => ContactRemoved((Entities.Subscribers.Subscriber)a),
                [Request.SUBSCRIBER_BLOCK_ADD] = (a, b) => SubscriberBlocked((Entities.Subscribers.Subscriber)a),
                [Request.SUBSCRIBER_BLOCK_DELETE] = (a, b) => SubscriberUnblocked((Entities.Subscribers.Subscriber)a),
                [Request.TIP_ADD] = (a, b) => TipAdded((Tip)a),
                [Internal.PING] = (a, b) => Ping(),
                [Internal.PONG] = (a, b) => Pong((TimeSpan)a),
                [Internal.ERROR] = (a, b) => Error((string)a),
                [Internal.PRIVATE_MESSAGE_ACCEPT_RESPONSE] = (a,b)=>PrivateMessageRequestAccepted((Entities.Subscribers.Subscriber)a)
            };
        }
    }
}