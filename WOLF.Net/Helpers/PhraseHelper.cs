using System;
using System.Collections.Generic;
using System.Linq;
using WOLF.Net.Commands.Commands;
using WOLF.Net.Commands.Form;
using WOLF.Net.Entities.API;

namespace WOLF.Net
{
    public partial class WolfBot
    {
        public List<Phrase> Phrases = new List<Phrase>();


        /// <summary>
        /// 
        /// </summary>
        /// <returns>PhraseCount (Phrase count by language, and total count)</returns>
        public PhraseCount GetPhraseCount() => new PhraseCount(Phrases);

        /// <summary>
        /// Clear the bots phrase list
        /// </summary>
        public void ClearPhrases() => Phrases.Clear();

        /// <summary>
        /// Load phrases into the bots cache
        /// </summary>
        /// <param name="phrases"></param>
        public void LoadPhrases(params Phrase[] phrases) => LoadPhrases(phrases.ToList());

        /// <summary>
        /// Load phrases into the bots cache
        /// </summary>
        /// <param name="phrases"></param>
        public void LoadPhrases(List<Phrase> phrases)
        {
            if (phrases.Any((phrase) => string.IsNullOrWhiteSpace(phrase.Name) || string.IsNullOrWhiteSpace(phrase.Value) || string.IsNullOrWhiteSpace(phrase.Language)))
                throw new Exception("Name, Value or Language cannot be empty");

            Phrases = phrases;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>List of all languages in the phrase cache</returns>
        public List<string> GetPhraseLanguageList() => Phrases.Select(r => r.Language).Distinct().ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>All phrases by name</returns>
        public List<string> GetAllPhrasesByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Name cannot be empty");

            return Phrases.Where((phrase) => phrase.Name.IsEqual(name)).Select((phrase) => phrase.Value).ToList();
        }

        /// <summary>
        ///  Request a phrase by command and name
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="name"></param>
        /// <returns>Value of a phrase if it exists</returns>
        public string GetPhraseByName(CommandData commandData, string name) => GetPhraseByName(Configuration.UseSubLanguage?commandData.SubLanguage:commandData.Language, name);

        /// <summary>
        /// Request a phrase by command and name
        /// </summary>
        /// <param name="formData"></param>
        /// <param name="name"></param>
        /// <returns>Value of a phrase if it exists</returns>
        public string GetPhraseByName(FormData formData, string name) => GetPhraseByName(formData.Language, name);


        /// <summary>
        /// Request a phrase by language and name
        /// </summary>
        /// <param name="language"></param>
        /// <param name="name"></param>
        /// <returns>Value of a phrase if it exists</returns>
        public string GetPhraseByName(string language, string name)
        {
            if (string.IsNullOrWhiteSpace(language) || string.IsNullOrWhiteSpace(name))
                throw new Exception("Language and Name cannot be empty");

            var requested = Phrases.FirstOrDefault((phrase) => phrase.Language.IsEqual(language) && phrase.Name.IsEqual(name));

            if (requested == null)
                if (language.IsEqual(Configuration.DefaultLanguage.ToPhraseLanguage()))
                    return $"requested_{language}_phrase_{name}_missing";
                else
                    return GetPhraseByName(Configuration.DefaultLanguage.ToPhraseLanguage(), name);

            return requested.Value;

        }

        /// <summary>
        /// Check to see if the input by a user is the expected phrase
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns>bool</returns>
        public bool IsRequestedPhrase(string name, string value) => Phrases.Any((phrase) => phrase.Name.IsEqual(name) && phrase.Value.IsEqual(value));

        /// <summary>
        /// Get the name of a phrase by value
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Name of phrase if it exists</returns>
        public string GetPhraseName(string value) => Phrases.FirstOrDefault((phrase) => phrase.Value.IsEqual(value))?.Name;
    }
}