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

        public ActionType Action => Type.ToLowerInvariant() switch
        {
            "ban" => ActionType.BAN,
            "kick" => ActionType.KICK,
            "mod" => ActionType.MOD,
            "reset" => ActionType.REGULAR,
            "admin" => ActionType.ADMIN,
            "join" => ActionType.JOIN,
            "leave" => ActionType.LEAVE,
            "owner" => ActionType.OWNER,
            "silence" => ActionType.SILENCE,
            _ => ActionType.REGULAR,
        };

        public Capability Role => Type.ToLowerInvariant() switch
        {
            "ban" => Capability.BANNED,
            "kick" => Capability.NOT_MEMBER,
            "mod" => Capability.MOD,
            "reset" => Capability.REGULAR,
            "admin" => Capability.ADMIN,
            "join" => Capability.REGULAR,
            "leave" => Capability.NOT_MEMBER,
            "owner" => Capability.OWNER,
            "silence" => Capability.SILENCED,
            _ => Capability.REGULAR,
        };
    }
}