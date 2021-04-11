using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Subscribers;
using WOLF.Net.Networking;

namespace WOLF.Net.Helper
{
    public class BlockHelper : BaseHelper<Entities.Groups.Subscriber>
    {
        public async Task<IReadOnlyList<Entities.Groups.Subscriber>> ListAsync()
        {
            if (this.cache.Count == 0)
            {
                var result = await WebSocket.Emit<Response<List<Entities.Groups.Subscriber>>>(Request.SUBSCRIBER_CONTACT_LIST);

                if (result.Success)
                {
                    cache = result.Body;
                }
            }
            return cache;
        }
        public async Task<bool> IsBlockedAsync(int id) => (await ListAsync()).Any((blocked) => blocked.Id == id);

        public async Task<Response> BlockAsync(int subscriberId) => await WebSocket.Emit<Response>(Request.SUBSCRIBER_BLOCK_ADD, new { id = subscriberId });

        public async Task<Response> UnblockAsync(int subscriberId) => await WebSocket.Emit<Response>(Request.SUBSCRIBER_BLOCK_DELETE, new { id = subscriberId });

        internal async Task<Entities.Groups.Subscriber> Process(int id)
        {
            if (cache.Any((sub) => sub.Id == id))
            {
                var removed = cache.FirstOrDefault((sub) => sub.Id == id);

                cache.RemoveAll((sub) => sub.Id == id);

                return removed;
            }
            else
            {
                var subscriber = await Bot.Subscriber().GetByIdAsync(id);
                cache.Add(new Entities.Groups.Subscriber()
                {
                    Id = id,
                    AdditionalInfo = new Entities.Groups.AdditionalInfo()
                    {
                        Hash = subscriber.Hash,
                        Nickname = subscriber.Nickname,
                        OnlineState = subscriber.OnlineState,
                        Privileges = subscriber.Privileges
                    }
                });

                return cache.FirstOrDefault((sub) => sub.Id == id); 
            }
        }

        internal BlockHelper(WolfBot bot, WebSocket WebSocket) : base(bot, WebSocket) { }
    }
}