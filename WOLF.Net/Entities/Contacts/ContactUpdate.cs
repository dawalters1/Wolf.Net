using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Contacts
{
    public class ContactUpdate
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("targetId")]
        public int TargetId { get; set; }
    }
}
