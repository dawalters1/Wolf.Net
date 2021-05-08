using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WOLF.Net.Commands.Attributes;

namespace WOLF.Net.Commands.Instances
{
    public class TypeInstance<T>
    {
        public Type Type { get; set; }

        public T Value { get; set; }

        public List<CustomAttribute> CustomAttributes { get; set; }

        public TypeInstance(Type type, T value)
        {
            Type = type;
            Value = value;

            CustomAttributes = type.GetCustomAttributes()
                        .Where(t => t is CustomAttribute)
                     .Cast<CustomAttribute>().Where(r => r.GetType() != typeof(RequiredPermissions))
                        .ToList();
        }
    }
}
