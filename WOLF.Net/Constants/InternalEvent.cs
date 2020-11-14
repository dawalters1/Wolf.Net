using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Constants
{
    internal class InternalEvent
    {
        public const string CONNCETED = "connected";

        public const string DISCONNECTED = "disconnected";

        public const string RECONNECTING = "reconnecting";

        public const string CONNECTION_ERROR = "connection error";

        public const string PING = "ping";

        public const string PONG = "pong";

        public const string PACKET_SENT = "packet sent";

        public const string PACKET_RECEIVED = "packet received";

        public const string LOGIN = "login";

        public const string LOGIN_FAILED = "login failed";

        public const string LOG = "log";

        public const string INTERNAL_ERROR = "internal error";

        public const string JOINED_GROUP = "joined group";

        public const string LEFT_GROUP = "left group";

        public const string READY = "ready";

        public const string PERMISSIONS_FAILED = "permissions failed";

        public const string PRIVATE_MESSAGE_ACCEPT_RESPONSE = "private message accept response";
    }
}
