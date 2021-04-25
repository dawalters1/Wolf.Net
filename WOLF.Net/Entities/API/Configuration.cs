﻿using System;
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

        public Configuration(bool useTranslations = false, bool ignoreOfficialBots = false, Language defaultLanguage = Language.ENGLISH)
        {
            UseTranslations = useTranslations;
            IgnoreOfficialBots = ignoreOfficialBots;
            DefaultLanguage = defaultLanguage;
        }
    }
}