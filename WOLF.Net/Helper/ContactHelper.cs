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
    public class ContactHelper : BaseHelper<Entities.Groups.Subscriber>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of contacts</returns>
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

        /// <summary>
        /// Check to see if a subscriber is a contact
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public async Task<bool> IsContactAsync(int id) => (await ListAsync()).Any((blocked) => blocked.Id == id);

        /// <summary>
        /// Add a subscriber as a contact
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns>Response</returns>
        public async Task<Response> AddAsync(int subscriberId) => await WebSocket.Emit<Response>(Request.SUBSCRIBER_CONTACT_ADD, new { id = subscriberId });

        /// <summary>
        /// Remove a subscriber as a contact
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns></returns>
        public async Task<Response> DeleteAsync(int subscriberId) => await WebSocket.Emit<Response>(Request.SUBSCRIBER_CONTACT_DELETE, new { id = subscriberId });

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

        internal ContactHelper(WolfBot bot, WebSocket WebSocket) : base(bot, WebSocket) { }
    }
}
