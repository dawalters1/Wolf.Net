using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Entities.Phrases;
using WOLF.Net.Utilities;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        public List<Phrase> Phrases = new List<Phrase>();

        public void LoadPhrases(List<Phrase> phrases)
        {
            if (phrases.Any(r => string.IsNullOrWhiteSpace(r.Name) || string.IsNullOrWhiteSpace(r.Value) || string.IsNullOrWhiteSpace(r.Language)))
                throw new Exception("Translation Key, Value or Language cannot be null");

            Phrases = phrases;
        }

        public string GetPhraseByName(string language, string name)
        {
            var phrase = Phrases.FirstOrDefault(r => r.Language.IsEqual(language) && r.Name.IsEqual(name));

            if (phrase == null)
                if (language != "en")
                    return GetPhraseByName("en", name);
                else
                    return $"missing-{language}-translation: {name}";

            return phrase.Value;
        }

        public List<Phrase> GetAllPhrasesByLanguageAndName(string language, string name)
        {
            var result = Phrases.Where(r => r.Language.IsEqual(language) && r.Name.IsEqual(name)).ToList();
            return result;
        }
        public List<Phrase> GetAllPhrasesByName(string name) => Phrases.Where(r => r.Name.IsEqual(name)).ToList();

        public bool IsRequestedPhrase(string name, string value) => Phrases.Any(r => r.Value.IsEqual(value) && r.Name.IsEqual(name));

        public string GetPhraseName(string value)
        {
            var phrase = Phrases.FirstOrDefault(r => r.Value.IsEqual(value));

            if (phrase == null)
                return "non-existent-phrase-name";

            return phrase.Name;
        }
    }
}
