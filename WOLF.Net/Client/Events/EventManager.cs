using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Entities.Groups.Subscribers;
using WOLF.Net.Entities.Groups.Stages;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Entities.Messages.Tipping;
using WOLF.Net.Entities.Subscribers;
using WOLF.Net.Constants;
using Newtonsoft.Json;

namespace WOLF.Net.Client.Events
{
    public class EventManager
    {
        #region ConnectionEvents

        // public Action Connecting = delegate { };
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
        public Action<Subscriber> LoginSuccess = delegate { };

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

        /// <summary>
        /// The bot is ready to use
        /// </summary>
        public Action Ready = delegate { };
        #endregion

        #region Group Events
        /// <summary>
        /// A groups audio configuration has been updated
        /// </summary>
        public Action<Group, GroupAudioConfiguration> GroupAudioConfigurationUpdated = delegate { };

        /// <summary>
        /// A groups audio count has been updated
        /// </summary>
        public Action<Group, GroupAudioCount> GroupAudioCountUpdated = delegate { };

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
        /// A user has joined a group
        /// </summary>
        public Action<Group, Subscriber> UserJoined = delegate { };

        /// <summary>
        /// A user has left a group
        /// </summary>
        public Action<Group, Subscriber> UserLeft = delegate { };

        /// <summary>
        /// A users capability has been updated
        /// </summary>
        public Action<Group, GroupAction> GroupMemberUpdated = delegate { };

        #endregion


        #region Internal

        /// <summary>
        /// An error has occurred internally (Please forward these to Dave (ID 29976610)
        /// </summary>
        public Action<string> InternalError = delegate { };

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

        #region User

        /// <summary>
        /// A users profile has been updated
        /// </summary>
        public Action<Subscriber> UserUpdated = delegate { };

        /// <summary>
        /// A users online state or device has changed
        /// </summary>
        public Action<Subscriber, PresenceUpdate> PresenceUpdate = delegate { };
        #endregion

        #region Commands

        /// <summary>
        /// A user tried to use a command they dont have the permissions to use
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
        public Action<Subscriber> ContactAdded = delegate { };

        /// <summary>
        /// A contact has been removed from your contacts list
        /// </summary>
        public Action<Subscriber> ContactRemoved = delegate { };

        /// <summary>
        /// A user has been added to your blocked list
        /// </summary>
        public Action<Subscriber> UserBlocked = delegate { };

        /// <summary>
        /// a user has been removed from your blocked list
        /// </summary>
        public Action<Subscriber> UserUnblocked = delegate { };

        #endregion
        private readonly Dictionary<string, Action<object, object>> _events;

