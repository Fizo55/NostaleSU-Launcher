namespace Constants
{
    public static class Wow
    {
        public static class FileName
        {
            /// <summary>Название файла, в котором хранится список файлов для обновления</summary>
            public const string PATCH_FILE_NAME = "patches.txt";

            /// <summary>PID </summary>
            public const string PID_FILE_NAME = "pid.uls";

            /// <summary>Название файла с информацией о локализации</summary>
            public const string LANG_FILE_NAME = "lang";

            /// <summary>Exe file wow</summary>
            public const string WOW_GAME_EXE_NAME = "NostaleX.dat";

            public const string NOSXY_DAT_NAME = "nosxy.dat";

            public const string NOSTALE_EXE_NAME = "NostaleRun.exe";

            public const string NOSTALE_RU_TORRENT_NAME = "NosTale_UK_20140328.torrent";
            public const string NOSTALE_EN_TORRENT_NAME = "NosTale_EN_20140328.torrent";
        }

        public static class FolderName
        {
            /// <summary>
            /// Название временной папки для целей сохранения временных файлов,
            /// учавствующих в процессе обновления
            /// </summary>
            public const string TEMP_FOLDER_NAME = "WowUpdateTemp";

            /// <summary>Группирует константы, относящиеся к клиенту</summary>
            public static class Client
            {
                /// <summary>Folder Data</summary>
                public const string DATA_FOLDER_NAME = "NostaleData";

                /// <summary>localization folder</summary>
                public const string LOCALE_FOLDER_NAME = "enUS";
            }
        }


    }

}
