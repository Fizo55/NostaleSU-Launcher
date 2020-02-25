using Constants;
using System;
using System.IO;
using System.Linq;

namespace WowSuite.Utils.Patching
{
    /// <summary>
    /// Представляет описание файла обновления.
    /// </summary>
    public class UpdateFile
    {
        /// <summary>
        /// Создать экземпляр класса.
        /// </summary>
        /// <exception cref="ArgumentNullException">Генерируется, если переданная строка имеет значение null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Генерируется, если переданный размер файла в байтах меньше нуля</exception>
        /// <param name="fileName"></param>
        /// <param name="hash"></param>
        /// <param name="fileSize"></param>
        public UpdateFile(string fileName, string hash, long fileSize)
        {

            if (fileName == null)
                throw new ArgumentNullException("fileName");

            if (hash == null)
                throw new ArgumentNullException("hash");
            if (fileSize < 0L)
                throw new ArgumentOutOfRangeException("fileSize");
            FileName = fileName;

            Hash = hash;
            FileSize = fileSize;
        }


        /// <summary>Название файла</summary>

        public string FileName { get; private set; }

        /// <summary>Хэш код</summary>
        public string Hash { get; private set; }

        /// <summary>Размер файла в байтах</summary>
        public long FileSize { get; private set; }

        /// <summary>
        /// Получить информацию о файле апдейта из строки в специальном формате.
        /// </summary>
        /// <param name="line"></param>
        /// <exception cref="FormatException">Генерируется, если переданная строка имеет неверный формат</exception>
        /// <exception cref="ArgumentNullException">Генерируется, если переданная строка имеет значение null</exception>
        /// <exception cref="OverflowException">Генерируется, если неудаётся распарсить размер файла в байтах</exception>
        /// <returns></returns>
        public static UpdateFile FromString(string line)
        {

            var i1 = line.LastIndexOf(' ');
            var size = long.Parse(line.Substring(i1 + 1));
            var i2 = line.LastIndexOf(' ', i1 - 1);
            var hash = line.Substring(i2 + 1, i1 - i2);
            var name = line.Substring(0, i2);



            //if (line == null)
            //    throw new ArgumentNullException("line");

            //if (line.StartsWith(" "))
            //    line = line.TrimStart();

            //if (line.EndsWith(" "))
            //    line = line.TrimEnd();

            //string[] segments = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //if (segments == null || segments.Length != 3)
            //{
            //    throw new FormatException("Переданная строка имеет неверный формат");
            //}

            //long fileSize = long.Parse(segments[2]);
            return new UpdateFile(name, hash, size);
        }

        /// <summary>
        /// Создать экземпляр класса распарсив файл.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="localeFolderName">Название папки локализации программы</param>
        /// <exception cref="FileNotFoundException">Генерируется, если файл не найден</exception>
        /// <exception cref="ArgumentNullException">Генерируется, если переданная строка имеет значение null</exception>
        /// <returns></returns>
        public static UpdateFile FromFile(string file, string localeFolderName)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (file == null)
                throw new ArgumentNullException("localeFolderName");

            var fileInfo = new FileInfo(file);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("Файл: \"{0}\" не найден", Path.GetFileName(file));
            }

            //string filePath = file.Split(Path.DirectorySeparatorChar).Last().Split('\\')[0];
            string[] filePathSegments = file.Split(Path.DirectorySeparatorChar);
            //string filePath = filePathSegments[filePathSegments.Length - 1];
            string fileName = filePathSegments[filePathSegments.Length - 1];

            //string filePath = filePathSegments[filePathSegments.Length + 1];

            var updateFile = new UpdateFile(fileName, HashHelper.GetMD5HashOfFile(file), fileInfo.Length);
            return updateFile;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", FileName, Hash, FileSize);
        }
    }
}
