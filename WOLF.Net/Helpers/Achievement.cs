using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Entities.Achievements;

namespace WOLF.Net
{
    //TO-DO:
    /*
     * Get By Id
     * Get All
     * Get subscriber achievements
     */
    public partial class WolfBot
    {
        public async Task<List<Achievement>> GetAchievements()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Achievement>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Achievement>> GetByIds(params int[] ids)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SubscriberAchievement>> GetSubscriberAchievements(int subscriberId)
        {
            throw new NotImplementedException();
        }
    }
}
