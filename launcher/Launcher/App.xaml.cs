using Constants;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;
using WowLauncher.Utils;
using WowSuite.Language;
using WowSuite.Launcher.Config;
using WowSuite.Launcher.Utils;

namespace WowSuite.Launcher
{
    public partial class App : Application
    {
        private string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppFolder.APP_DATA_FOLDER_NAME);
        private string conftemporaryFile = Path.Combine(LocalConfiguration.Instance.Files.ConfTempDataFile);
        private string configFile = Path.Combine(LocalConfiguration.Instance.Files.ConfDataFile);
        private System.Threading.Mutex mut;

        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            if (!File.Exists(this.configFile))
            {
                this.CreateFileConfiguration();
                Logger.Current.AppendText("Создание файла конфигурации");
            }

            ResourceProvider.Instance.TextResourcesSet = new TextResourceSet1();
            string lang = GetSettingValue("app_language");
            Languages language = (Languages)Enum.Parse(typeof(Languages), lang);
            ResourceProvider.Instance.Languages = language;

            Logger.Current = new FileLogger(
                LocalConfiguration.Instance.Folders.GetPath(WowLauncherFolder.Logs), "NostaleLauncherLog.txt");

            await LocalConfiguration.Instance.Folders.CheckFoldersAsync();
            Logger.Current.AppendText("Запуск приложения");

            string runCopyApp = GetSettingValue("run_copy_app");
            bool createdNew;
            string mutName = "Launcher";
            mut = new System.Threading.Mutex(true, mutName, out createdNew);
            if (!createdNew && runCopyApp == "false")
            {
                Shutdown();
                Logger.Current.AppendText("Обнаружена попытка запуска копии приложения");
            }
        }

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

        private void CreateFileConfiguration()
        {
            Directory.CreateDirectory(this.configPath);
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                Encoding = Encoding.UTF8
            };
            using (XmlWriter xmlWriter = XmlWriter.Create(this.conftemporaryFile, settings))
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement("configuration");
                xmlWriter.WriteStartElement("appSettings");

                xmlWriter.WriteStartElement("add");
                xmlWriter.WriteAttributeString("key", "run_copy_app");
                xmlWriter.WriteAttributeString("value", "false");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("add");
                xmlWriter.WriteAttributeString("key", "user_login");
                xmlWriter.WriteAttributeString("value", "");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("add");
                xmlWriter.WriteAttributeString("key", "user_password");
                xmlWriter.WriteAttributeString("value", "");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("add");
                xmlWriter.WriteAttributeString("key", "app_language");
                xmlWriter.WriteAttributeString("value", Wow.FolderName.Client.LOCALE_FOLDER_NAME);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("add");
                xmlWriter.WriteAttributeString("key", "realm1_client_location");
                xmlWriter.WriteAttributeString("value", "");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("add");
                xmlWriter.WriteAttributeString("key", "client_lang");
                xmlWriter.WriteAttributeString("value", "en");
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
            string str = "Nostale.Default";
            FileInfo fileInfo = new FileInfo(this.conftemporaryFile);
            string destFileName = Path.Combine(fileInfo.Directory.FullName, str + fileInfo.Extension);
            fileInfo.MoveTo(destFileName);
            Logger.Current.AppendText("Файл конфигурации успешно создан");
        }
    }
}