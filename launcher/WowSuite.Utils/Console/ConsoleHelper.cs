using System;

namespace WowSuite.Utils.Console
{
    public static class ConsoleHelper
    {
        static ConsoleHelper()
        {
            ErrorColor = ConsoleColor.Red;
        }

        /// <summary>
        /// Цвет сообщений, выводимых при ошибке.
        /// </summary>
        public static ConsoleColor ErrorColor { get; set; }

        public static void WriteLine(string text, ConsoleColor foregroundColor, params object[] args)
        {
            ConsoleColor defaultColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = foregroundColor;
            System.Console.WriteLine(text, args);
            System.Console.ForegroundColor = defaultColor;
        }

        public static void WriteLine(string text, ConsoleColor foregroundColor)
        {
            ConsoleColor defaultColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = foregroundColor;
            System.Console.WriteLine(text);
            System.Console.ForegroundColor = defaultColor;
        }

        public static void Write(string text, ConsoleColor foregroundColor, params object[] args)
        {
            ConsoleColor defaultColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = foregroundColor;
            System.Console.Write(text, args);
            System.Console.ForegroundColor = defaultColor;
        }

        public static void Write(string text, ConsoleColor foregroundColor)
        {
            ConsoleColor defaultColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = foregroundColor;
            System.Console.Write(text);
            System.Console.ForegroundColor = defaultColor;
        }

        public static void WriteError(string text)
        {
            Write(text, ErrorColor);
        }

        public static void WriteError(string text, params object[] args)
        {
            Write(text, ErrorColor, args);
        }

        public static void WriteErrorLine(string text)
        {
            WriteLine(text, ErrorColor);
        }

        public static void WriteErrorLine(string text, params object[] args)
        {
            WriteLine(text, ErrorColor, args);
        }

        public static void WriteSeparatorLine(char segment, ConsoleColor foregroundColor, string text = "")
        {
            ConsoleColor defaultColor = System.Console.ForegroundColor;
            System.Console.ForegroundColor = foregroundColor;
            WriteSeparatorLine(segment, text);
            System.Console.ForegroundColor = defaultColor;
        }

        public static void WriteSeparatorLine(char segment, string text = "")
        {
            string result = (text.Length > 0) ? (text + " ") : "";
            int iterations = (text.Length > 0) ? (80 - text.Length - 1) : 80;
            for (int i = 0; i < iterations; i++)
            {
                result += segment;
            }
            System.Console.WriteLine(result);
        }
    }
}
