using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Charms;
using WOLF.Net.Entities.Misc;
using WOLF.Net.Enums.Misc;
using WOLF.Net.Enums.Subscribers;

namespace WOLF.Net
{

    public partial class WolfBot
    {

        /// <summary>
        /// Update the bots profile
        /// </summary>
        /// <returns>Subscriber profile builder</returns>
        public Builders.Profiles.Subscriber UpdateProfile() => new Builders.Profiles.Subscriber(this, this.CurrentSubscriber);

        /// <summary>
        /// Set the online state for the bot
        /// </summary>
        /// <param name="onlineState"></param>
        /// <returns></returns>
        public async Task<Response> SetOnlineStateAsync(OnlineState onlineState) => await _webSocket.Emit<Response>(Request.SUBSCRIBER_SETTINGS_UPDATE, new { state = new { state = (int)onlineState } /* State inside state? wtf is this shit...*/ });

        /// <summary>
        /// Set charms for the bots profile
        /// </summary>
        /// <param name="charms"></param>
        /// <returns></returns>
        public async Task<Response> SetCharmsAsync(params SelectedCharm[] charms) => await _webSocket.Emit<Response>(Request.CHARM_SUBSCRIBER_SET_SELECTED, new
        {
            selectedList = charms
        });

        /// <summary>
        /// Delete charms from the bots account
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<Response> DeleteCharmsAsync(params int[] ids) => await _webSocket.Emit<Response>(Request.CHARM_SUBSCRIBER_DELETE, new
        {
            idList = ids
        });

        /// <summary>
        /// Get bot message settings
        /// </summary>
        /// <returns></returns>
        public async Task<Response<MessageSetting>> GetMessageSettingsAsync() => await _webSocket.Emit<Response<MessageSetting>>(Request.MESSAGE_SETTING);

        /// <summary>
        /// Update bot message settings
        /// </summary>
        /// <param name="messageFilterType"></param>
        /// <returns></returns>
        public async Task<Response> UpdateMessageSettingsAsync(MessageFilterType messageFilterType) => await _webSocket.Emit<Response>(Request.MESSAGE_SETTING_UPDATE, new
        {
            spamFilter = new
            {
                enabled = messageFilterType != MessageFilterType.OFF,
                tier = (int)messageFilterType
            },
        });
    }
}