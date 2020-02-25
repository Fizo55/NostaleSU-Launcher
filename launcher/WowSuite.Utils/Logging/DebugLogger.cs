using System;
using System.Diagnostics;

namespace WowSuite.Utils.Logging
{
    public class DebugLogger : ILogger
    {        
        private static readonly object _lockerObject = new object();
        private const string _separator = "=========================";

        /// <summary>
        /// Добавить текст в конец файла лога, добавив в начале дату и время с точностью до миллесекунд в начале текста
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        /// <param name="memberName">Имя члена класса, в котором вызван данный метод</param>
        /// <param name="sourceFilePath">Название файла исходного кода</param>
        /// <param name="sourceLineNumber">Номер строки в файле исходного кода</param>
        public void AppendText(string text, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            lock (_lockerObject)
            {
                DateTime now = DateTime.Now;
                string resultText = string.Format("{0} {1}: {3}Method: {5}. String: {7}.{3}File: {6}{3}Message: {2}{3}{4}{3}{3}",
                    now.ToShortDateString(),
                    now.TimeOfDay,
                    text,
                    Environment.NewLine,
                    _separator,
                    memberName,
                    sourceFilePath,
                    sourceLineNumber);

                Debug.Write(resultText);
            }
        }

        /// <summary>
        /// Добавить исключение
        /// </summary>
        /// <param name="ex">Исключение</param>
        /// <param name="memberName">Имя члена класса, в котором вызван данный метод</param>
        /// <param name="sourceFilePath">Название файла исходного кода</param>
        /// <param name="sourceLineNumber">Номер строки в файле исходного кода</param>
        public void AppendException(Exception ex, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            string text = string.Format(
                "Message: {1};{0}" +
                "Source: {2};{0}" +
                "TargetSite: {3}{0}" +
                "InnerException: {4};{0}" +
                "StackTrace: {0}{5};"
                , Environment.NewLine, ex.Message, ex.Source, ex.TargetSite, ex.InnerException, ex.StackTrace);
            AppendText(text, memberName, sourceFilePath, sourceLineNumber);
        }
    }
}
