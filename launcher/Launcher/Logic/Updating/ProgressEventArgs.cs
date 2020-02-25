using System;

namespace WowSuite.Launcher.Logic.Updating
{
    /// <summary>
    /// Аргументы события изменения прогресса выполнения какого-либо действия.
    /// </summary>
    public class ProgressEventArgs : EventArgs
    {
        public ProgressEventArgs(int progress)
        {
            Progress = progress;
        }

        public int Progress { get; protected set; }
    }

    public class InfoEventArgs : EventArgs
    {
        public InfoEventArgs(string name, float size, float remainingSize)
        {
            Name = name;
            Size = size;
            RemainingSize = remainingSize;
        }

        /// <summary>
        /// Прогресс выполнения какой-либо операции
        /// </summary>

        public string Name { get; protected set; }
        public float Size { get; protected set; }
        public float RemainingSize { get; protected set; }
    }
}