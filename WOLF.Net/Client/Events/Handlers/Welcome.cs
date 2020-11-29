using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;

namespace WOLF.Net.Client.Events.Handlers
{
    public class Welcome : Event<Entities.API.Welcome>
    {
        public override string Command => Event.WELCOME;

        public override async void HandleAsync(Entities.API.Welcome data)
        {
            if (data.LoggedInUser == null)
            {
                var result = await Bot.InternalLoginAsync();

                if (!result.Success)
                {
                    Bot.On.Emit(InternalEvent.LOGIN_FAILED, new Response()
                    {
                        Code = result.Code,
                        Headers = result.Headers
                    });
                    return;
                }

                Bot.CurrentSubscriber = result.Body.Subscriber;

                Bot.On.Emit(InternalEvent.LOGIN, Bot.CurrentSubscriber);
            }
            else
            {
                Bot.Groups.Clear();
                Bot.Subscribers.Clear();
                Bot.Stages.Clear();
                Bot.Achievements.Clear();
                Bot.Charms.Clear();
                Bot.Contacts.Clear();

                Bot.CurrentSubscriber = data.LoggedInUser;
            }
            await OnLoginSuccess();
        }

        private async Task OnLoginSuccess()
        {
            try
            {
                await Bot.GetJoinedGroupsAsync(true);

                Bot.CurrentSubscriber = await Bot.GetSubscriberAsync(Bot.CurrentSubscriber.Id, true);

                await Bot.GroupMessageSubscribeAsync();

                await Bot.PrivateMessageSubscribeAsync();

                await Bot.GroupTipSubscribeAsync();

                Bot.On.Emit(InternalEvent.READY);
            }
            catch (Exception d)
            {
                Bot.On.Emit(InternalEvent.INTERNAL_ERROR, d.ToString());
            }
        }

        public override void Register()
        {
            Client.On<Entities.API.Welcome>(Command, resp => HandleAsync(resp));
        }
    }
}