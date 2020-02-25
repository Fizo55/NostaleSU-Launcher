using System;
using System.IO;
using System.Xml;
using WowSuite.Launcher.Config;

namespace WowSuite.Launcher.Utils
{
    internal class XmlHelper
    {
        private string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppFolder.APP_DATA_FOLDER_NAME);
        private string conftemporaryFile = Path.Combine(LocalConfiguration.Instance.Files.ConfTempDataFile);
        private string configFile = Path.Combine(LocalConfiguration.Instance.Files.ConfDataFile);

        public string GetSettingValue(string key)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(this.configFile);
            foreach (XmlNode xmlNode in xmlDocument.SelectSingleNode("configuration/appSettings"))
            {
                if (xmlNode.Attributes["key"].Value == key)
                {
                    return xmlNode.Attributes["value"].Value;
                }
            }
            return "";
        }

        public void UpdateSettingValue(string key, string value)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(this.configFile);
            foreach (XmlNode xmlNode in xmlDocument.SelectSingleNode("configuration/appSettings"))
            {
                if (xmlNode.Attributes["key"].Value == key)
                {
                    xmlNode.Attributes["value"].Value = value;
                }
            }
            xmlDocument.Save(this.configFile);
        }
    }
}