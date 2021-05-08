using Newtonsoft.Json;

namespace WOLF.Net.Entities.Misc
{
    public class ContactUpdate
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("targetId")]
        public int TargetId { get; set; }
    }
}
