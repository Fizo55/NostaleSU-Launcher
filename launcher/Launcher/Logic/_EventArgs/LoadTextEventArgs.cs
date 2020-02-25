using WowSuite.Utils;

namespace WowSuite.Launcher.Logic
{
    /// <summary>
    /// Данные загрузки в виде текста
    /// </summary>
    public class LoadTextEventArgs : LoadEventArgs<LoadingState, QueryResult<string>>
    {
        /// <summary>
        /// Инициализирует экземпляр класса данных загрузки новостей
        /// </summary>
        /// <param name="state">Состояние загрузки</param>
        /// <param name="result">Результат</param>
        public LoadTextEventArgs(LoadingState state, QueryResult<string> result)
            : base(state, result)
        {
        }
    }
}