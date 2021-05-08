using WOLF.Net.Constants;

namespace WOLF.Net.Networking.Events.Handlers
{
    public class Tip : BaseEvent<Entities.Tip.Tip>
    {
        public override string Command => Event.TIP_ADD;

        public override bool ReturnBody => true;

        public override void Handle(Entities.Tip.Tip data) => Bot.On.Emit(Request.TIP_ADD, data);
    }
}