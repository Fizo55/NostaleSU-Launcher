using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;

namespace WowSuite.Utils.News
{
    /// <summary>
    /// Экспресс новость
    /// </summary>
    [DebuggerDisplay("Realms = {Realms}")]
    public class ExpressNewsItem
    {
        /// <summary>Заголовок</summary>
        [XmlElement("NewsTitle")]
        public string Title { get; set; }

        /// <summary>Тело сообщения</summary>
        [XmlElement("Text")]
        public string Text { get; set; }

        [XmlElement("Realms")]
        public string Realms { get; set; }

        [XmlElement("Date")]
        public string Date { get; set; }

        /// <summary>Ссылка на изображение</summary>
        public string ImageLink { get; set; }

        /// <summary>Ссылка новость</summary>
        public string NewsLink { get; set; }
                
        [XmlIgnore]
        public string RealmsFriendly { get; set; }
    }
}
