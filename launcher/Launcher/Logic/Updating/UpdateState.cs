namespace WowSuite.Launcher.Logic.Updating
{
    /// <summary>
    /// Предоставляет набор значений, показывающих, в каком состоянии находится апдэйтер.
    /// </summary>
    public enum UpdateState
    {
        /// <summary>Ничего не делает</summary>
        None,

        /// <summary>Проверяет наличие обновлений</summary>
        Checking,

        /// <summary>Обновление запущено</summary>
        Started,

        /// <summary>Обновление не требуется</summary>
        NotNeeded,

        /// <summary>Обновление успешно завершено</summary>
        Completed
    }
}