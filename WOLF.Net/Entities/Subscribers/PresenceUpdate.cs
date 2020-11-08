using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Subscribers
{
    public class PresenceUpdate
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("deviceType")]
        public DeviceType DeviceType { get;set; }

        [JsonProperty("onlineState")]
        public OnlineState OnlineState { get; set; }
    }
}
