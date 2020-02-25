using System;
using System.Linq;

namespace WowSuite.Utils.Patching
{
    /// <summary>
    /// Представляет информацию о патче.
    /// </summary>
    public class PatchInfo
    {
        /// <summary>
        /// Initialise la copie actuelle des informations de patch en analysant le texte formaté transmis.
        /// </summary>
        /// <param name="patchList">
        /// Форматированный текст, представляющий из себя 
        /// список информации о файлах для обновления</param>
        /// <exception cref="ArgumentNullException">Генерирует, если один из переданных параметров равен null</exception>
        public PatchInfo(string patchList)
        {
            if (patchList == null)
                throw new ArgumentNullException("patchList");

            string[] formattedLines = patchList.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
            UpdateFile[] updateFiles = new UpdateFile[formattedLines.Length];

            long sumOfLenght = 0L;
            for (int i = 0; i < formattedLines.Length; i++)
            {
                UpdateFile updateFile = UpdateFile.FromString(formattedLines[i]);
                updateFiles[i] = updateFile;
                sumOfLenght += updateFile.FileSize;
            }
            PatchLength = sumOfLenght;

            UpdateFiles = updateFiles;
        }

        /// <summary>
        /// Список файлов обновления из патч листа
        /// </summary>
        public UpdateFile[] UpdateFiles { get; private set; }

        /// <summary>
        /// Размер в байтах всех файлов в патче.
        /// </summary>
        public long PatchLength { get; set; }
    }
}
