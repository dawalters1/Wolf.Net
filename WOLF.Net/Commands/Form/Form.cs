using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Commands.Form
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class Form : Attribute
    {
        internal string Trigger { get; set; }

        internal double Duration { get; set; }
        public Form(string trigger, double duration = 60000)
        {
            if (string.IsNullOrWhiteSpace(trigger))
                throw new Exception("Form trigger cannot be null or empty");

            if (duration <= 0)
                throw new Exception("Timeout duration cannot be 0 or negative");

            Trigger = trigger;
            Duration = duration;
        }
    }
}
