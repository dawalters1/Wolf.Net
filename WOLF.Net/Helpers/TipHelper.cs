using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Entities.Tip;
using WOLF.Net.Entities.Tip.Leaderboards;
using WOLF.Net.Enums.Tip;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        internal async Task<Response> GroupTipSubscribeAsync() => await _webSocket.Emit<Response>(Request.TIP_GROUP_SUBSCRIBE);

        internal async Task<Response> PrivateTipSubscribeAsync() => await _webSocket.Emit<Response>(Request.TIP_PRIVATE_SUBSCRIBE);

        /// <summary>
        /// Tip a user a charm or charms
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="groupId"></param>
        /// <param name="timestamp"></param>
        /// <param name="contextType"></param>
        /// <param name="charmList"></param>
        /// <returns>Response</returns>
        public async Task<Response> AddTip(int subscriberId, int groupId, long timestamp, ContextType contextType, List<TipCharm> charmList) => await _webSocket.Emit<Response>(Request.TIP_ADD, new
        {
            subscriberId,
            groupId,
            charmList,
            context = new
            {
                type = contextType.ToString().ToLowerInvariant(),
                id = timestamp
            }
        });

        /// <summary>
        /// Tip a user a charm or charms
        /// </summary>
        /// <param name="message"></param>
        /// <param name="charms"></param>
        /// <returns>Response</returns>
        public async Task<Response> AddTip(Message message, params TipCharm[] charms) => await AddTip(message.SourceSubscriberId, message.TargetGroupId, message.Timestamp, ContextType.MESSAGE, charms.ToList());

        /// <summary>
        /// Get tip details for a message
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="timestamp"></param>
        /// <param name="contextType"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns>Response<TipDetail></returns>
        public async Task<Response<TipDetail>> GetTipDetails(int groupId, long timestamp, ContextType contextType, int limit = 20, int offset = 0) => await _webSocket.Emit<Response<TipDetail>>(Request.TIP_DETAIL, new
        {
            groupId,
            id = timestamp,
            contextType = contextType.ToString().ToLowerInvariant(),
            limit,
            offset
        });

        /// <summary>
        /// Get tip details for a message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns>Response<TipDetail></returns>
        public async Task<Response<TipDetail>> GetTipDetails(Message message, int limit = 20, int offset = 0) => await GetTipDetails(message.TargetGroupId, message.Timestamp, ContextType.MESSAGE, limit, offset);

        /// <summary>
        /// Get tip summary for a message
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="timestamp"></param>
        /// <param name="contextType"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns>Response<TipSummary></returns>
        public async Task<Response<TipSummary>> GetTipSummary(int groupId, long timestamp, ContextType contextType, int limit = 20, int offset = 0) => await _webSocket.Emit<Response<TipSummary>>(Request.TIP_SUMMARY, new
        {
            groupId,
            id = timestamp,
            contextType = contextType.ToString().ToLowerInvariant(),
            limit,
            offset
        });

        /// <summary>
        /// Get tip summary for a message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <returns>Response<TipSummary></returns>
        public async Task<Response<TipSummary>> GetTipSummary(Message message, int limit = 20, int offset = 0) => await GetTipSummary(message.TargetGroupId, message.Timestamp, ContextType.MESSAGE, limit, offset);

        /// <summary>
        /// Get group leaderboard summary
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="tipPeriod"></param>
        /// <param name="tipType"></param>
        /// <param name="tipDirection"></param>
        /// <returns>Response<TipLeaderboardSummary></returns>
        public async Task<Response<TipLeaderboardSummary>> GetGroupLeaderboardSummary(int groupId, TipPeriod tipPeriod = TipPeriod.DAY, TipType tipType = TipType.SUBSCRIBER, TipDirection tipDirection = TipDirection.SENT) => await _webSocket.Emit<Response<TipLeaderboardSummary>>(Request.TIP_LEADERBOARD_GROUP_SUMMARY, new
        {
            id = groupId,
            period = tipPeriod.ToString().ToLowerInvariant(),
            type = tipType.ToString().ToLowerInvariant(),
            tipDirection = tipType == TipType.CHARM ? null : tipDirection.ToString().ToLowerInvariant()
        });

        /// <summary>
        /// Get group leaderboard
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="tipPeriod"></param>
        /// <param name="tipType"></param>
        /// <param name="tipDirection"></param>
        /// <returns>Response<TipLeaderboard></returns>
        public async Task<Response<TipLeaderboard>> GetGroupLeaderboard(int groupId, TipPeriod tipPeriod = TipPeriod.DAY, TipType tipType = TipType.SUBSCRIBER, TipDirection tipDirection = TipDirection.SENT) => await _webSocket.Emit<Response<TipLeaderboard>>(Request.TIP_LEADERBOARD_GROUP, new
        {
            groupId,
            period = tipPeriod.ToString().ToLowerInvariant(),
            type = tipType.ToString().ToLowerInvariant(),
            tipDirection = tipType == TipType.CHARM ? null : tipDirection.ToString().ToLowerInvariant()
        });

        /// <summary>
        /// Get global leaderboard summary
        /// </summary>
        /// <param name="tipPeriod"></param>
        /// <param name="tipType"></param>
        /// <param name="tipDirection"></param>
        /// <returns>Response<TipLeaderboardSummary></returns>
        public async Task<Response<TipLeaderboardSummary>> GetGlobalLeaderboardSummary(TipPeriod tipPeriod = TipPeriod.DAY) => await _webSocket.Emit<Response<TipLeaderboardSummary>>(Request.TIP_LEADERBOARD_GLOBAL_SUMMARY, new
        {
            period = tipPeriod.ToString().ToLowerInvariant(),
        });

        /// <summary>
        /// Get global leaderboard
        /// </summary>
        /// <param name="tipPeriod"></param>
        /// <param name="tipType"></param>
        /// <param name="tipDirection"></param>
        /// <returns>Response<TipLeaderboard></returns>
        public async Task<Response<TipLeaderboard>> GetGlobalLeaderboard(TipPeriod tipPeriod = TipPeriod.DAY, TipType tipType = TipType.SUBSCRIBER, TipDirection tipDirection = TipDirection.SENT) => await _webSocket.Emit<Response<TipLeaderboard>>(Request.TIP_LEADERBOARD_GLOBAL, new
        {
            period = tipPeriod.ToString().ToLowerInvariant(),
            type = tipType.ToString().ToLowerInvariant(),
            tipDirection = tipType == TipType.CHARM || tipType == TipType.GROUP ? null : tipDirection.ToString().ToLowerInvariant()
        });
    }
}