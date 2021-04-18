using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WOLF.Net.Entities.Messages;

namespace WOLF.Net.Utils
{
    internal static class Internal
    {
        internal static KeyValuePair<string, string> GetTriggerAndLanguage(this WolfBot bot, string trigger, string content)
        {
            if (!bot.Configuration.UseTranslations)
                return new KeyValuePair<string, string>(bot.Configuration.DefaultLanguage.ToPhraseLanguage(), trigger);

            var phrase = bot.Phrase().cache.Where(r => r.Name.IsEqual(trigger)).ToList()
                .OrderByDescending(r => r.Value.Length)
                .FirstOrDefault(r => content.ToLower().StartsWith(r.Value.ToLower()));

            if (phrase != null)
                return new KeyValuePair<string, string>(phrase.Language, phrase.Value);

            return new KeyValuePair<string, string>(null, null);
        }

        internal static bool HasProperty(this object obj, string propertyName) => obj.GetType().GetProperty(propertyName) != null;

        internal static bool PropretyExists(dynamic obj, string name)
        {
            Type objType = obj.GetType();

            if (objType == typeof(ExpandoObject))
            {
                return ((IDictionary<string, object>)obj).ContainsKey(name);
            }

            return objType.GetProperty(name) != null;
        }

        public static IEnumerable<System.Type> GetAllTypes(this System.Type type, bool nestedOnly = false)
        {
            foreach (var assemblies in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var typeInfo in assemblies.DefinedTypes)
                {
                    if (nestedOnly && typeInfo.IsNested)
                        yield return typeInfo;

                    if (typeInfo.IsAbstract || typeInfo.IsInterface || typeInfo.IsNested || !type.IsAssignableFrom(typeInfo))
                        continue;

                    yield return typeInfo;
                }
            }
        }

        internal static bool StartsWithCommand(this string content, string startsWith)
        {
            content = content.ToLower();
            startsWith = startsWith.ToLower();

            if (startsWith.Length > content.Length)
                return false;

            if (content.StartsWith(startsWith))
            {
                var nextChar = content.ElementAtOrDefault(startsWith.Length);

                return nextChar == default || string.IsNullOrWhiteSpace(nextChar.ToString());
            }

            return false;
        }
    }
}