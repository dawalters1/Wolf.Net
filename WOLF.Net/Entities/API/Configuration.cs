using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Enums.Misc;

namespace WOLF.Net.Entities.API
{
    public class Configuration
    {
        public bool UseTranslations { get; set; } = false;
        public bool IgnoreOfficialBots { get; set; } = false;
        public Language DefaultLanguage { get; set; } = Language.ENGLISH;
        public int BotOwnerId { get; set; }

        public Configuration(bool useTranslations = false, bool ignoreOfficialBots = false, int botOwnerId = 0, Language defaultLanguage = Language.ENGLISH)
        {
            BotOwnerId = botOwnerId;
            UseTranslations = useTranslations;
            IgnoreOfficialBots = ignoreOfficialBots;
            DefaultLanguage = defaultLanguage;
        }
    }
}