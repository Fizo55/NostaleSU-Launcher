using WowSuite.Utils.Logging;

namespace WowSuite.Launcher.Utils
{
    /// <summary>
    /// Предоставляет доступ к логеру. Позволяет задать другой логер.
    /// </summary>
    public class Logger
    {
        private static ILogger _logger;

        static Logger()
        {
            _logger = new DebugLogger();
        }

        /// <summary>
        /// Возвращает текущий логер приложения.
        /// </summary>
        public static ILogger Current
        {
            get { return _logger; }
            set { _logger = value; }
        }
    }
}