using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Misc
{
    public class IdHash
    {
        public IdHash() { }

        public IdHash(int id, string hash)
        {
            Id = id;
            Hash = hash;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }
    }
}
