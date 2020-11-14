using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using WOLF.Net.Enums.Groups;

namespace WOLF.Net.Utilities
{
    internal static class Internal
    {
        internal static bool HasProperty(this object obj, string propertyName) =>  obj.GetType().GetProperty(propertyName) != null;

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

        public static IEnumerable<T> Union<T>(this IEnumerable<T> data, params T[] objs)
        {
            foreach (var item in data)
                yield return item;
            foreach (var item in objs)
                yield return item;
        }

        public static IEnumerable<System.Type> GetAllTypes(this System.Type type, bool excludeAbstracts = true, bool excludeInterfaces = true)
        {
            var assembly = Assembly.GetEntryAssembly();
            var assemblies = assembly.GetReferencedAssemblies();

            foreach (var asm in assemblies.Union(assembly.GetName()))
            {
                var asml = Assembly.Load(asm);
                foreach (var ty in asml.DefinedTypes)
                {
                    if ((excludeAbstracts && ty.IsAbstract) ||
                        (excludeInterfaces && ty.IsInterface) ||
                        !type.IsAssignableFrom(ty))
                        continue;
                    yield return ty;
                }
            }
        }
    }
}