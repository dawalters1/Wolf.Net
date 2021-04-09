using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Entities.Tip;
using WOLF.Net.Entities.Tip.Leaderboards;
using WOLF.Net.Enums.Tip;
using WOLF.Net.Networking;

namespace WOLF.Net.Helper
{
    public class TipHelper : BaseHelper<Tip>
    {
        internal async Task<Response> GroupTipSubscribeAsync() => await WebSocket.Emit<Response>(Request.TIP_GROUP_SUBSCRIBE);

        internal async Task<Response> PrivateTipSubscribeAsync() => await WebSocket.Emit<Response>(Request.TIP_PRIVATE_SUBSCRIBE);

        public async Task<Response> AddTip(int userId, int groupId, long timestamp, ContextType contextType, List<TipCharm> charmList) => await WebSocket.Emit<Response>(Request.TIP_ADD, new
        {
            subscriberId = userId,
            groupId,
            charmList,
            context = new
            {
                type = contextType.ToString().ToLower(),
                id = timestamp
            }
        });

        public async Task<Response> AddTip(Message message, params TipCharm[] charms) => await AddTip(message.SourceSubscriberId, message.TargetGroupId, message.Timestamp, ContextType.MESSAGE, charms.ToList());

        public async Task<Response<TipDetail>> GetTipDetails(int groupId, long timestamp, ContextType contextType, int limit = 20, int offset = 0) => await WebSocket.Emit<Response<TipDetail>>(Request.TIP_DETAIL, new
        {
            groupId,
            id = timestamp,
            contextType = contextType.ToString().ToLower(),
            limit,
            offset
        });


        public async Task<Response<TipDetail>> GetTipDetails(Message message, int limit = 20, int offset = 0) => await GetTipDetails(message.TargetGroupId, message.Timestamp, ContextType.MESSAGE, limit, offset);

        public async Task<Response<TipSummary>> GetTipSummary(int groupId, long timestamp, ContextType contextType, int limit = 20, int offset = 0) => await WebSocket.Emit<Response<TipSummary>>(Request.TIP_SUMMARY, new
        {
            groupId,
            id = timestamp,
            contextType = contextType.ToString().ToLower(),
            limit,
            offset
        });

        public async Task<Response<TipSummary>> GetTipSummary(Message message, int limit = 20, int offset = 0) => await GetTipSummary(message.TargetGroupId, message.Timestamp, ContextType.MESSAGE, limit, offset);

        public async Task<Response<TipLeaderboardSummary>> GetGroupLeaderboardSummary(int groupId, TipPeriod tipPeriod = TipPeriod.DAY, TipType tipType = TipType.SUBSCRIBER, TipDirection tipDirection = TipDirection.SENT) => await WebSocket.Emit<Response<TipLeaderboardSummary>>(Request.TIP_LEADERBOARD_GROUP_SUMMARY, new
        {
            id = groupId,
            period = tipPeriod.ToString().ToLower(),
            type = tipType.ToString().ToLower(),
            tipDirection = tipType == TipType.CHARM ? null : tipDirection.ToString().ToLower()
        });


        public async Task<Response<TipLeaderboard>> GetGroupLeaderboard(int groupId, TipPeriod tipPeriod = TipPeriod.DAY, TipType tipType = TipType.SUBSCRIBER, TipDirection tipDirection = TipDirection.SENT) => await WebSocket.Emit<Response<TipLeaderboard>>(Request.TIP_LEADERBOARD_GROUP, new
        {
            groupId,
            period = tipPeriod.ToString().ToLower(),
            type = tipType.ToString().ToLower(),
            tipDirection = tipType == TipType.CHARM ? null : tipDirection.ToString().ToLower()
        });

        public async Task<Response<TipLeaderboardSummary>> GetGlobalLeaderboardSummary(TipPeriod tipPeriod = TipPeriod.DAY, TipType tipType = TipType.SUBSCRIBER, TipDirection tipDirection = TipDirection.SENT) => await WebSocket.Emit<Response<TipLeaderboardSummary>>(Request.TIP_LEADERBOARD_GLOBAL_SUMMARY, new
        {
            period = tipPeriod.ToString().ToLower(),
            type = tipType.ToString().ToLower(),
            tipDirection = tipType == TipType.CHARM || tipType == TipType.GROUP ? null : tipDirection.ToString().ToLower()
        });

        public async Task<Response<TipLeaderboard>> GetGlobalLeaderboard(TipPeriod tipPeriod = TipPeriod.DAY, TipType tipType = TipType.SUBSCRIBER, TipDirection tipDirection = TipDirection.SENT) => await WebSocket.Emit<Response<TipLeaderboard>>(Request.TIP_LEADERBOARD_GLOBAL, new
        {
            period = tipPeriod.ToString().ToLower(),
            type = tipType.ToString().ToLower(),
            tipDirection = tipType == TipType.CHARM || tipType == TipType.GROUP ? null : tipDirection.ToString().ToLower()
        });


        internal TipHelper(WolfBot bot, WebSocket WebSocket) : base(bot, WebSocket) { }
    }
}