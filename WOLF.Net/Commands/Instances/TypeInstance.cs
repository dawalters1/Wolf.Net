using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WOLF.Net.Commands.Attributes;

namespace WOLF.Net.Commands.Instances
{
    public class TypeInstance<T>
    {
        public Type Type { get; set; }

        public T Value { get; set; }

        public List<BaseAttribute> Attributes { get; set; }

        public TypeInstance(Type type, T value)
        {
            Type = type;
            Value = value;

            Attributes = type.GetCustomAttributes()
                        .Where(t => t is BaseAttribute)
                        .Cast<BaseAttribute>()
                        .ToList();
        }
    }
}
