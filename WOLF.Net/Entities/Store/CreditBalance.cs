using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Store
{
    public class CreditBalance
    {
        [JsonProperty("balance")]
        public int Balance { get; set; }
    }
}