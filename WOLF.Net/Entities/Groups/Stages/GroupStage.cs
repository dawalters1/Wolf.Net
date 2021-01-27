using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WOLF.Net.Entities.Groups.Stages
{
    public class GroupStage
    {
        [JsonProperty("id")]
        public int Id { get;set; }

        [JsonProperty("imageUrl")]
        public Uri ImageUrl { get;set; }

        [JsonProperty("name")]
        public string Name { get;set; }

        [JsonProperty("productId")]
        public int? ProductId { get;set; }

        [JsonProperty("schemaUrl")]
        public Uri SchemaUrl { get;set; }

       Schema cachedSchema { get; set; }

        public Schema GetSchema(bool requestNew = false)
        {
            if (cachedSchema != null && !requestNew)
                return cachedSchema;

            else
            {
                using var webclient = new WebClient();

                return cachedSchema = JsonConvert.DeserializeObject<Schema>(webclient.DownloadString(SchemaUrl));
            }
        }
    }

    public class Schema
    {
        [JsonProperty("version")]
        public int Version { get;set; }

        [JsonProperty("name")]
        public string Name { get;set; }

        [JsonProperty("stage")]
        public Stage Stage { get;set; }

        [JsonProperty("slots")]
        public Slots Slots { get;set; }

    }

    public class SingleUrl
    {
        [JsonProperty("url")]
        public string Url { get;set; }
    }

    public class Trims
    {
        [JsonProperty("right")]
        public SingleUrl Right { get;set; }

        [JsonProperty("left")]
        public SingleUrl Left { get;set; }
    }

    public class Padding
    {
        [JsonProperty("left")]
        public int Left { get;set; }

        [JsonProperty("right")]
        public int Right { get;set; }
    }

    public class Stage
    {
        [JsonProperty("background")]
        public SingleUrl Background { get;set; }

        [JsonProperty("trims")]
        public Trims Trims { get;set; }

        [JsonProperty("padding")]
        public Padding Padding { get;set; }

        [JsonProperty("maxWidth")]
        public int MaxWidth { get;set; }

    }

    public class UrlList
    {
        [JsonProperty("urls")]
        public List<string> Urls { get;set; }

    }

    public class ActivityOverlay
    {
        [JsonProperty("urls")]
        public List<string> Urls { get;set; }

        [JsonProperty("showAnimatedRings")]
        public List<bool> ShowAnimatedRings { get;set; }

        [JsonProperty("useAvatarTints")]
        public List<bool> UseAvatarTints { get;set; }
    }

    public class SlotState
    {
        [JsonProperty("urls")]
        public List<string> Urls { get;set; }

        [JsonProperty("avatarY")]
        public List<int> AvatarY { get;set; }

        [JsonProperty("labelY")]
        public List<int> LabelY { get;set; }

        [JsonProperty("labelColurs")]
        public List<string> LabelColours { get;set; }

    }


    public class States
    {
        [JsonProperty("occupied")]
        public SlotState Occupied { get;set; }

        [JsonProperty("locked")]
        public SlotState Locked { get;set; }

        [JsonProperty("empty")]
        public SlotState Empty { get;set; }

        [JsonProperty("muted")]
        public SlotState Muted { get;set; }

    }

    public class Slots
    {
        [JsonProperty("avatarSzies")]
        public List<int> AvatarSizes { get;set; }

        [JsonProperty("zPositions")]
        public List<int> ZPositions { get;set; }

        [JsonProperty("forground")]
        public UrlList Foreground { get;set; }

        [JsonProperty("background")]
        public UrlList Background { get;set; }

        [JsonProperty("activityOverlay")]
        public ActivityOverlay ActivityOverlay { get;set; }

        [JsonProperty("states")]
        public States States { get;set; }
    }
}