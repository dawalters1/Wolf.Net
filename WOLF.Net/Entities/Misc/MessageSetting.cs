﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Misc;

namespace WOLF.Net.Entities.Misc
{
    public class MessageSetting
    {
        [JsonProperty("spamFilter")]
        public SpamFilter SpamFilter { get; set; }
    }

    public class SpamFilter
    {
        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("tier")]
        public MessageFilterType Tier { get; set; }
    }
}
