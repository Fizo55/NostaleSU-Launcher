using System.Runtime.InteropServices;
using System.Text;

namespace WowSuite.Launcher.Utils
{
    public class INI
    {
        public class Section
        {
            private string Name;

            private INI File;

            public string this[string Key]
            {
                get
                {
                    return this.File.IniReadValue(this.Name, Key);
                }
                set
                {
                    if (value != this[Key])
                    {
                        this.File.IniWriteValue(this.Name, Key, value);
                    }
                }
            }

            public Section(string name, INI file)
            {
                this.Name = name;
                this.File = file;
            }
        }

        private string _path;

        public INI.Section this[string Section]
        {
            get
            {
                return new INI.Section(Section, this);
            }
        }

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public INI(string Path)
        {
            this._path = Path;
        }

        public string GetIniPath()
        {
            return this._path;
        }

        protected void IniWriteValue(string Section, string Key, string Value)
        {
            INI.WritePrivateProfileString(Section, Key, Value, this._path);
        }

        protected string IniReadValue(string Section, string Key)
        {
            StringBuilder stringBuilder = new StringBuilder(255);
            int privateProfileString = INI.GetPrivateProfileString(Section, Key, "", stringBuilder, 255, this._path);
            return stringBuilder.ToString();
        }
    }
}