using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Networking;

namespace WOLF.Net.Helper
{
    public abstract class BaseHelper<T>
    {
        internal WebSocket WebSocket;

        internal WolfBot Bot;

        internal List<T> cache = new List<T>();

        public BaseHelper(WolfBot bot, WebSocket webSocket)
        {
            Bot = bot;
            WebSocket = webSocket;
        }
    }
}
