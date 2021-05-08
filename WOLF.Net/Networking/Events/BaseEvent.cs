using WOLF.Net.Entities.API;

namespace WOLF.Net.Networking.Events
{
    public abstract class BaseEvent<T> : IBaseEvent
    {
        public abstract string Command { get; }

        public WolfBot Bot { get; set; }

        public WebSocket Client { get; set; }

        public abstract bool ReturnBody { get;  } 

        public virtual void Register()
        {
            if (ReturnBody)
                Client.On<Response<T>>(Command, callback => Handle(callback.Body));
            else
                Client.On<T>(Command, callback =>Handle(callback));
        }

        public abstract void Handle(T data);
    }
}