using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace BSU.Utils.Data
{
    public static class DataSerializer<T>
    {
        /// <summary>
        /// Сохранить данные
        /// </summary>
        /// <param name="data">Список экземпляров заданного типа для сохранения</param>
        public static string SaveAsText(List<T> data)
        {
            var serializer = new XmlSerializer(typeof(List<T>));
            using (var memStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memStream, Encoding.UTF8))
                {
                    serializer.Serialize(writer, data);
                }

                var xml = Encoding.UTF8.GetString(memStream.GetBuffer());
                xml = xml.Substring(xml.IndexOf(Convert.ToChar(60)));
                xml = xml.Substring(0, (xml.LastIndexOf(Convert.ToChar(62)) + 1));
                return xml;
            }
        }

        /// <summary>
        /// Сохранить данные
        /// </summary>
        /// <param name="data">Список экземпляров заданного типа для сохранения</param>
        /// <param name="fileName">Путь к файлу, в который будут сохранены данные</param>
        public static void Save(List<T> data, string fileName)
        {
            var serializer = new XmlSerializer(typeof(List<T>));
            var fs = new FileStream(fileName, FileMode.Create);
            using (var streamWriter = new StreamWriter(fs))
            {
                serializer.Serialize(streamWriter, data);
            }
        }

        /// <summary>
        /// Сохранить данные
        /// </summary>
        /// <param name="data">Список экземпляров заданного типа для сохранения</param>
        /// <param name="fileName">Путь к файлу, в который будут сохранены данные</param>
        public static void SaveObject(T data, string fileName)
        {
            var serializer = new XmlSerializer(typeof(T));
            var fs = new FileStream(fileName, FileMode.Create);
            using (var streamWriter = new StreamWriter(fs))
            {
                serializer.Serialize(streamWriter, data);
            }
        }

        /// <summary>
        /// Загрузить данные из файла
        /// </summary>
        /// <param name="fileName">Путь к файлу</param>
        /// <returns>Список экземпляров заданного типа</returns>
        public static T LoadObject(string fileName)
        {
            var serializer = new XmlSerializer(typeof(T));
            var fs = new FileStream(fileName, FileMode.Open);
            using (var streamReader = new StreamReader(fs))
            {
                return (T)serializer.Deserialize(streamReader);
            }
        }

        /// <summary>
        /// Загрузить данные из файла
        /// </summary>
        /// <param name="fileName">Путь к файлу</param>
        /// <returns>Список экземпляров заданного типа</returns>
        public static List<T> Load(string fileName)
        {
            var serializer = new XmlSerializer(typeof(List<T>));
            var fs = new FileStream(fileName, FileMode.Open);
            using (var streamReader = new StreamReader(fs))
            {
                return (List<T>)serializer.Deserialize(streamReader);
            }
        }

        /// <summary>
        /// Десериализовать данные из XML в виде текста.
        /// </summary>
        /// <param name="xmlText"></param>
        /// <returns></returns>
        public static T LoadFromXml(string xmlText)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(xmlText))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}
