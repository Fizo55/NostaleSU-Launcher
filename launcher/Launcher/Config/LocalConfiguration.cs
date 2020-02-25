namespace WowSuite.Launcher.Config
{
    /// <summary>
    /// Взаимодействие с локальной конфигурацией программы
    /// </summary>
    internal class LocalConfiguration
    {
        static LocalConfiguration()
        {
            Instance = new LocalConfiguration();
        }

        private LocalConfiguration()
        {
            Folders = new AppFolder();
            Files = new AppFiles(Folders);
        }

        /// <summary>
        /// Возвращает единственный экземпляр объекта
        /// </summary>
        public static LocalConfiguration Instance { get; protected set; }

        /// <summary>
        /// Возвращает набор путей к папкам для приложения
        /// </summary>
        public AppFolder Folders { get; protected set; }

        /// <summary>
        /// Возвращает набор путей к файлам для приложения
        /// </summary>
        public AppFiles Files { get; protected set; }
    }
}