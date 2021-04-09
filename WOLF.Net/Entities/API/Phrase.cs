using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.API
{
    public class Phrase
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Language { get; set; }

        public Phrase() { }

        public Phrase(string name, string value, string langauge)
        {
            Name = name;
            Value = value;
            Language = langauge;
        }
    }
}
