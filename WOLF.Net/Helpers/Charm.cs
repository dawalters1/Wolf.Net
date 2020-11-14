using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Charms;
using WOLF.Net.Enums.Misc;

namespace WOLF.Net
{
    //TO-DO:
    /*
     * Get By ID
     * Get All
     * Get charm statistics
     * Get Expired charms
     * Get Active charms
     * Set selected charms
     * Delete Charms
     */
    public partial class WolfBot
    {
        public List<Charm> Charms = new List<Charm>();

        public async Task<Response<Charm>> GetCharmAsync(int id, bool requestNew = false)
        {
            var charms = await GetCharmsAsync(requestNew: requestNew);

            var charm = charms.FirstOrDefault(r => r.Id == id);

            if (charm != null)
                return new Response<Charm>() { Code = 200, Body = charm, Headers = new Dictionary<string, string>() };

            return new Response<Charm>() { Code = 404, Body = default, Headers = new Dictionary<string, string>() };

        }
        public async Task<List<Charm>> GetCharmsAsync(Language language = Language.English, bool requestNew = false)
        {
            if (!requestNew && Charms.Count > 0)
                return Charms;

            Charms.Clear();

            var result = await WolfClient.Emit<List<Charm>>(Request.CHARM_LIST, new
            {
                language = (int)language
            });

            if (result.Success)
            {
                Charms = result.Body;

                return Charms;
            }

            return new List<Charm>();
        }

        public async Task<Dictionary<int, Response<Charm>>> GetCharmByIdsAsync(List<int> ids, bool requestNew = false)
        {
            Dictionary<int, Response<Charm>> charms = new Dictionary<int, Response<Charm>>();

            var charmsList = await GetCharmsAsync(requestNew: requestNew);

            foreach (var id in ids.Distinct())
            {
                var charm = charmsList.FirstOrDefault(r => r.Id == id);
                if (charm != null)
                    charms.Add(id, new Response<Charm>() { Code = 200, Body = charm, Headers = new Dictionary<string, string>() });
                else
                    charms.Add(id, new Response<Charm>() { Code = 404, Body = default, Headers = new Dictionary<string, string>() });
            }

            return charms;
        }

        public async Task<Response<CharmStatistics>> GetCharmStatisticsAsync(int subscriberId)
        {
            return await WolfClient.Emit<CharmStatistics>(Request.CHARM_SUBSCRIBER_STATISTICS, new
            {
                id = subscriberId
            });
        }

        public async Task<Response<List<SubscriberCharm>>> GetActiveCharmsAsync(int subscriberId, int offset = 0, int limit = 25)
        {
            return await WolfClient.Emit<List<SubscriberCharm>>(Request.CHARM_SUBSCRIBER_ACTIVE_LIST, new
            {
                id = subscriberId,
                offset,
                limit
            });
        }

        public async Task<Response<List<SubscriberCharm>>> GetExpiredCharmsAsync(int subscriberId, int offset = 0, int limit = 25)
        {
            return await WolfClient.Emit<List<SubscriberCharm>>(Request.CHARM_SUBSCRIBER_EXPIRED_LIST, new
            {
                id = subscriberId,
                offset,
                limit
            });
        }

        public async Task<Response> SetCharmsAsync(params SelectedCharm[] charms)
        {
            return await WolfClient.Emit(Request.CHARM_SUBSCRIBER_SET_SELECTED, new
            {
                selectedList = charms
            });
        }

        public async Task<Response> DeleteCharmsAsync(params int[] ids)
        {
            return await WolfClient.Emit(Request.CHARM_SUBSCRIBER_DELETE, new
            {
                idList = ids
            });
        }
    }
}
