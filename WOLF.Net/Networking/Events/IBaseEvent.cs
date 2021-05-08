namespace WOLF.Net.Networking.Events
{
    public interface IBaseEvent
    {
        WolfBot Bot { get; set; }

        WebSocket Client { get; set; }

        string Command { get; }

        void Register();
    }
}
