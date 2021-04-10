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
        public async Task<List<Charm>> ListAsync(bool requestNew = false)
        {
            if (cache.Count > 0 && !requestNew)
                return cache;

            var result = await WebSocket.Emit<Response<List<Charm>>>(Request.CHARM_LIST);

            if (result.Success)
                cache = result.Body;

            return cache;
        }

        public async Task<Charm> GetByIdAsync(int charmId, bool requestNew = false) => (await GetAllAsync(requestNew)).Where((charm) => charm.Id == charmId).FirstOrDefault();

        public async Task<List<Charm>> GetByIdsAsync(List<int> charmIds, bool requestNew = false) => await GetByIdsAsync(charmIds.ToArray(), requestNew);

        public async Task<List<Charm>> GetByIdsAsync(int[] charmIds, bool requestNew = false) => (await GetAllAsync(requestNew)).Where((charm) => charmIds.Any((id) => id == charm.Id)).ToList();

        internal CharmHelper(WolfBot bot, WebSocket WebSocket) : base(bot, WebSocket) { }
    }
}
