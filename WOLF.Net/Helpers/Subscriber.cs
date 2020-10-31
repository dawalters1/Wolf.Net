using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WOLF.Net.Helpers
{
    public class Subscriber
    {
        private WolfBot Bot;

        public Subscriber(WolfBot bot)
        {
            Bot = bot;
        }

        public async Task<Entities.Subscriber.Subscriber> GetById(int subscriberId)
        {
            return new Entities.Subscriber.Subscriber();
        }
    }
}
