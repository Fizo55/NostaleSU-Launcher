using System;

namespace WowSuite.Launcher.Logic.Updating
{
    /// <summary>
    /// Аргументы события изменения состояния процесса обновления.
    /// </summary>
    public class UpdateStateEventArgs : EventArgs
    {
        public UpdateStateEventArgs(UpdateState newState, UpdateState oldState)
        {
            NewState = newState;
            OldState = oldState;
        }

        /// <summary>
        /// Новое состояние
        /// </summary>
        public UpdateState NewState { get; protected set; }

        /// <summary>
        /// Старое состояние
        /// </summary>
        public UpdateState OldState { get; protected set; }
    }
}