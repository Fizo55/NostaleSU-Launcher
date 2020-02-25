using System;

namespace WowSuite.Launcher.Logic
{
    /// <summary>
    /// Данные загрузки
    /// </summary>
    public class LoadEventArgs<TState, TResult> : EventArgs
        where TState : struct //Подразумевается перечисление, отражающее состояние загрузки
        where TResult : class
    {
        /// <summary>
        /// Инициализирует экземпляр класса данных инициализации
        /// </summary>
        /// <param name="state">Состояние выполнения</param>
        /// <param name="result">Данные приложения</param>
        public LoadEventArgs(TState state, TResult result)
        {
            State = state;
            Result = result;
        }

        /// <summary>
        /// Состояние инициализации
        /// </summary>
        public TState State { get; protected set; }

        /// <summary>
        /// Данные, которые возвращаются в качестве результата
        /// </summary>
        public TResult Result { get; set; }
    }
}