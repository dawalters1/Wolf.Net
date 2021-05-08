using System.Collections.Generic;
using WOLF.Net.Networking;

namespace WOLF.Net.Helpers
{
    public abstract class BaseHelper<T>
    {
        internal WebSocket WebSocket;

        internal List<T> cache = new List<T>();

        public BaseHelper(WebSocket webSocket)
        {
            WebSocket = webSocket;
        }
    }
}
