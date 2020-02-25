using System;
using System.Collections.Generic;
using System.IO;
using WowSuite.Utils.Configuration;

namespace WowSuite.Launcher.Config
{
    internal class AppFolder : AppFolderBase<WowLauncherFolder>
    {
        public AppFolder()
        {
            string cache = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CACHE_FOLDER_NAME);

            //string appData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APP_DATA_FOLDER_NAME);
            //string logs = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APP_LOGS_FOLDER_NAME);

            string logs = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, APP_LOGS_FOLDER_NAME); // save to wow client/logs
            string appData = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, APP_DATA_FOLDER_NAME);//  save dir to wow client/

            _folders = new Dictionary<WowLauncherFolder, string>
            {
                { WowLauncherFolder.Cache, cache },
                { WowLauncherFolder.AppData, appData },
                { WowLauncherFolder.Logs, logs }
            };
        }

        public const string CACHE_FOLDER_NAME = "Cache";

        public const string APP_DATA_FOLDER_NAME = "NostaleLauncher";

        public const string APP_LOGS_FOLDER_NAME = "Logs";
    }
}