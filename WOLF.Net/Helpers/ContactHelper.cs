using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        internal List<Entities.Groups.Subscriber> contactCache = new List<Entities.Groups.Subscriber>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of contacts</returns>
        public async Task<IReadOnlyList<Entities.Groups.Subscriber>> ContactList()
        {
            if (this.contactCache.Count == 0)
            {
                var result = await _webSocket.Emit<Response<List<Entities.Groups.Subscriber>>>(Request.SUBSCRIBER_CONTACT_LIST);

                if (result.Success)
                {
                    contactCache = result.Body;
                }
            }
            return contactCache;
        }

        /// <summary>
        /// Check to see if a subscriber is a contact
        /// </summary>
        /// <param name="id"></param>
        /// <returns>bool</returns>
        public async Task<bool> IsContactAsync(int id) => (await ContactList()).Any((blocked) => blocked.Id == id);

        /// <summary>
        /// Add a subscriber as a contact
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns>Response</returns>
        public async Task<Response> AddSubscriberAsync(int subscriberId) => await _webSocket.Emit<Response>(Request.SUBSCRIBER_CONTACT_ADD, new { id = subscriberId });

        /// <summary>
        /// Remove a subscriber as a contact
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <returns></returns>
        public async Task<Response> DeleteSubscriberAsync(int subscriberId) => await _webSocket.Emit<Response>(Request.SUBSCRIBER_CONTACT_DELETE, new { id = subscriberId });

        internal async Task<Entities.Groups.Subscriber> ProcessContact(int id)
        {
            if (contactCache.Any((sub) => sub.Id == id))
            {
                var removed = contactCache.FirstOrDefault((sub) => sub.Id == id);

                contactCache.RemoveAll((sub) => sub.Id == id);

                return removed;
            }
            else
            {
                var subscriber = await GetSubscriberAsync(id);
                contactCache.Add(new Entities.Groups.Subscriber()
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

                return contactCache.FirstOrDefault((sub) => sub.Id == id);
            }
        }
    }
}