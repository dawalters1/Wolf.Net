using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.RTimer
{
    public abstract class Timeout<T> : ITimeout
    {
        public abstract string HandlerName { get; }

        public RTimer RTimer { get; set; }

        public abstract void OnTimeout(object data);
    }
}