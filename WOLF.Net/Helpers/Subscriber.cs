using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Subscribers;
using WOLF.Net.Utilities;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        public List<Subscriber> Subscribers { get; internal set; } = new List<Subscriber>();

        /// <summary>
        /// Add a subscriber as a contact
        /// </summary>
        /// <param name="subscriberId">The id of the subscriber</param>
        /// <returns></returns>
        public async Task<Response> AddSubscriberAsync(int subscriberId)
        {
            return await WolfClient.Emit(Request.SUBSCRIBER_CONTACT_ADD, new
            {
                id = subscriberId,
                Message = "I would like to add you as a contact!"
            });
        }

        /// <summary>
        /// Block a subscriber
        /// </summary>
        /// <param name="subscriberId">The id of the subscriber</param>
        /// <returns></returns>
        public async Task<Response> BlockSubscriberAsync(int subscriberId)
        {
            return await WolfClient.Emit(Request.SUBSCRIBER_BLOCK_ADD, new
            {
                id = subscriberId,
            });
        }

        /// <summary>
        /// Get a subscriber
        /// </summary>
        /// <param name="subscriberId">The id of the subscriber</param>
        /// <param name="requestNew">Used by the API when a subscriber update event happens and hashes do not match</param>
        /// <returns></returns>
        public async Task<Subscriber> GetSubscriberAsync(int subscriberId, bool requestNew = false)
        {
            if (!requestNew && Subscribers.Any(r => r.Id == subscriberId))
            {
                return Subscribers.FirstOrDefault(r => r.Id == subscriberId);
            }

            var result = await WolfClient.Emit<Dictionary<int, Response<Subscriber>>>(Request.SUBSCRIBER_PROFILE, new
            {
                headers = new
                {
                    version = 4
                },
                body = new
                {
                    idList = new List<int>() { subscriberId },
                    subscribe = true,
                    extended = true
                }
            });

            if (result.Success)
            {
                var subscriber = result.Body[subscriberId];

                if (subscriber.Success)
                {
                    ProcessSubscriber(subscriber.Body);

                    return subscriber.Body;
                }
            }

            return new Subscriber(subscriberId);
        }

        /// <summary>
        /// Bulk get subscribers
        /// </summary>
        /// <param name="subscriberIds">A list of subscriber ids</param>
        /// <param name="requestNew">Used by the API when a subscriber update event happens and hashes do not match</param>
        /// <returns></returns>
        public async Task<List<Subscriber>> GetSubscribersAsync(List<int> subscriberIds, bool requestNew = false)
        {
            List<Subscriber> subscribers = new List<Subscriber>();

            if (!requestNew)
            {
                foreach (var subscriberId in subscriberIds.ToList())
                    if (Subscribers.Any(r => r.Id == subscriberId))
                    {
                        subscribers.Add(Subscribers.FirstOrDefault(r => r.Id == subscriberId));
                        subscriberIds.RemoveAll(r => r == subscriberId);
                    }
            }

            if (subscriberIds.Count == 0)
                return subscribers;

            var chunks = subscriberIds.ChunkBy(50).ToList();

            foreach (var chunk in chunks)
            {
                var results = await WolfClient.Emit<Dictionary<int, Response<Subscriber>>>(Request.SUBSCRIBER_PROFILE, new
                {
                    headers = new
                    {
                        version = 4
                    },
                    body = new
                    {
                        idList = chunk,
                        subscribe = true,
                        extended = true
                    }
                });

                if (results.Success)
                {
                    foreach (var subscriber in results.Body)
                    {
                        if (subscriber.Value.Success)
                        {
                            ProcessSubscriber(subscriber.Value.Body);

                            subscribers.Add(subscriber.Value.Body);
                        }
                        else
                            subscribers.Add(new Subscriber(subscriber.Key));
                    }
                }

            }

            return subscribers;
        }

        /// <summary>
        /// Remove a subscriber as a contact
        /// </summary>
        /// <param name="subscriberId">The id of the subscriber</param>
        /// <returns></returns>
        public async Task<Response> RemoveSubscriberAsync(int subscriberId)
        {
            return await WolfClient.Emit(Request.SUBSCRIBER_CONTACT_DELETE, new
            {
                id = subscriberId,
            });
        }

        /// <summary>
        /// Unblock a subscriber
        /// </summary>
        /// <param name="subscriberId">The id of the subscriber</param>
        /// <returns></returns>
        public async Task<Response> UnblockSubscriberAsync(int subscriberId)
        {
            return await WolfClient.Emit(Request.SUBSCRIBER_BLOCK_DELETE, new
            {
                id = subscriberId,
            });
        }


        internal void ProcessSubscriber(Subscriber subscriber)
        {
            subscriber.Bot = this;

            if (Subscribers.Any(r => r.Id == subscriber.Id))
                Subscribers.FirstOrDefault(r => r.Id == subscriber.Id).Update(subscriber);
            else
                Subscribers.Add(subscriber);
        }

    }
}