using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Notifications;
using WOLF.Net.Enums.Misc;
using WOLF.Net.Networking;

namespace WOLF.Net.Helper
{
    public class NotificationHelper : BaseHelper<Notification>
    {
        public async Task<Response<List<Notification>>> GetAsync(Language language = Language.ENGLISH) => await Bot._webSocket.Emit<Response<List<Notification>>>(Request.NOTIFICATION_LIST, new
        {
            language = (int)language,
            deviceType = Bot.LoginSettings.LoginDeviceId
        });

        public async Task<Response> ClearAsync() => await Bot._webSocket.Emit<Response>(Request.NOTIFICATION_LIST_CLEAR);

        internal NotificationHelper(WolfBot bot, WebSocket webSocket) : base(bot, webSocket) { }
    }
}