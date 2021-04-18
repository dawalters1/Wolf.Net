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
    public class SubscriberHelper : BaseHelper<Subscriber>
    {
        /// <summary>
        /// Get a subscriber by Id
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="requestNew"></param>
        /// <returns>Subscriber</returns>
        public async Task<Subscriber> GetByIdAsync(int subscriberId, bool requestNew = false) => (await GetByIdsAsync(new List<int>() { subscriberId }, requestNew)).FirstOrDefault();

        /// <summary>
        /// Get a list of subscribers by Id
        /// </summary>
        /// <param name="subscriberIds"></param>
        /// <param name="requestNew"></param>
        /// <returns>List<Subscriber>></returns>
        public async Task<List<Subscriber>> GetByIdsAsync(List<int> subscriberIds, bool requestNew = false) => await GetByIdsAsync(subscriberIds.ToArray(), requestNew);
     
        /// <summary>
        /// Get a list of subscribers by Id
        /// </summary>
        /// <param name="subscriberIds"></param>
        /// <param name="requestNew"></param>
        /// <returns>List<Subscriber>></returns>
        public async Task<List<Subscriber>> GetByIdsAsync(int[] subscriberIds, bool requestNew = false)
        {
            List<Subscriber> subscribers = new List<Subscriber>();

            if (!requestNew)
                subscribers.AddRange(cache.Where((subscriber) => subscriberIds.Any((subscriberId) => subscriberId == subscriber.Id)).ToList());

            if (subscribers.Count != subscriberIds.Length)
            {
                var result = await WebSocket.Emit<Response<Dictionary<int, Response<Subscriber>>>>(Request.SUBSCRIBER_PROFILE, new
                {
                    headers = new
                    {
                        version = 4
                    },
                    body = new
                    {
                        idList = subscriberIds.Where((subscriberId) => !subscribers.Any((subscriber) => subscriber.Id == subscriberId)).ToList(),
                        subscribe = true,
                        extended = true
                    }
                });

                if (result.Success)
                    foreach (var subscriber in result.Body)
                        subscribers.Add(Process(subscriber.Value.Success ? subscriber.Value.Body : new Subscriber(subscriber.Key)));
                else
                    subscribers.AddRange(subscriberIds.Where((subscriberId) => !subscribers.Any((subscriber) => subscriber.Id == subscriberId)).ToList().Select(r => new Subscriber(r)).ToList());
            }

            return subscribers;
        }

        internal Subscriber Process(Subscriber subscriber)
        {
            subscriber.Bot = Bot;

            if(subscriber.Exists)
            {
                if (cache.Any((sub) => sub.Id == subscriber.Id))
                    cache.FirstOrDefault((sub) => sub.Id == subscriber.Id).Update(subscriber);
                else
                    cache.Add(subscriber);
            }

            return subscriber;
        }

        internal SubscriberHelper(WolfBot bot, WebSocket WebSocket) : base(bot, WebSocket) { }
    }
}