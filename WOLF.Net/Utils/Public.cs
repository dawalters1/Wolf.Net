﻿using System;
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
using WOLF.Net.Entities.Subscribers;
using WOLF.Net.Enums.Subscribers;

public static class Public
{
    public static List<List<T>> ChunkBy<T>(this List<T> source, int chunkSize = 8) => source.Select((x, i) => new { Index = i, Value = x }).GroupBy(x => x.Index / chunkSize).Select(x => x.Select(v => v.Value).ToList()).ToList();

    public static bool IsEqual(this string key, string value) => key != null && value != null && key.Trim().ToLower() == value.Trim().ToLower();
    
    internal static string ToMD5(this string input)
    {
        using MD5 hash = MD5.Create();
        byte[] data = hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        StringBuilder sBuilder = new StringBuilder();

        for (int i = 0; i < data.Length; i++)
            sBuilder.Append(data[i].ToString("x2"));

        return sBuilder.ToString();
    }

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

    public static byte[] ToBytes(this Bitmap image)
    {
        using MemoryStream m = new MemoryStream();

        image.Save(m, ImageFormat.Jpeg);

        return m.ToArray();
    }

    public static List<string> BatchString(this string text, int max, string splitChar = "\n", string joinChar = "\n")
    {
        var charCount = 0;
        var lines = text.Split(new[] { splitChar }, StringSplitOptions.RemoveEmptyEntries);
        return lines.GroupBy(w => (charCount += (((charCount % max) + w.Length + 1 >= max)
                        ? max - (charCount % max) : 0) + w.Length + 1) / max)
                    .Select(g => string.Join(joinChar, g.ToArray()))
                    .ToList();
    }

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

    public static string ToArabicNumbers(this int input) =>  input.ToString().ToArabicNumbers();

    public static string TrimAds(this string nickname)
    {
        if (nickname.StartsWith("[") && nickname.EndsWith("]"))
            nickname = nickname.TrimStart('[').TrimEnd(']');

        return Regex.Replace(nickname, @"\[.*?\]", string.Empty);
    }

    public static async Task<TResult> TimeoutAfter<TResult>(this Task<TResult> task, double timeoutInMilliseconds = 60000)
    {
        if (timeoutInMilliseconds == Timeout.Infinite)
            return await task;

        else if (task == await Task.WhenAny(task, Task.Delay((int)timeoutInMilliseconds)))
            return await task;
        else
            throw new TimeoutException();

    }

    public static async Task TimeoutAfter(this Task task, double timeoutInMilliseconds = 60000)
    {
        if (timeoutInMilliseconds == Timeout.Infinite)
            await task;
        else if (task == await Task.WhenAny(task, Task.Delay((int)timeoutInMilliseconds)))
            await task;
        else
            throw new TimeoutException();
    }

    private static readonly Privilege[] privileges = { Privilege.BOT, Privilege.STAFF, Privilege.ELITECLUB_1, Privilege.ELITECLUB_2, Privilege.ELITECLUB_3, Privilege.SELECTCLUB_1, Privilege.SELECTCLUB_2, Privilege.ENTERTAINER, Privilege.VOLUNTEER };

    public static Privilege GetTag(this Subscriber subscriber) => privileges.Any(r => subscriber.Privileges.HasFlag(r)) ? privileges.FirstOrDefault(r => subscriber.Privileges.HasFlag(r)) : Privilege.SUBSCRIBER;
}