namespace WowSuite.Launcher.Config
{
    public class AuthData
    {
        public AuthData()
            : this(string.Empty, string.Empty)
        {
        }

        public AuthData(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class LocData
    {
        public LocData()
            : this(string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        public LocData(string location1, string location2, string location3, string location4)
        {
            Location1 = location1;
            Location2 = location2;
            Location3 = location3;
            Location4 = location4;
        }

        public string Location1 { get; set; }
        public string Location2 { get; set; }
        public string Location3 { get; set; }
        public string Location4 { get; set; }
    }
}