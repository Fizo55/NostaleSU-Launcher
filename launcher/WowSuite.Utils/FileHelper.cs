using System;
using System.IO;
using System.Linq;

namespace WowSuite.Utils
{
    //ToDO: Create a method of reading text from a stream , this method will be used in the current method of reading from a file. It will open the stream and call the method . to test its performance in unit tests.
    public static class FileHelper
    {
        public static class Txt 
        {        
            /// <summary>
            /// Прочитать первую строку из файла.
            /// </summary>
            /// <param name="pathToFile">Путиь к файлу</param>
            /// <returns></returns>
            public static string ReadFirstLine(string pathToFile)
            {
                //читаем строку и удаляем все пробелы, переносы на новую строку и символ возврата каретки
                string text = string.Empty;
                using (StreamReader reader = File.OpenText(pathToFile))
                {
                    string line = reader.ReadLine();
                    if (line != null) //ToDO: Make safe rasparsivanie checking of number of elements in the array
                        text = line.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).First();
                }
                return text;
            }
        }
    }
}
