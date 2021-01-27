using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.API
{
    public class Response
    {
        public int Code { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public bool Success => Code >= 200 && Code <= 299;
    }

    public class Response<T>
    {
        public int Code { get; set; }

        public T Body { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public bool Success => Code >= 200 && Code <= 299;
    }
}
