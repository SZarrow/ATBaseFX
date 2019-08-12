using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ATBase.Core;
using Newtonsoft.Json;

namespace ATBase.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public static class LogUtil
    {
        private const String indentChar = "   ";
        private const String guidlines = "|--- ";
        private const String exPrefix = "-";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="exIndex"></param>
        public static String FormatException(Exception ex, Int32 exIndex = 0)
        {
            if (ex == null)
            {
                return String.Empty;
            }

            StringBuilder sb = new StringBuilder();

            if (ex is AggregateException)
            {
                var ae = (ex as AggregateException).Flatten();
                if (ae.InnerExceptions.Count > 0)
                {
                    Int32 index = 0;
                    foreach (var ie in ae.InnerExceptions)
                    {
                        sb.AppendLine(FormatException(ie, index++));
                    }
                }

                return sb.ToString();
            }
            else
            {
                Int32 depth = 1;
                AppendTab(sb, depth);
                sb.AppendLine($"Exception[{exIndex}]：{ ex.Message}");

                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    depth++;
                    AppendTab(sb, depth);
                    sb.AppendLine($"InnerException：{innerException.Message}");

                    if (innerException.StackTrace != null)
                    {
                        depth++;
                        AppendTab(sb, depth);
                        sb.AppendLine("StackTrace：");

                        using (StringReader sr = new StringReader(innerException.StackTrace))
                        {
                            depth++;
                            while (sr.Peek() != -1)
                            {
                                AppendTab(sb, depth);
                                sb.AppendLine(Trim(sr.ReadLine()));
                            }
                        }
                    }

                    innerException = innerException.InnerException;
                }

                if (ex.StackTrace != null)
                {

                    depth = 1;
                    AppendTab(sb, depth);
                    sb.AppendLine("StackTrace：");

                    using (StringReader sr = new StringReader(ex.StackTrace))
                    {
                        depth++;

                        while (sr.Peek() != -1)
                        {
                            AppendTab(sb, depth);
                            sb.AppendLine(Trim(sr.ReadLine()));
                        }
                    }

                    sb.AppendLine();
                }

                return sb.ToString();
            }
        }

        public static String MiniFormatExceptions(Exception[] exceptions)
        {
            if (exceptions == null || exceptions.Length == 0) { return String.Empty; }

            MiniException[] miniExceptions = new MiniException[exceptions.Length];

            StringBuilder sb = new StringBuilder();
            MiniException miniEx;

            for (var i = 0; i < exceptions.Length; i++)
            {
                miniEx = ConvertToMiniExceptionFrom(exceptions[i]);

                sb.Append(exPrefix).AppendLine(miniEx.Message);

                if (miniEx.Stack.HasValue())
                {
                    using (var reader = new StringReader(miniEx.Stack))
                    {
                        while (reader.Peek() != -1)
                        {
                            sb.Append(exPrefix).AppendLine(Trim(reader.ReadLine()));
                        }
                    }
                }
                else
                {
                    sb.Append(exPrefix).AppendLine(Trim(miniEx.Source));
                }
            }

            return sb.ToString();
        }

        private static MiniException ConvertToMiniExceptionFrom(Exception ex)
        {
            if (ex == null)
            {
                return null;
            }

            var miniEx = new MiniException();

            if (ex.StackTrace.HasValue())
            {
                var match = Regex.Match(ex.StackTrace, @"\bat\s([^\s]+)\sin\s([^\s]+):line\s(\d+)");
                if (match.Success)
                {
                    miniEx.Source = match.Value;
                    miniEx.FilePath = match.Groups[2].Value;
                    miniEx.LineNumber = match.Groups[3].Value.ToInt32();
                }
                else
                {
                    miniEx.Stack = ex.StackTrace;
                }
            }

            var sb = new StringBuilder();
            sb.Append(ex.Message);
            while (ex.InnerException != null)
            {
                sb.Append(" -> ").Append(ex.InnerException.Message);
                ex = ex.InnerException;
            }
            miniEx.Message = sb.ToString();

            return miniEx;
        }

        private static void AppendTab(StringBuilder sb, Int32 depth)
        {
            for (var i = 1; i <= (depth - 1) * 2; i++) { sb.Append(indentChar); }
            sb.Append(guidlines);
        }

        private static String Trim(String str)
        {
            if (str.HasValue())
            {
                Int32 i = 0;
                while (i < str.Length)
                {
                    if (str[i] != ' ') { break; } else { i++; }
                }
                return str.Substring(i);
            }

            return String.Empty;
        }
    }

    internal class MiniException
    {
        public String Message { get; set; }
        public String FilePath { get; set; }
        public Int32 LineNumber { get; set; }
        public String Source { get; set; }
        public String Stack { get; set; }
    }
}
