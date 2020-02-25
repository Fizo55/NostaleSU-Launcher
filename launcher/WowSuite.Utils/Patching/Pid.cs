using System;
using System.Linq;

namespace WowSuite.Utils.Patching
{
    public class Pid
    {
        private Pid(string hash)
        {
            if (hash == null)
                throw new ArgumentNullException("hash");

            Hash = hash;
        }

        /// <summary>
        /// Получить Pid из строки имеющей определённый формат.
        /// </summary>
        /// <param name="formattedText">Специально форматированная строка</param>
        /// <returns></returns>
        public static Pid FromString(string formattedText)
        {
            if (formattedText == null)
                throw new ArgumentNullException("formattedText");

            string[] linesFromPid = formattedText.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            string pidHash = linesFromPid.First();

            return new Pid(pidHash);
        }

        /// <summary>
        /// Получить Pid из номера версии.
        /// </summary>
        /// <param name="version">Номер версии</param>
        /// <returns></returns>
        public static Pid FromVersionNumber(int version)
        {
            return new Pid(HashHelper.GetMD5Hash(version.ToString()));
        }

        /// <summary>
        /// Получить Pid из специально форматированного текстового файла.
        /// </summary>
        /// <param name="pidFile">Файл с хэшем</param>
        /// <returns></returns>
        public static Pid FromTextFile(string pidFile)
        {
            string localPidHash = FileHelper.Txt.ReadFirstLine(pidFile);
            return new Pid(localPidHash);
        }

        /// <summary>
        /// Хэш код версии обновления
        /// </summary>
        public string Hash { get; private set; }

        public override string ToString()
        {
            return Hash;
        }
    }
}
