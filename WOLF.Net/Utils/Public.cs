using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WOLF.Net.Enums.Misc;

public static class Public
{

    /// <summary>
    /// Convert a language to phrase language
    /// </summary>
    /// <param name="language"></param>
    /// <returns></returns>
    public static string ToPhraseLanguage(this Language language) => language switch
    {
        Language.ARABIC => "ar",
        Language.BAHASA_INDONESIA => "bin",
        Language.BRAZILIAN_PORTUGUESE => "brpt",
        Language.BULGARIAN => "bu",
        Language.CHINESE_SIMPLIFIED => "ch",
        Language.CZECH => "cz",
        Language.DANISH => "da",
        Language.DUTCH => "du",
        Language.ENGLISH => "en",
        Language.ESTONIAN => "est",
        Language.FINNISH => "fi",
        Language.FRENCH => "fr",
        Language.GERMAN => "ge",
        Language.GREEK => "gr",
        Language.HINDI => "hi",
        Language.HUNGARIAN => "hu",
        Language.ITALIAN => "it",
        Language.JAPANESE => "ja",
        Language.KAZAKH => "ka",
        Language.KOREAN => "ko",
        Language.LATIN_SPANISH => "les",
        Language.LATVIAN => "la",
        Language.LITHUANIAN => "li",
        Language.MALAY => "ma",
        Language.NORWEGIAN => "no",
        Language.PERSIAN_FARSI => "fa",
        Language.POLISH => "po",
        Language.PORTUGUESE => "pt",
        Language.RUSSIAN => "ru",
        Language.SLOVAK => "sl",
        Language.SPANISH => "es",
        Language.SWEDISH => "sv",
        Language.THAI => "th",
        Language.TURKISH => "tr",
        Language.UKRAINIAN => "uk",
        Language.VIETNAMESE => "vi",
        _ => "en"
    };

    /// <summary>
    /// Chunk a list into a lists of chunkSize
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="chunkSize"></param>
    /// <returns>List<List<T>></returns>
    public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize = 8) => source.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / chunkSize).Select(x => x.Select(v => v.Value).ToList()).ToList();

    /// <summary>
    /// Check to see if two strings are the same
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns>bool</returns>
    public static bool IsEqual(this string key, string value) => key != null && value != null && key.Trim().ToLower() == value.Trim().ToLower();

    internal static string ToMD5(this string input)
    {
        using MD5 hash = MD5.Create();
        byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        StringBuilder sBuilder = new 
            StringBuilder();

        for (int i = 0; i < data.Length; i++)
            sBuilder.Append(data[i].ToString("x2"));

        return sBuilder.ToString();
    }

    /// <summary>
    /// Get an image from url
    /// </summary>
    /// <param name="url"></param>
    /// <returns>Bitmap</returns>
    public static async Task<Bitmap> DownloadImageFromUrl(this string url)
    {
        var tsk = new TaskCompletionSource<Bitmap>();
        try
        {
            using var webclient = new WebClient();
            tsk.SetResult((Bitmap)Image.FromStream(new MemoryStream(webclient.DownloadData(url))));
        }
        catch (Exception d)
        {
            tsk.SetException(d);
        }

        return await tsk.Task;
    }

    /// <summary>
    /// Convert an image to bytes
    /// </summary>
    /// <param name="image"></param>
    /// <returns>byte[]</returns>
    public static byte[] ToBytes(this Bitmap image)
    {
        using MemoryStream m = new MemoryStream();

        image.Save(m, ImageFormat.Jpeg);

        return m.ToArray();
    }

    /// <summary>
    /// Split a string into chunks of a specific length
    /// </summary>
    /// <param name="text"></param>
    /// <param name="max"></param>
    /// <param name="splitChar"></param>
    /// <param name="joinChar"></param>
    /// <returns></returns>
    public static List<string> BatchString(this string text, int max, string splitChar = "\n", string joinChar = "\n")
    {
        if (text.Length <= max)
            return new List<string>() { text };

        var charCount = 0;
        var lines = text.Split(new[] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
        return lines.GroupBy(w => (charCount += (((charCount % max) + w.Length + 1 >= max)
                        ? max - (charCount % max) : 0) + w.Length + 1) / max)
                    .Select(g => string.Join(joinChar, g.ToArray()))
                    .ToList();
    }

    /// <summary>
    /// Convert all numbers in a string to english
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ToEnglishNumbers(this string input)
    {
        string EnglishNumbers = "";

        for (int i = 0; i < input.Length; i++)
            if (char.IsDigit(input[i]))
                EnglishNumbers += char.GetNumericValue(input, i);
            else
                EnglishNumbers += input[i].ToString();

        return EnglishNumbers;
    }

    /// <summary>
    /// Convert all numbers in a string to arabic
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string ToArabicNumbers(this string input)
    {
        return input.Replace('0', '\u0660')
                .Replace('1', '\u0661')
                .Replace('2', '\u0662')
                .Replace('3', '\u0663')
                .Replace('4', '\u0664')
                .Replace('5', '\u0665')
                .Replace('6', '\u0666')
                .Replace('7', '\u0667')
                .Replace('8', '\u0668')
                .Replace('9', '\u0669');
    }

    /// <summary>
    /// Convert all numbers in a string to arabic
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// 
    public static string ToArabicNumbers(this int input) => input.ToString().ToArabicNumbers();

    /// <summary>
    /// Remove all ads from a string
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string TrimAds(this string input)
    {
        if (input.StartsWith("[") && input.EndsWith("]"))
            input = input.TrimStart('[').TrimEnd(']');

        return Regex.Replace(input, @"\[.*?\]", string.Empty);
    }

    /// <summary>
    /// Timeout a task after specified milliseconds
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="task"></param>
    /// <param name="timeoutInMilliseconds"></param>
    /// <returns></returns>
    public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, double timeoutInMilliseconds = 60000)
    {
        if (timeoutInMilliseconds == Timeout.Infinite)
            return await task;

        else if (task == await Task.WhenAny(task, Task.Delay((int)timeoutInMilliseconds)))
            return await task;
        else
            throw new TimeoutException();

    }

    /// <summary>
    /// Timeout a task after specified milliseconds
    /// </summary>
    /// <param name="task"></param>
    /// <param name="timeoutInMilliseconds"></param>
    /// <returns></returns>
    public static async Task TimeoutAfter(this Task task, double timeoutInMilliseconds = 60000)
    {
        if (timeoutInMilliseconds == Timeout.Infinite)
            await task;
        else if (task == await Task.WhenAny(task, Task.Delay((int)timeoutInMilliseconds)))
            await task;
        else
            throw new TimeoutException();
    }
}