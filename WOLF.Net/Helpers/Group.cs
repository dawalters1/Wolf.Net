using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WOLF.Net.Helpers
{
    public class Group
    {
        private WolfBot Bot;

        public Group(WolfBot bot)
        {
            Bot = bot;
        }

        public async Task<Entities.Group.Group> GetById(int groupId)
        {
            return new Entities.Group.Group();
        }

        public async Task<List<int>> GetSubscriberList(int groupId)
        {
            return new List<int>();
        }
    }
}
