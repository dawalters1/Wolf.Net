using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Charms
{
    public class SubscriberCharm
    {
        [JsonProperty("selectedList")]
        public List<SelectedCharm> SelectedList { get; set; }
    }
}
