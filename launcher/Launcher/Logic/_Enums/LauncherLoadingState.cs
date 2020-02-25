namespace WowSuite.Launcher.Logic
{
    /// <summary>
    /// Хранит в себе константы, которые сообщают состояние загрузки
    /// </summary>
    public enum LoadingState
    {
        /// <summary>
        /// Отменено
        /// </summary>
        Canceled,

        /// <summary>
        /// Начато
        /// </summary>
        Started,

        /// <summary>
        /// Завершено
        /// </summary>
        Completed,
    }
}