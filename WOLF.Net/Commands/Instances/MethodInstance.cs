using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using WOLF.Net.Commands.Attributes;

namespace WOLF.Net.Commands.Instances
{
    public class MethodInstance<T>
    {
        public MethodInfo Type { get; set; }

        public T Value { get; set; }

        public List<BaseAttribute> Attributes { get; set; }

        public MethodInstance(MethodInfo type, T value)
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
