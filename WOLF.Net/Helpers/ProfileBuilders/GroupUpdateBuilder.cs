using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Entities.Groups.Stages;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Misc;

namespace WOLF.Net.Helpers.ProfileBuilders
{
    public class GroupUpdateBuilder
    {
        private WolfBot Bot { get; set; }

        private int Id { get; set; }

        private string Description { get; set; }

        private bool Peekable { get; set; }

        private bool Discoverable { get; set; }

        private bool AdvancedAdmin { get; set; }

        private int EntryLevel { get; set; }

        private Language Language { get; set; }

        private Category Category { get; set; }

        private string LongDescription { get; set; }

        public GroupAudioConfiguration GroupAudioConfiguration = new GroupAudioConfiguration()
        {
            Enabled = false,
            MinRepLevel = 0,
            StageId = 1
        };
        public GroupUpdateBuilder(WolfBot bot, Entities.Groups.Group group)
        {
            Bot = bot;
            Id = group.Id;
            Description = group.Description;
            LongDescription = group.Extended.LongDescription;
            Category = group.Extended.Category;
            Language = group.Extended.Language;
            EntryLevel = group.Extended.EntryLevel;
            AdvancedAdmin = group.Extended.AdvancedAdmin;
            Discoverable = group.Extended.Discoverable;
            Peekable = group.Peekable;
            GroupAudioConfiguration = group.AudioConfiguration;
        }

        public GroupUpdateBuilder SetShortDescription(string shortDescription)
        {
            Description = shortDescription;
            return this;
        }

        public GroupUpdateBuilder SetLongDescription(string longDescription)
        {
            LongDescription = longDescription;
            return this;
        }

        public GroupUpdateBuilder SetCategory(Category category)
        {
            Category = category;
            return this;
        }

        public GroupUpdateBuilder SetLanguage(Language language)
        {
            Language = language;
            return this;
        }

        public GroupUpdateBuilder SetEntryLevel(int entryLevel)
        {
            EntryLevel = entryLevel;
            return this;
        }

        public GroupUpdateBuilder SetAdvancedAdmin(bool isAdvancedAdminEnabled)
        {
            AdvancedAdmin = isAdvancedAdminEnabled;
            return this;
        }

        public GroupUpdateBuilder SetDiscoverable(bool isDiscoverable)
        {
            Discoverable = isDiscoverable;
            return this;
        }

        public GroupUpdateBuilder SetConversationPreview(bool isConversationPreviewEnabled)
        {
            Peekable = isConversationPreviewEnabled;
            return this;
        }

        public async Task<Response> Save()
        {
            return await Bot.WolfClient.Emit(Request.GROUP_PROFILE_UPDATE, new Entities.Groups.Group()
            {
                Id = Id,
                Extended = new Extended()
                {
                    AdvancedAdmin = AdvancedAdmin,
                    LongDescription = LongDescription,
                    Category = Category,
                    EntryLevel = EntryLevel,
                    Discoverable = Discoverable
                },
                Description = Description,
                Peekable = Peekable,
            });
        }
    }
}