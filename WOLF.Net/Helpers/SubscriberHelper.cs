using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Subscribers;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        public List<Subscriber> Subscribers { get; internal set; } = new List<Subscriber>();

        /// <summary>
        /// Get a subscriber by Id
        /// </summary>
        /// <param name="subscriberId"></param>
        /// <param name="requestNew"></param>
        /// <returns>Subscriber</returns>
        public async Task<Subscriber> GetSubscriberAsync(int subscriberId, bool requestNew = false) => (await GetSubscribersAsync(new List<int>() { subscriberId }, requestNew)).FirstOrDefault();

        /// <summary>
        /// Get a list of subscribers by Id
        /// </summary>
        /// <param name="subscriberIds"></param>
        /// <param name="requestNew"></param>
        /// <returns>List<Subscriber>></returns>
        public async Task<List<Subscriber>> GetSubscribersAsync(List<int> subscriberIds, bool requestNew = false) => await GetSubscribersAsync(subscriberIds.ToArray(), requestNew);

        /// <summary>
        /// Get a list of subscribers by Id
        /// </summary>
        /// <param name="subscriberIds"></param>
        /// <param name="requestNew"></param>
        /// <returns>List<Subscriber>></returns>
        public async Task<List<Subscriber>> GetSubscribersAsync(int[] subscriberIds, bool requestNew = false)
        {
            subscriberIds = subscriberIds.Distinct().ToArray();

            List<Subscriber> subscribers = new List<Subscriber>();

            if (!requestNew)
                subscribers.AddRange(Subscribers.Where((subscriber) => subscriberIds.Any((subscriberId) => subscriberId == subscriber.Id)).ToList());

            if (subscribers.Count != subscriberIds.Length)
            {
                foreach (var batchSubscriberIdList in subscriberIds.Where((subscriberId) => !subscribers.Any((subscriber) => subscriber.Id == subscriberId)).ToList().ChunkBy(50))
                {
                    var result = await _webSocket.Emit<Response<Dictionary<int, Response<Subscriber>>>>(Request.SUBSCRIBER_PROFILE, new
                    {
                        headers = new
                        {
                            version = 4
                        },
                        body = new
                        {
                            idList = batchSubscriberIdList,
                            subscribe = true,
                            extended = true
                        }

                    });

                    if (result.Success)
                        foreach (var subscriber in result.Body)
                            subscribers.Add(ProcessSubscriber(subscriber.Value.Success ? subscriber.Value.Body : new Subscriber(subscriber.Key)));
                    else
                        subscribers.AddRange(subscriberIds.Where((subscriberId) => !subscribers.Any((subscriber) => subscriber.Id == subscriberId)).ToList().Select(r => new Subscriber(r)).ToList());
                }
            }

            return subscribers;
        }

        internal Subscriber ProcessSubscriber(Subscriber subscriber)
        {
            subscriber.Bot = this;

            if (subscriber.Exists)
            {
                if (Subscribers.Any((sub) => sub.Id == subscriber.Id))
                    Subscribers.FirstOrDefault((sub) => sub.Id == subscriber.Id).Update(subscriber);
                else
                    Subscribers.Add(subscriber);
            }

            return subscriber;
        }
    }
}