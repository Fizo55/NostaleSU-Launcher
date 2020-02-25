using System;
using System.Runtime.CompilerServices;

namespace WowSuite.Utils.Logging
{    
    /// <summary>
    /// Предоставляет интерфейс для записи в лог
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Добавить текст в лог
        /// </summary>
        /// <param name="text">Текст сообщения</param>
        /// <param name="memberName">Имя члена класса, в котором вызван данный метод</param>
        /// <param name="sourceFilePath">Название файла исходного кода</param>
        /// <param name="sourceLineNumber">Номер строки в файле исходного кода</param>
        void AppendText(string text,
            [CallerMemberName]string memberName = "",
            [CallerFilePath]string sourceFilePath = "",
            [CallerLineNumber]int sourceLineNumber = 0);

        /// <summary>
        /// Добавить исключение в лог
        /// </summary>
        /// <param name="ex">Исключение</param>
        /// <param name="memberName">Имя члена класса, в котором вызван данный метод</param>
        /// <param name="sourceFilePath">Название файла исходного кода</param>
        /// <param name="sourceLineNumber">Номер строки в файле исходного кода</param>
        void AppendException(Exception ex, string memberName = "", string sourceFilePath = "", int sourceLineNumber = 0);
    }
}