using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Entities.API;
using WOLF.Net.Networking;

namespace WOLF.Net.Helper
{
    public class PhraseHelper:BaseHelper<Phrase>
    {
        public void Load(params Phrase[] phrases) => Load(phrases.ToList());

        public void Load(List<Phrase> phrases)
        {
            if (phrases.Any((phrase) => string.IsNullOrWhiteSpace(phrase.Name) || string.IsNullOrWhiteSpace(phrase.Value) || string.IsNullOrWhiteSpace(phrase.Language)))
                throw new Exception("Name, Value or Language cannot be empty");

            cache = phrases;
        }

        public string GetByName(string language, string name)
        {
            if (string.IsNullOrWhiteSpace(language) || string.IsNullOrWhiteSpace(name))
                throw new Exception("Language and Name cannot be empty");

            var requested = cache.FirstOrDefault((phrase) => phrase.Language.IsEqual(language) && phrase.Name.IsEqual(name));

            if (requested == null)
                if (language.IsEqual("en"))
                    return null;
                else
                    return GetByName("en", name);

            return requested.Value;

        }

        public bool IsRequestedPhrase(string name, string value) => cache.Any((phrase) => phrase.Name.IsEqual(name) && phrase.Value.IsEqual(value));

        public string GetNameByValue(string value)
        {
            var requested = cache.FirstOrDefault((phrase) => phrase.Value.IsEqual(value));

            return requested?.Name;
        }

        internal PhraseHelper(WolfBot Bot, WebSocket WebSocket) : base(Bot, WebSocket) { }
    }
}
