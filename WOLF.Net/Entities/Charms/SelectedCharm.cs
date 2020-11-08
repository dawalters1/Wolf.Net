using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
