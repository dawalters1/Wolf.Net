using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.API
{
    public class WolfBotConfig
    {
        public bool UseTranslations { get; set; } = false;

        public bool OfficialBotsCanUseCommands { get; set; } = false;

        public bool RetryOn500Codes { get; set; } = true;

        public WolfBotConfig() { }
    }
}
