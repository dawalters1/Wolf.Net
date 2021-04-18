using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Enums.Misc;

namespace WOLF.Net.Entities.API
{
    public class PhraseCount
    {
        public Dictionary<string, int> CountPerLanguage { get; set; } = new Dictionary<string, int>();

        public int Total { get; set; }

        public PhraseCount(List<Phrase> phrases)
        {
            var languages = ((Language[])Enum.GetValues(typeof(Language))).Select((language) => language.ToPhraseLanguage()).ToList();

            foreach (var language in languages)
                CountPerLanguage.Add(language, phrases.Count((phrase) => phrase.Language.IsEqual(language)));

            Total = phrases.Count;
        }
    }
}
