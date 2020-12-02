using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Groups;

namespace WOLF.Net.Entities.Groups.Subscribers
{
    public class GroupAction
    {
        [JsonProperty("groupId")]
        public int GroupId { get; set; }

        [JsonProperty("sourceId")]
        public int SourceId { get; set; }

        [JsonProperty("targetId")]
        public int TargetId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public Capability Capabilities => Type switch
        {
            "owner" =>
                    Capability.Owner,
            "admin" =>
                 Capability.Admin,
            "mod" =>
                 Capability.Mod,
            "reset" => Capability.Regular,
            "kick" =>
                 Capability.None,
            "silence" =>
                 Capability.Silenced,
            "ban" =>
                 Capability.Banned,
            _ =>
                 Capability.None
        };
    }
}