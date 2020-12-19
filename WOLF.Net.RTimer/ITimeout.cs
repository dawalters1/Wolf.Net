using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.RTimer
{

    public interface ITimeout
    {
        string HandlerName { get; }

        RTimer RTimer { get; set; }

        void OnTimeout(object data);
    }
}
