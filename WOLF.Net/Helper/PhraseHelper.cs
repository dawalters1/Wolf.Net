using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Commands.Form;
using WOLF.Net.Entities.API;
using WOLF.Net.Networking;

namespace WOLF.Net.Helper
{
    public class PhraseHelper:BaseHelper<Phrase>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of phrases in the bot</returns>
        public IReadOnlyList<Phrase> List() => cache;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>PhraseCount (Phrase count by language, and total count)</returns>
        public PhraseCount Count() => new PhraseCount(cache);

        /// <summary>
        /// Clear the bots phrase list
        /// </summary>
        public void Clear() => cache.Clear();

        /// <summary>
        /// Load phrases into the bots cache
        /// </summary>
        /// <param name="phrases"></param>
        public void Load(params Phrase[] phrases) => Load(phrases.ToList());

        /// <summary>
        /// Load phrases into the bots cache
        /// </summary>
        /// <param name="phrases"></param>
        public void Load(List<Phrase> phrases)
        {
            if (phrases.Any((phrase) => string.IsNullOrWhiteSpace(phrase.Name) || string.IsNullOrWhiteSpace(phrase.Value) || string.IsNullOrWhiteSpace(phrase.Language)))
                throw new Exception("Name, Value or Language cannot be empty");

            cache = phrases;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of all languages in the phrase cache</returns>
        public List<string> GetLanguageList() => cache.Select(r => r.Language).Distinct().ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>All phrases by name</returns>
        public List<string> GetAllByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Name cannot be empty");

            return cache.Where((phrase) => phrase.Name.IsEqual(name)).Select((phrase)=>phrase.Value).ToList();
        }

        /// <summary>
        ///  Request a phrase by command and name
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="name"></param>
        /// <returns>Value of a phrase if it exists</returns>
        public string GetByName(CommandData commandData, string name) => GetByName(commandData.Language, name);

        /// <summary>
        /// Request a phrase by command and name
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="name"></param>
        /// <returns>Value of a phrase if it exists</returns>
        public string GetByName(FormData formData, string name) => GetByName(formData.Language, name);


        /// <summary>
        /// Request a phrase by language and name
        /// </summary>
        /// <param name="language"></param>
        /// <param name="name"></param>
        /// <returns>Value of a phrase if it exists</returns>
        public string GetByName(string language, string name)
        {
            if (string.IsNullOrWhiteSpace(language) || string.IsNullOrWhiteSpace(name))
                throw new Exception("Language and Name cannot be empty");

            var requested = cache.FirstOrDefault((phrase) => phrase.Language.IsEqual(language) && phrase.Name.IsEqual(name));

            if (requested == null)
                if (language.IsEqual(Bot.Configuration.DefaultLanguage.ToPhraseLanguage()))
                    return $"requested_{language}_phrase_{name}_missing";
                else
                    return GetByName(Bot.Configuration.DefaultLanguage.ToPhraseLanguage(), name);

            return requested.Value;

        }

        /// <summary>
        /// Check to see if the input by a user is the expected phrase
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns>bool</returns>
        public bool IsRequestedPhrase(string name, string value) => cache.Any((phrase) => phrase.Name.IsEqual(name) && phrase.Value.IsEqual(value));
        
        /// <summary>
        /// Get the name of a phrase by value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Name of phrase if it exists</returns>
        public string GetNameByValue(string value)
        {
            var requested = cache.FirstOrDefault((phrase) => phrase.Value.IsEqual(value));

            return requested?.Name;
        }

        internal PhraseHelper(WolfBot Bot, WebSocket WebSocket) : base(Bot, WebSocket) { }
    }
}
