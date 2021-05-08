using Newtonsoft.Json;

namespace WOLF.Net.Entities.Achievements
{
    public class Category
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
