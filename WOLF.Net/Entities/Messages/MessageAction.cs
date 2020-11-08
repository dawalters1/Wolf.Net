using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Groups;

namespace WOLF.Net.Entities.Messages
{
    public class MessageAction
    {
        [JsonProperty("instigatorId")]
        public int InstigatorId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public GroupActionType Action => Type.ToLower() switch
        {
            "ban" => GroupActionType.Ban,
            "kick" => GroupActionType.Kick,
            "mod" => GroupActionType.Mod,
            "reset" => GroupActionType.Regular,
            "admin" => GroupActionType.Admin,
            "join" => GroupActionType.Join,
            "leave" => GroupActionType.Leave,
            _ => GroupActionType.Silence,
        };

        public Capability Role => Type.ToLower() switch
        {
            "ban" => Capability.Banned,
            "kick" => Capability.NotGroupSubscriber,
            "mod" => Capability.Mod,
            "reset" => Capability.Regular,
            "admin" => Capability.Admin,
            "join" => Capability.Regular,
            "leave" => Capability.NotGroupSubscriber,
            _ => Capability.Silenced,
        };
    }
}
