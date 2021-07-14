using System;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;

namespace WOLF.Net.Networking.Events.Handlers
{
    public class Welcome : BaseEvent<Entities.Misc.Welcome>
    {
        public override string Command => Event.WELCOME;

        public override bool ReturnBody => false;

        public async Task Login()
        {
            var result = await Bot._webSocket.Emit<Response<LoginResponse>>(Request.SECURITY_LOGIN, new
            {
                headers = new
                {
                    version = 2
                },
                body = new
                {
                    type = Bot.LoginSettings.LoginType.ToString().ToLowerInvariant(),
                    deviceTypeId = (int)Bot.LoginSettings.LoginDevice,
                    username = Bot.LoginSettings.Email,
                    password = Bot.LoginSettings.LoginType == Enums.API.LoginType.EMAIL ? Bot.LoginSettings.Password.ToMD5() : Bot.LoginSettings.Password,
                    md5Password = Bot.LoginSettings.LoginType == Enums.API.LoginType.EMAIL
                }
            });

            if (!result.Success)
            {
                Bot.On.Emit(Internal.LOGIN_FAILED, new Response()
                {
                    Code = result.Code,
                    Headers = result.Headers
                });

                if (result.Headers != null && result.Headers.ContainsKey("subCode") && int.Parse(result.Headers["subCode"]) > 1)
                {
                    await Task.Delay(90000);// Attempt to reconnect after 30 seconds regardless of expiry given
                    await Login();
                }
                return;
            }

            Bot.LoginSettings.Cognito = result.Body.Cognito;

            Bot.CurrentSubscriber = result.Body.Subscriber;

            Bot.On.Emit(Internal.LOGIN, Bot.CurrentSubscriber);

            await OnLoginSuccess(false);
        }
        public override async void Handle(Entities.Misc.Welcome data)
        {
            try
            {
                if (data.LoggedInUser == null)
                    await Login();
                else
                {
                    Bot._cleanUp();

                    Bot.CurrentSubscriber = data.LoggedInUser;

                    await OnLoginSuccess(true);
                }
            }
            catch (Exception d)
            {
                Bot.On.Emit(Internal.ERROR, d.ToString());
            }
        }

        private async Task OnLoginSuccess(bool reconnect = false)
        {
            try
            {
                await Bot.GetJoinedGroupsAsync(true, true);

                Bot.CurrentSubscriber = await Bot.GetSubscriberAsync(Bot.CurrentSubscriber.Id, true);

                await Bot.GroupSubscribeAsync();

                await Bot.PrivateSubscribeAsync();

                await Bot.GroupTipSubscribeAsync();

                Bot.On.Emit(!reconnect ? Internal.READY : Internal.RECONNECTED);
            }
            catch (Exception d)
            {
                Bot.On.Emit(Internal.ERROR, d.ToString());
            }
        }
    }
}