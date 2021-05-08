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

        public override async void Handle(Entities.Misc.Welcome data)
        {
            try
            {
                if (data.LoggedInUser == null)
                {
                    var result = await Bot._webSocket.Emit<Response<LoginResponse>>(Request.SECURITY_LOGIN, new
                    {
                        headers = new
                        {
                            version = 2
                        },
                        body = new
                        {
                            type = Bot.LoginSettings.LoginType.ToString().ToLower(),
                            deviceTypeId = (int)Bot.LoginSettings.LoginDevice,
                            username = Bot.LoginSettings.Email,
                            password = Bot.LoginSettings.Password.ToMD5(),
                            md5Password = true
                        }
                    });

                    if (!result.Success)
                    {
                        Bot.On.Emit(Internal.LOGIN_FAILED, new Response()
                        {
                            Code = result.Code,
                            Headers = result.Headers
                        });
                        return;
                    }

                    Bot.LoginSettings.Cognito = result.Body.Cognito;

                    Bot.CurrentSubscriber = result.Body.Subscriber;

                    Bot.On.Emit(Internal.LOGIN, Bot.CurrentSubscriber);
                }
                else
                {

                    Bot._cleanUp();

                    Bot.CurrentSubscriber = data.LoggedInUser;
                }
                await OnLoginSuccess(data.LoggedInUser != null);
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