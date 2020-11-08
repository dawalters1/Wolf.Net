using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace WOLF.Net.Utilities
{
    internal static class Internal
    {
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