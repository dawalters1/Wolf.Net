using Newtonsoft.Json;
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
        internal static async Task<List<dynamic>> GetGroupAdsFromMessageAsync(this string content, WolfBot bot) => (await Task.WhenAll(Regex.Matches(content, @"\[.*?\]").Select(async (result) =>
        {
            dynamic link = new ExpandoObject();
            link.start = result.Index;
            link.end = result.Index + result.Value.Length;

            var group = await bot.Group().GetByNameAsync(content.Substring(link.start + 1, result.Value.Length - 2));

            if (group.Exists)
                link.groupId = group.Id;
            return link;

        }))).ToList();
        internal static List<dynamic> GetLinksFromMessageAsync(this string content) => Regex.Matches(content, @"(\b(http|ftp|https):(\/\/|\\\\)[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&:/~\+#]*[\w\-\@?^=%&/~\+#])?|\bwww\.[^\s])").Select((result) =>
       {
           dynamic link = new ExpandoObject();

           link.start = result.Index;
           link.end = result.Index + result.Value.Length;
           link.url = result.Value;

           return link;
       }).ToList();

        internal static KeyValuePair<string, string> GetTriggerAndLanguage(this WolfBot bot, string trigger, string content)
        {
            if (!bot.Configuration.UseTranslations)
                return new KeyValuePair<string, string>(bot.Configuration.DefaultLanguage.ToPhraseLanguage(), trigger);

            var phrase = bot.Phrase().cache.Where(r => r.Name.IsEqual(trigger)).ToList()
                .OrderByDescending(r => r.Value.Length)
                .FirstOrDefault(r => content.ToLowerInvariant().StartsWith(r.Value.ToLowerInvariant()));

            if (phrase != null)
                return new KeyValuePair<string, string>(phrase.Language, phrase.Value);

            return new KeyValuePair<string, string>(null, null);
        }

        internal static bool HasProperty(this object obj, string propertyName) => obj.GetType().GetProperty(propertyName) != null;

        internal static bool PropretyExists(dynamic obj, string name) => obj.GetType() == typeof(ExpandoObject) ? ((IDictionary<string, object>)obj).ContainsKey(name) : obj.GetType().GetProperty(name) != null;
     
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
            content = content.ToLowerInvariant();
            startsWith = startsWith.ToLowerInvariant();

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