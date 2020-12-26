using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WOLF.Net.Enums.Groups;

namespace WOLF.Net.Utilities
{
    internal static class Internal
    {
        internal static KeyValuePair<string, string> GetTriggerAndLanguage(this WolfBot bot, string trigger, string content)
        {
            if (!bot.UsingTranslations)
                return new KeyValuePair<string, string>("en", trigger);

            var phrase = bot.GetAllPhrasesByName(trigger).OrderByDescending(r => r.Value.Length).FirstOrDefault(r => content.StartsWith(r.Value.ToLower()));

            if (phrase != null)
                return new KeyValuePair<string, string>(phrase.Language, phrase.Value);

            return new KeyValuePair<string, string>(null, null);
        }

        internal static bool HasProperty(this object obj, string propertyName) => obj.GetType().GetProperty(propertyName) != null;

        internal static string ToErrorMessage(this string eventString, int subCode, string subMessage = null)
        {
            return (subCode) switch
            {
                0 => "No such user",
                2 => $"TOS VIOLATIONS - {subMessage}",
                8 => "Group name already exists",
                9 => "Group name not allowed",
                15 => "Group name must be atleast 3 characters",
                4 => "Higher level required",
                100 => "Group full",
                101 => "Maximum permitted number of groups",
                105 => "Group doesn't exist",
                107 => "Banned",
                112 => "Restricted to new users",
                115 => "Group locked",
                116 => "Too many accounts",
                117 => "Game join only",
                110 => "Already in group",
                1 => eventString.IsEqual("security login") ? "Incorrect login information" : eventString.IsEqual("group member add") ? "Incorrect Password" : "Not in group, silenced or banned",
                _ => $"Request {eventString} failed with ({subCode})"
            };
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