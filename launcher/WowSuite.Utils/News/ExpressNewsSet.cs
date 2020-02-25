using System;
using System.Xml.Serialization;
using BSU.Utils.Data;
using System.Collections.Generic;


namespace WowSuite.Utils.News
{
    [XmlRoot("NewsRoot")]
    public class ExpressNewsSet
    {
        [XmlArray("ExpressNews")]
        [XmlArrayItem("NewsItem")]
        public ExpressNewsItem[] ExpressNews { get; set; }

        public static ExpressNewsSet FromXml(string xmlText)
        {
            if (xmlText == null)
                throw new ArgumentNullException("xmlText");

            var set = DataSerializer<ExpressNewsSet>.LoadFromXml(xmlText);

            return set;
        }
        
        public void Update(IDictionary<string, string> correlations)
        {
            if (correlations != null)
            {
                foreach (ExpressNewsItem item in ExpressNews)
                {
                    if (!string.IsNullOrEmpty(item.Realms) && correlations.ContainsKey(item.Realms))
                    {
                        item.RealmsFriendly = correlations[item.Realms];
                    }
                    else
                    {
                        item.RealmsFriendly = item.Realms;
                    }
                }
            }
            else
            {
                foreach (ExpressNewsItem item in ExpressNews)
                {
                    item.RealmsFriendly = string.Empty;
                }
            }
        }
    }
}