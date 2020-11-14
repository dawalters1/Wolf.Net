using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Notifications;
using WOLF.Net.Enums.Misc;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        public async Task<Response<List<Notification>>> GetNotificationsListAsync(Language language = Language.English)
        {
            return await WolfClient.Emit<List<Notification>>(Request.NOTIFICATION_LIST, new
            {
                language = (int)language,
                deviceType = LoginData.LoginDeviceId
            });
        }

        public async Task<Response> ClearNotificationsAsync()
        {
            return await WolfClient.Emit(Request.NOTIFICATION_LIST_CLEAR, new { });
        }
    }
}
