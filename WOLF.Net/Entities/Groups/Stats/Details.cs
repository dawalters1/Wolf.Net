using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace WOLF.Net.Entities.Groups.Stats
{
    public class Details
    {
        [JsonProperty("actionCount")]
        public int ActionCount { get;  set; }

        [JsonProperty("emoticonCount")]
        public int EmoticonCount { get;  set; }

        [JsonProperty("happyCount")]
        public int HappyCount { get;  set; }

        [JsonProperty("imageCount")]
        public int ImageCount { get;  set; }

        [JsonProperty("lineCount")]
        public int LineCount { get;  set; }

        [JsonProperty("message")]
        public string Message { get;  set; }

        [JsonProperty("nickname")]
        public string Nickname { get;  set; }

        [JsonProperty("packCount")]
        public int PackCount { get;  set; }

        [JsonProperty("questionCount")]
        public int QuestionCount { get;  set; }

        [JsonProperty("randomQuote")]
        public string RandomQuote { get;  set; }

        [JsonProperty("sadCount")]
        public int SadCount { get;  set; }

        [JsonProperty("subId")]
        public int SubId { get;  set; }

        [JsonProperty("swearCount")]
        public int SwearCount { get;  set; }

        [JsonProperty("textCount")]
        public int TextCount { get;  set; }

        [JsonProperty("voiceCount")]
        public int VoiceCount { get;  set; }

        [JsonProperty("wordCount")]
        public int WordCount { get;  set; }
    }
}
