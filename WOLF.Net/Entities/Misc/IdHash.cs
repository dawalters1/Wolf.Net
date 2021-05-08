using Newtonsoft.Json;

namespace WOLF.Net.Entities.Misc
{
    public class IdHash
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        public IdHash() { }

        public IdHash(int id, string hash)
        {
            Id = id;
            Hash = hash;
        }
    }
}
