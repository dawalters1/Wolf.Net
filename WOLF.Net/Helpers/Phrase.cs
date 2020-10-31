using System;
using System.Collections.Generic;
using System.Text;
using WOLF.Net.Entities.Phrase;

namespace WOLF.Net.Helpers
{
    public class Phrase
    {
        private WolfBot Bot;

        public Phrase(WolfBot bot)
        {
            Bot = bot;
        }

        public List<Translation> GetAllByKey(string key)
        {
            return new List<Translation>();
        }

        public Translation GetByName(string language, string name)
        {
            return new Translation()
            {
                Key = name,
                Language = language,
                Value = "test"
            };
        }
    }
}
