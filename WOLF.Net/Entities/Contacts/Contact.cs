using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Contacts
{
    public class Contact
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("additionalInfo")]
        public AdditionalInfo AdditionalInfo { get; set; }
    }

    public class AdditionalInfo
    {
        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("nicknameShort")]
        public string Nickname { get; set; }

        [JsonProperty("onlineState")]
        public OnlineState OnlineState { get; set; }

        [JsonProperty("privileges")]
        public int Privileges { get; set; }

    }
}