        /// <summary>
        /// Default constructor
        /// </summary>
        public EventManager()
        {
            _events = new Dictionary<string, Action<object, object>>
            {
                // [InternalEvent.connecting] = (a,b)=>Connecting(),
                [InternalEvent.CONNCETED] = (a, b) => Connected(),
                [InternalEvent.CONNECTION_ERROR] = (a, b) => ConnectionError((string)a),
                [InternalEvent.DISCONNECTED] = (a, b) => Disconnected((string)a),
                [InternalEvent.INTERNAL_ERROR] = (a, b) => InternalError((string)a),
                [InternalEvent.RECONNECTING] = (a, b) => Reconnecting(),
              //  [InternalEvent.reconnected] = (a, b) => Reconnected(),
                [Event.WELCOME] = (a, b) => Welcomed((Welcome)a),
                [InternalEvent.LOGIN] = (a, b) => LoginSuccess((Subscriber)a),
                [InternalEvent.LOGIN_FAILED] = (a, b) => LoginFailed((Response)a),
                [Request.MESSAGE_SEND] = (a, b) => MessageReceived((Message)a),
                [Request.MESSAGE_UPDATE] = (a, b) => MessageUpdated((MessageUpdate)a),
                [Event.GROUP_UPDATE] = (a, b) => GroupUpdated((Group)a),
                [Event.GROUP_AUDIO_CONFIGURATION_UPDATE] = (a, b) => GroupAudioConfigurationUpdated((Group)a, (GroupAudioConfiguration)b),
                [Event.GROUP_AUDIO_COUNT_UPDATE] = (a, b) => GroupAudioCountUpdated((Group)a, (GroupAudioCount)b),
                [InternalEvent.READY] = (a, b) => Ready(),
                [InternalEvent.JOINED_GROUP] = (a, b) => JoinedGroup((Group)a),
                [InternalEvent.LEFT_GROUP] = (a, b) => LeftGroup((Group)a),
                [Event.GROUP_MEMBER_ADD] = (a, b) => UserJoined((Group)a, (Subscriber)b),
                [Event.GROUP_MEMBER_DELETE] = (a, b) => UserLeft((Group)a, (Subscriber)b),
                [Event.GROUP_MEMBER_UPDATE] = (a, b) => GroupMemberUpdated((Group)a, (GroupAction)b),
                [Event.SUBSCRIBER_UPDATE] = (a, b) => UserUpdated((Subscriber)a),
                [InternalEvent.LOG] = (a, b) => Log((string)a),
                [InternalEvent.PACKET_RECEIVED] = (a, b) => PacketReceived((string)a, b),
                [InternalEvent.PACKET_SENT] = (a, b) => PacketSent((string)a, b),
                [InternalEvent.PERMISSIONS_FAILED] = (a, b) => PermissionFailed((FailedPermission)a),
                [Event.PRESENCE_UPDATE] = (a, b) => PresenceUpdate((Subscriber)a, (PresenceUpdate)b),
                [Request.SUBSCRIBER_CONTACT_ADD] = (a, b) => ContactAdded((Subscriber)a),
                [Request.SUBSCRIBER_CONTACT_DELETE] = (a, b) => ContactRemoved((Subscriber)a),
                [Request.SUBSCRIBER_BLOCK_ADD] = (a, b) => UserBlocked((Subscriber)a),
                [Request.SUBSCRIBER_BLOCK_DELETE] = (a, b) => UserUnblocked((Subscriber)a),
                [Request.TIP_ADD] = (a, b) => TipAdded((Tip)a),
                [InternalEvent.PING]= (a, b) => { },
            };
        }

        private void EmitEvent(string name, [Optional] object arg1, [Optional] object arg2)
        {
            try
            {
                if (_events.ContainsKey(name))
                    _events[name](arg1, arg2);
                else
                    _events[InternalEvent.LOG]($"Invalid event {name}", null);
            }
            catch (Exception d)
            {
                _events[InternalEvent.INTERNAL_ERROR]($"Event \"{name}\" error ", d.ToString());
            }
        }

        /// <summary>
        /// Trigger an On event by name with two arguements
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="eventString"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public void Emit<T1, T2>(string eventString, T1 arg1, T2 arg2)
        {
            EmitEvent(eventString, arg1, arg2);
        }

        /// <summary>
        /// Trigger an On event by name with a single argument
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="eventString"></param>
        /// <param name="arg1"></param>
        public void Emit<T1>(string eventString, T1 arg1)
        {
            EmitEvent(eventString, arg1);
        }

        /// <summary>
        /// Trigger an On event by string name only
        /// </summary>
        /// <param name="eventString"></param>
        public void Emit(string eventString)
        {
            EmitEvent(eventString);
        }

        private Dictionary<string, IEvent> events = new Dictionary<string, IEvent>();

        internal void RegisterEvents(WolfBot bot)
        {
            var types = (from x in Assembly.GetExecutingAssembly().GetTypes() from z in x.GetInterfaces() let y = x.BaseType where (y != null && y.IsGenericType && typeof(IEvent).IsAssignableFrom(y.GetGenericTypeDefinition())) || (z.IsGenericType && typeof(IEvent).IsAssignableFrom(z.GetGenericTypeDefinition())) select x).Where(r => !r.IsAbstract).ToList();

            foreach (var eventHandler in types)
            {
                IEvent @event = Activator.CreateInstance(eventHandler) as IEvent;

                @event.Bot = bot;
                @event.Client = bot.WolfClient;

                @event.Register();

                events.Add(@event.Command, @event);

                bot.On.Emit(InternalEvent.LOG, $"Registered server event {@event.Command}");
            }
        }

        internal void UnregisterEvents(WolfBot bot)
        {
            if (events.Count == 0)
                return;

            foreach (var @event in events)
            {
                bot.WolfClient.Socket.Off(@event.Key);
                events.Remove(@event.Key);

                bot.On.Emit(InternalEvent.LOG, $"Unregistered server event {@event.Key}");
            }
        }
    }
}