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

        public List<CustomAttribute> CustomAttributes { get; set; }

        public MethodInstance(MethodInfo type, T value)
        {
            Type = type;
            Value = value;

            CustomAttributes = type.GetCustomAttributes()
                        .Where(t => t is CustomAttribute)
                        .Cast<CustomAttribute>().Where(r=>r.GetType() != typeof(RequiredPermissions))
                        .ToList();
         }
    }
}
