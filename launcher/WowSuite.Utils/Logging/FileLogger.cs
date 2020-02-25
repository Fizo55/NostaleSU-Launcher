using System;
using System.IO;
using WowSuite.Utils.Logging;

namespace WowLauncher.Utils
{
    /// <summary>
    /// Класс логирования. Пишет лог в текстовый файл.
    /// </summary>
    public class FileLogger : ILogger
    {
        /// <summary>Путь к папке содержащей логи приложений</summary>
        private readonly string _logPath;
        private static readonly object _syncObject = new object();
        private readonly string _fileName;
        private const string _separator = "________________________";

        /// <summary>
        /// Инициализирует экземпляр класса логирования.
        /// </summary>
        /// <param name="logPath">Путь к папке, в которую будут сохраняться логи</param>
        /// <param name="fileName">Название файла, которое будет использоваться при генерации актуального названия файла лога.</param>
        public FileLogger(string logPath, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName");

            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);

            _logPath = logPath;
            _fileName = fileName;

            GenerateLogFileName(fileName, DateTime.Now);
        }

        /// <summary>
        /// Полный путь к файлу конфигурации, который был открыт для записи последний раз.
        /// Каждую новую запись это свойство обновляется
        /// </summary>
        public string FullPathToLastWritedLogFile { get; private set; }

        /// <summary>
        /// Генерирует актуальное название файла лога в соответствии с частью названия и датой.
        /// </summary>
        /// <param name="fileName">Текст, который будет использоваться, как часть генерируемого названия файла.</param>
        /// <param name="now">Дата, которая будет использоваться, как часть генерируемого названия файла.</param>
        private void GenerateLogFileName(string fileName, DateTime now)
        {
            FullPathToLastWritedLogFile = Path.Combine(_logPath,
                //string.Format("{0} - {1}{2}", fileName, now.ToShortDateString(), ".txt"));
                string.Format("{0}", fileName, ".txt"));
        }

        /// <summary>
        /// Добавить текст сообщения в конец файла лога.
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        /// <param name="memberName">Имя члена класса, в котором вызван данный метод</param>
        /// <param name="sourceFilePath">Название файла исходного кода</param>
        /// <param name="sourceLineNumber">Номер строки в файле исходного кода</param>
        public void AppendText(string text, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0)
        {
            lock (_syncObject)
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

                GenerateLogFileName(_fileName, now);
                File.AppendAllText(FullPathToLastWritedLogFile, resultText);
            }
        }

        /// <summary>
        /// Добавить текст исключения в конец файла лога.
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