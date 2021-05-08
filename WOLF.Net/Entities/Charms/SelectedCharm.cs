using Newtonsoft.Json;

namespace WOLF.Net.Entities.Charms
{
    public class SelectedCharm
    {
        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("charmId")]
        public int CharmId { get; set; }

        public SelectedCharm(int position, int charmId)
        {
            Position = position;
            CharmId = charmId;
        }
    }
}
