using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Client.Events
{
    public abstract class Event<T> : IEvent
    {
        public abstract string Command { get; }

        public WolfBot Bot { get; set; }

        public WolfClient Client { get; set; }

        public abstract void Register();

        public abstract void HandleAsync(T data);
    }
}
