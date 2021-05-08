using System;
using System.Threading.Tasks;
using WOLF.Net.Constants;
using WOLF.Net.Entities.API;
using WOLF.Net.Entities.Groups;
using WOLF.Net.Enums.Groups;
using WOLF.Net.Enums.Misc;

namespace WOLF.Net.Builders.Profiles
{
    public class Group
    {
        private readonly WolfBot bot;

        private readonly int id = 0;

        private string name;

        private string tagLine;

        private string description;

        private bool peekable = false;

        private bool discoverable = true;

        private bool advancedAdmin = false;

        private int entryLevel = 0;

        private Language language = Language.NOT_SPECIFIED;

        private Category category = Category.NOT_SPECIFIED;


        private readonly bool isNew = false;

        public Group(WolfBot bot) { this.bot = bot; isNew = true; }

        public Group(WolfBot bot, Entities.Groups.Group group)
        {
            this.bot = bot;

            if (group!=null||group.Exists)
            {
                this.id = group.Id;
                this.description = group.Description;
                this.description = group.Extended.LongDescription;
                this.category = group.Extended.Category;
                this.language = group.Extended.Language;
                this.entryLevel = group.Extended.EntryLevel;
                this.advancedAdmin = group.Extended.AdvancedAdmin;
                this.discoverable = group.Extended.Discoverable;
                this.peekable = group.Peekable;
            } 
            else
            {
                isNew = true;
            }
        }

        public Group SetName(string name)
        {
            if (!isNew)
                throw new Exception("You cannot change the name of an existing group");

            this.name = name;

            return this;
        }

        public Group SetTagLine(string tagline)
        {
            this.tagLine = tagline;
            return this;
        }

        public Group SetDescription(string description)
        {
            this.description = description;
            return this;
        }

        public Group SetCategory(Category category)
        {
            this.category = category;
            return this;
        }

        public Group SetLanguage(Language language)
        {
            this.language = language;
            return this;
        }

        public Group SetEntryLevel(int entryLevel)
        {
            this.entryLevel = entryLevel;
            return this;
        }

        public Group SetAdvancedAdmin(bool isEnabled)
        {
            this.advancedAdmin = isEnabled;
            return this;
        }

        public Group SetDiscoverable(bool isDiscoverable)
        {
            this.discoverable = isDiscoverable;
            return this;
        }

        public Group SetConversationPreview(bool isEnabled)
        {
            this.peekable = isEnabled;
            return this;
        }

        public async Task<Response> Create() => await (!isNew ? Save() : DoCorrectAction(Request.GROUP_CREATE));

        public async Task<Response> Save() => await (isNew ? Create() : DoCorrectAction(Request.GROUP_PROFILE_UPDATE));

        private async Task<Response> DoCorrectAction(string action)
        {
            return await bot._webSocket.Emit<Response>(action, new Entities.Groups.Group()
            {
                Id = this.id,
                Name = this.name,
                Extended = new Extended()
                {
                    Language = this.language,
                    AdvancedAdmin = this.advancedAdmin,
                    LongDescription = this.description,
                    Category = this.category,
                    EntryLevel = this.entryLevel,
                    Discoverable = this.discoverable
                },
                Description = this.tagLine,
                Peekable = this.peekable,
            });
        }
    }
}