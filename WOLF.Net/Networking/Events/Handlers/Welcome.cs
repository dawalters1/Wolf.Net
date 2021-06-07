﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Entities.Misc;

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
                            type = Bot.LoginSettings.LoginType.ToString().ToLowerInvariant(),
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

                    Bot.Cognito = result.Body.Cognito;

                    Bot.CurrentSubscriber = result.Body.Subscriber;

                    Bot.On.Emit(Internal.LOGIN, Bot.CurrentSubscriber);
                }
                else
                {

                    Bot._cleanUp();

                    Bot.CurrentSubscriber = data.LoggedInUser;
                }

                Bot.Endpoints = data.EndpointConfig;

                await OnLoginSuccess(data.LoggedInUser != null);
            }
            catch (Exception d)
            {
                Bot._eventHandler.Emit(Internal.ERROR, d.ToString());
            }
        }

        private async Task OnLoginSuccess(bool reconnect = false)
        {
            try
            {
                await Bot.Group().ListAsync(true, true);

                Bot.CurrentSubscriber = await Bot.Subscriber().GetByIdAsync(Bot.CurrentSubscriber.Id, true);

                await Bot.Messaging().GroupSubscribeAsync();

                await Bot.Messaging().PrivateSubscribeAsync();

                await Bot.Tip().GroupSubscribeAsync();

                Bot.On.Emit(!reconnect?Internal.READY:Internal.RECONNECTED);
            }
            catch (Exception d)
            {
                Bot.On.Emit(Internal.ERROR, d.ToString());
            }
        }
    }
}
