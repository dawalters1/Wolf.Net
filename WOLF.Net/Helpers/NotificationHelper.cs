using System.Collections.Generic;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Notifications;
using WOLF.Net.Enums.Misc;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        /// <summary>
        /// Get a list of notifications
        /// </summary>
        /// <param name="language"></param>
        /// <returns>Response<List<Notification>></returns>
        public async Task<Response<List<Notification>>> GetNotificationListAsync(Language language = Language.ENGLISH) => await _webSocket.Emit<Response<List<Notification>>>(Request.NOTIFICATION_LIST, new
        {
            language = (int)language,
            deviceType = LoginSettings.LoginDeviceId
        });

        /// <summary>
        /// Clear the list of notifications
        /// </summary>
        /// <returns>Response</returns>
        public async Task<Response> ClearNotificationsListAsync() => await _webSocket.Emit<Response>(Request.NOTIFICATION_LIST_CLEAR);
    }
}