using System;

namespace WOLF.Net.Enums.Misc
{
    [Flags]
    public enum Language
    {
        NOT_SPECIFIED = 0,
        [Obsolete("use NOT_SPECIFIED")]
        NotSpecified = NOT_SPECIFIED,

        ENGLISH = 1,
        [Obsolete("use ENGLISH")]
        English = ENGLISH,

        GERMAN = 3,
        [Obsolete("use GERMAN")]
        German = GERMAN,

        SPANISH = 4,
        [Obsolete("use SPANISH")]
        Spanish = SPANISH,

        FRENCH = 6,
        [Obsolete("use FRENCH")]
        French = FRENCH,

        POLISH = 10,
        [Obsolete("use POLISH")]
        Polish = POLISH,

        CHINESE_SIMPLIFIED = 11,
        [Obsolete("use CHINESE_SIMPLIFIED")]
        Chinese_Simplified = CHINESE_SIMPLIFIED,

        RUSSIAN = 12,
        [Obsolete("use RUSSIAN")]
        Russian = RUSSIAN,

        ITALIAN = 13,
        [Obsolete("use ITALIAN")]
        Italian = ITALIAN,

        ARABIC = 14,
        [Obsolete("use ARABIC")]
        Arabic = ARABIC,

        PERSIAN_FARSI = 15,
        [Obsolete("use PERSIAN_FARSI")]
        Persian_Farsi = PERSIAN_FARSI,

        GREEK = 16,
        [Obsolete("use GREEK")]
        Greek = GREEK,

        PORTUGUESE = 17,
        [Obsolete("use PORTUGUESE")]
        Portuguese = PORTUGUESE,

        HINDI = 18,
        [Obsolete("use HINDI")]
        Hindi = HINDI,

        JAPANESE = 19,
        [Obsolete("use JAPANESE")]
        Japanese = JAPANESE,

        LATIN_SPANISH = 20,
        [Obsolete("use LATIN_SPANISH")]
        Latin_Spanish = LATIN_SPANISH,

        SLOVAK = 21,
        [Obsolete("use SLOVAK")]
        Slovak = SLOVAK,

        CZECH = 22,
        [Obsolete("use CZECH")]
        Czech = CZECH,

        DANISH = 24,
        [Obsolete("use DANISH")]
        Danish = DANISH,

        FINNISH = 25,
        [Obsolete("use FINNISH")]
        Finnish = FINNISH,

        HUNGARIAN = 27,
        [Obsolete("use HUNGARIAN")]
        Hungarian = HUNGARIAN,

        BAHASA_INDONESIA = 28,
        [Obsolete("use BAHASA_INDONESIA")]
        Bahasa_Indonesia = BAHASA_INDONESIA,

        MALAY = 29,
        [Obsolete("use MALAY")]
        Malay = MALAY,

        DUTCH = 30,
        [Obsolete("use DUTCH")]
        Dutch = DUTCH,

        NORWEGIAN = 31,
        [Obsolete("use NORWEGIAN")]
        Norwegian = NORWEGIAN,

        SWEDISH = 32,
        [Obsolete("use SWEDISH")]
        Swedish = SWEDISH,

        THAI = 33,
        [Obsolete("use ")]
        Thai = THAI,

        TURKISH = 34,
        [Obsolete("use TURKISH")]
        Turkish = TURKISH,

        VIETNAMESE = 35,
        [Obsolete("use VIETNAMESE")]
        Vietnamese = VIETNAMESE,

        KOREAN = 36,
        [Obsolete("use KOREAN")]
        Korean = KOREAN,

        BRAZILIAN_PORTUGUESE = 37,
        [Obsolete("use BRAZILIAN_PORTUGUESE")]
        Brazilian_Portuguese = BRAZILIAN_PORTUGUESE,

        ESTONIAN = 39,
        [Obsolete("use ESTONIAN")]
        Estonian = ESTONIAN,

        KAZAKH = 41,
        [Obsolete("use KAZAKH")]
        Kazakh = KAZAKH,

        LATVIAN = 42,
        [Obsolete("use LATVIAN")]
        Latvian = LATVIAN,

        LITHUANIAN = 43,
        [Obsolete("use LITHUANIAN")]
        Lithuanian = LITHUANIAN,

        UKRAINIAN = 44,
        [Obsolete("use UKRAINIAN")]
        Ukrainian = UKRAINIAN,

        BULGARIAN = 45,
        [Obsolete("use BULGARIAN")]
        Bulgarian = BULGARIAN
    }
}