using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Groups;

namespace WOLF.Net.Entities.Groups
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
                    Capability.OWNER,
            "admin" =>
                 Capability.ADMIN,
            "mod" =>
                 Capability.MOD,
            "reset" => Capability.REGULAR,
            "kick" =>
                 Capability.NOT_MEMBER,
            "silence" =>
                 Capability.SILENCED,
            "ban" =>
                 Capability.BANNED,
            _ =>
                 Capability.NOT_MEMBER
        };

        internal GroupAction(int groupId, int sourceId, int targetId, string type)
        {
            this.GroupId = groupId;
            this.SourceId = sourceId;
            this.TargetId = targetId;
            this.Type = type;
        }
    }
}