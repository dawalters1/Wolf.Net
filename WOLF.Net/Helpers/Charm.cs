using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Charms;

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

        public async Task<Charm> GetCharmAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Charm>> GetCharmsAsync(params int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<CharmStatistics> GetCharmStatisticsAsync(int subscriberId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CharmStatus>> GetActiveCharmsAsync(int subscriberId, int offset = 0, int limit =25)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CharmStatus>> GetExpiredCharmsAsync(int subscriberId, int offset = 0, int limit = 25)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> SetCharmsAsync(params SelectedCharm[] charms)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> DeleteCharmsAsync(params int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}
