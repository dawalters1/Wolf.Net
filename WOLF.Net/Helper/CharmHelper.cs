using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Charms;
using WOLF.Net.Networking;

namespace WOLF.Net.Helper
{
    public class CharmHelper : BaseHelper<Charm>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestNew"></param>
        /// <returns>List of charms</returns>
        public async Task<List<Charm>> ListAsync(bool requestNew = false)
        {
            if (cache.Count > 0 && !requestNew)
                return cache;

            var result = await WebSocket.Emit<Response<List<Charm>>>(Request.CHARM_LIST);

            if (result.Success)
                cache = result.Body;

            return cache;
        }

        /// <summary>
        /// Request a charm by ID
        /// </summary>
        /// <param name="charmId"></param>
        /// <param name="requestNew"></param>
        /// <returns>Charm (If it exists)</returns>
        public async Task<Charm> GetByIdAsync(int charmId, bool requestNew = false) => (await ListAsync(requestNew)).Where((charm) => charm.Id == charmId).FirstOrDefault();

        /// <summary>
        /// Request a list of charms by ID
        /// </summary>
        /// <param name="charmIds"></param>
        /// <param name="requestNew"></param>
        /// <returns>List of charms (If they exist)</returns>
        public async Task<List<Charm>> GetByIdsAsync(List<int> charmIds, bool requestNew = false) => await GetByIdsAsync(charmIds.ToArray(), requestNew);

        /// <summary>
        /// Request a list of charms by ID
        /// </summary>
        /// <param name="charmIds"></param>
        /// <param name="requestNew"></param>
        /// <returns>List of charms (If they exist)</returns>
        public async Task<List<Charm>> GetByIdsAsync(int[] charmIds, bool requestNew = false) => (await ListAsync(requestNew)).Where((charm) => charmIds.Any((id) => id == charm.Id)).ToList();

        /// <summary>
        /// Request information about a subscribers charms
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns>Total Gifted, Total Sent, Total Active, Total Expired, TotalLifeTime</returns>
        public async Task<Response<CharmStatistics>> GetSubscriberStatisticsAsync(int subscriberId) => await WebSocket.Emit<Response<CharmStatistics>>(Request.CHARM_SUBSCRIBER_STATISTICS, new
        {
            id = subscriberId
        });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns>Response<List<CharmStatus>></returns>
        public async Task<Response<List<CharmStatus>>> GetSubscriberActiveListAsync(int subscriberId, int offset = 0, int limit = 25) => await WebSocket.Emit<Response<List<CharmStatus>>>(Request.CHARM_SUBSCRIBER_ACTIVE_LIST, new
        {
            id = subscriberId,
            offset,
            limit
        });

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns>Response<List<CharmStatus>></returns>
        public async Task<Response<List<CharmStatus>>> GetSubscriberExpiredListAsync(int subscriberId, int offset = 0, int limit = 25) => await WebSocket.Emit<Response<List<CharmStatus>>>(Request.CHARM_SUBSCRIBER_EXPIRED_LIST, new
        {
            id = subscriberId,
            offset,
            limit
        });

        /// <summary>
        /// Get a summary of all charms owned by a subscriber
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns>Response<List<CharmSummary>></returns>
        public async Task<Response<List<CharmSummary>>> GetSubscriberCharmSummaryAsync(int subscriberId) => await WebSocket.Emit<Response<List<CharmSummary>>>(Request.CHARM_SUBSCRIBER_SUMMARY_LIST, new
        {
            id = subscriberId,
        });

        internal CharmHelper(WolfBot bot, WebSocket WebSocket) : base(bot, WebSocket) { }
    }
}