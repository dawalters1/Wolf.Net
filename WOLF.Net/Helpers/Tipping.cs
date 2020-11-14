using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Messages;
using WOLF.Net.Entities.Messages.Tipping;
using WOLF.Net.Entities.Tipping.Leaderboards;
using WOLF.Net.Enums.Messages;
using WOLF.Net.Enums.Tipping;

namespace WOLF.Net
{
    //TO-DO:
    /*
     * Tip Subscription
     */
    public partial class WolfBot
    {
        internal async Task<Response> GroupTipSubscribeAsync()
        {
            return await WolfClient.Emit(Request.TIP_GROUP_SUBSCRIBE, new { });
        }


        internal async Task<Response> PrivateTipSubscribeAsync()
        {
            return await WolfClient.Emit(Request.TIP_PRIVATE_SUBSCRIBE, new { });
        }

        public async Task<Response> AddTip(int userId, int groupId, long timestamp, ContextType contextType, List<TipCharm> charmList)
        {
            return await WolfClient.Emit(Request.TIP_ADD, new
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
        }

        public async Task<Response> AddTip(Message message, List<TipCharm> charmList)
        {
            if (message.MessageType != MessageType.Group)
                return new Response() { Code = 400, Headers = new Dictionary<string, string>() { { "error", "You currently cannot tip in pm" } } };

            return await AddTip(message.UserId, message.ReturnAddress, message.Timestamp, ContextType.Message, charmList);
        }

        public async Task<Response<TipDetail>> GetTipDetails(int groupId, long timestamp, ContextType contextType, int limit = 20, int offset = 0)
        {
            return await WolfClient.Emit<TipDetail>(Request.TIP_DETAIL, new
            {
                groupId,
                id = timestamp,
                contextType = contextType.ToString().ToLower(),
                limit,
                offset
            });
        }

        public async Task<Response<TipDetail>> GetTipDetails(Message message, int limit = 20, int offset = 0)
        {
            if (message.MessageType != MessageType.Group)
                return new Response<TipDetail>() { Code = 400, Body = default, Headers = new Dictionary<string, string>() { { "error", "You currently cannot tip in pm" } } };

            return await GetTipDetails(message.ReturnAddress, message.Timestamp, ContextType.Message, limit, offset);
        }

        public async Task<Response<TipSummary>> GetTipSummary(int groupId, long timestamp, ContextType contextType, int limit = 20, int offset = 0)
        {
            return await WolfClient.Emit<TipSummary>(Request.TIP_SUMMARY, new
            {
                groupId,
                id = timestamp,
                contextType = contextType.ToString().ToLower(),
                limit,
                offset
            });
        }

        public async Task<Response<TipSummary>> GetTipSummary(Message message, int limit = 20, int offset = 0)
        {
            if (message.MessageType != MessageType.Group)
                return new Response<TipSummary>() { Code = 400, Body = default, Headers = new Dictionary<string, string>() { { "error", "You currently cannot tip in pm" } } };

            return await GetTipSummary(message.ReturnAddress, message.Timestamp, ContextType.Message, limit, offset);
        }

        public async Task<Response<TipLeaderboardSummary>> GetGroupLeaderboardSummary(int groupId, TipPeriod tipPeriod = TipPeriod.Day, TipType tipType = TipType.Subscriber, TipDirection tipDirection = TipDirection.Sent)
        {
            return tipType == TipType.Group
                ? new Response<TipLeaderboardSummary>() { Code = 400, Body = default, Headers = new Dictionary<string, string>() { { "error", "group tipType is for use in global leaderboard only" } } }
                : await WolfClient.Emit<TipLeaderboardSummary>(Request.TIP_LEADERBOARD_GROUP_SUMMARY, new
                {
                    id = groupId,
                    period = tipPeriod.ToString().ToLower(),
                    type = tipType.ToString().ToLower(),
                    tipDirection = tipType == TipType.Charm ? null : tipDirection.ToString().ToLower()
                });
        }

        public async Task<Response<TipLeaderboard>> GetGroupLeaderboard(int groupId, TipPeriod tipPeriod = TipPeriod.Day, TipType tipType = TipType.Subscriber, TipDirection tipDirection = TipDirection.Sent)
        {
            return tipType == TipType.Group
                ? new Response<TipLeaderboard>() { Code = 400, Body = default, Headers = new Dictionary<string, string>() { { "error", "group tipType is for use in global leaderboard only" } } }
                : await WolfClient.Emit<TipLeaderboard>(Request.TIP_LEADERBOARD_GROUP, new
                {
                    groupId,
                    period = tipPeriod.ToString().ToLower(),
                    type = tipType.ToString().ToLower(),
                    tipDirection = tipType == TipType.Charm ? null : tipDirection.ToString().ToLower()
                });
        }

        public async Task<Response<TipLeaderboardSummary>> GetGlobalLeaderboardSummary(TipPeriod tipPeriod = TipPeriod.Day, TipType tipType = TipType.Subscriber, TipDirection tipDirection = TipDirection.Sent)
        {
            return await WolfClient.Emit<TipLeaderboardSummary>(Request.TIP_LEADERBOARD_GLOBAL_SUMMARY, new
            {
                period = tipPeriod.ToString().ToLower(),
                type = tipType.ToString().ToLower(),
                tipDirection = tipType == TipType.Charm || tipType == TipType.Group ? null : tipDirection.ToString().ToLower()
            });
        }


        public async Task<Response<TipLeaderboard>> GetGlobalLeaderboard(TipPeriod tipPeriod = TipPeriod.Day, TipType tipType = TipType.Subscriber, TipDirection tipDirection = TipDirection.Sent)
        {
            return await WolfClient.Emit<TipLeaderboard>(Request.TIP_LEADERBOARD_GLOBAL, new
            {
                period = tipPeriod.ToString().ToLower(),
                type = tipType.ToString().ToLower(),
                tipDirection = tipType == TipType.Charm || tipType == TipType.Group ? null : tipDirection.ToString().ToLower()
            });
        }
    }
}