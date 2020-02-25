using DiscordRPC;
using DiscordRPC.Logging;
using Samples;
using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Net;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WowSuite.Language;
using WowSuite.Launcher.Config;
using WowSuite.Launcher.Properties;
using WowSuite.Launcher.Utils;

namespace WowSuite.Launcher
{
    public partial class LoginForm : Window
    {
        private readonly WebClient _web;
        private readonly WebClientFactory _webClientFactory;
        private readonly XmlHelper _xmlhelper;
        public DiscordRpcClient client;

        public LoginForm()
        {
            InitializeComponent();
            Initialize();

            initIndicator.Visibility = Visibility.Hidden;
            _webClientFactory = new WebClientFactory();
            _xmlhelper = new XmlHelper();
            _web = _webClientFactory.Create();
        }

        #region RPC
        private void Initialize()
        {
            client = new DiscordRpcClient("645517082431062016")
            {
                Logger = new ConsoleLogger() { Level = LogLevel.Warning }
            };

            //Subscribe to events
            client.OnReady += (sender, e) =>
            {
                //Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                //Console.WriteLine("Received Update! {0}", e.Presence);
            };

            //Connect to the RPC
            client?.Initialize();

            //Set the rich presence
            //Call this as many times as you want and anywhere in your code.
            client?.SetPresence(new RichPresence()
            {
                Details = "Nostale PServer",
                State = "Ancelloan",
                Assets = new Assets()
                {
                    LargeImageKey = "ancelloan",
                    LargeImageText = "Ancelloan.eu"
                }
            });

            var timer = new System.Timers.Timer(150);
            timer.Elapsed += (sender, args) => { client?.Invoke(); };
            timer.Start();
        }
        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Translate();
            LoadAccount();
            IsLauncherBanned();
        }

        //Called when your application terminates.
        //For example, just after your main loop, on OnDisable for unity.
        void Deinitialize()
        {
            client.Dispose();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
            Deinitialize();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public void StartWaitAnimation()
        {
            initIndicator.Visibility = Visibility.Visible;
        }

        public void StopWaitAnimation()
        {
            initIndicator.Visibility = Visibility.Hidden;
        }

        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            ResourceProvider resource = ResourceProvider.Instance;

            string login = WebUtility.UrlEncode(loginBox.Text);
            string passwd = WebUtility.UrlEncode(passwordBox.Password);

            string bannedPeople = await _web.DownloadStringTaskAsync(Settings.Default.ban_url);
            if (bannedPeople.Contains(loginBox.Text))
            {
                MessageBox.Show("Sorry, you are banned from our service !");
                Environment.Exit(0);
            }

            // Send the request to the server part
            string query = string.Format("{0}?_key={1}&_url=auth&login={2}&password={3}", Settings.Default.api_url, Settings.Default.skey_api, login, passwd);
            try
            {
                loginBtn.IsEnabled = false;
                loginBtn.Content = "";
                StartWaitAnimation();
                string authorization = await _web.DownloadStringTaskAsync(query);
                Logger.Current.AppendText("Send user data for verification");
                if (authorization == "true")
                {
                    if (remember.IsChecked == true)
                    {
                        _xmlhelper.UpdateSettingValue("user_login", loginBox.Text);
                        _xmlhelper.UpdateSettingValue("user_password", EncDec.Shifrovka(passwordBox.Password, "private_string"));

                        Logger.Current.AppendText("Remember the user in the configuration file");
                    }

                    var launcher = new MainWindow();
                    launcher.Show();
                    Logger.Current.AppendText("Authorization was successful");
                    Close();
                }
                else
                {
                    FancyBalloon balloon = new FancyBalloon();
                    balloon.BalloonMessage = resource.Get(TextResource.ERRORLOGINORPASSWORD);
                    tb.ShowCustomBalloon(balloon, PopupAnimation.Slide, 5000);
                    Logger.Current.AppendText("Authorization error. Incorrect username or password");

                    loginBtn.Content = resource.Get(TextResource.LOGINBTN);
                    loginBtn.IsEnabled = true;
                    passwordBox.Password = string.Empty;
                    passwordBox.Focus();
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                Directory.Delete("NostaleLauncher"); // Allez hop les logs ça dégage gros fdp de t mort
                loginBtn.Content = resource.Get(TextResource.LOGINBTN);
                loginBtn.IsEnabled = true;
                FancyBalloon LoginError = new FancyBalloon();
                LoginError.BalloonMessage = resource.Get(TextResource.ERRORLOGIN);
                tb.ShowCustomBalloon(LoginError, PopupAnimation.Slide, 5000);
                Logger.Current.AppendException(ex);
            }
            finally
            {
                StopWaitAnimation();
            }
        }

        private void registrBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Settings.Default.reg_link);
        }

        private void forgotBtn_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(Settings.Default.forg_pass_link);
        }

        private void LoadAccount()
        {
            try
            {
                if (File.Exists(LocalConfiguration.Instance.Files.ConfDataFile))
                {
                    loginBox.Text = _xmlhelper.GetSettingValue("user_login");

                    string getPass = EncDec.DeShifrovka(_xmlhelper.GetSettingValue("user_password"), "private_string");
                    passwordBox.Password = getPass;
                    Logger.Current.AppendText("Учетная запись загружена");
                }
            }
            catch (Exception ex)
            {
                Logger.Current.AppendText("Ошибка загрузки файла конфигурации");
                Logger.Current.AppendException(ex);
            }
        }

        public string hwid()
        {
            string cpuInfo = string.Empty;
            ManagementClass mc = new ManagementClass("win32_processor");
            foreach (ManagementObject mo in mc.GetInstances())
            {
                if (cpuInfo == "")
                {
                    cpuInfo = mo.Properties["processorID"].Value.ToString();
                    break;
                }
            }
            return cpuInfo;
        }

        public async void IsLauncherBanned()
        {
            string bannedPeople = await _web.DownloadStringTaskAsync(Settings.Default.ban_url);
            if (bannedPeople.Contains(hwid()))
            {
                MessageBox.Show("Sorry, you are banned from our service !");
                Environment.Exit(0);
            }
        }

        #region TRANSLATE

        private void Translate()
        {
            ResourceProvider resource = ResourceProvider.Instance;
            closeButton.ToolTip = resource.Get(TextResource.TOOLCLOSE);
            forgotPassBtn.Content = resource.Get(TextResource.FORGOTPWD);
            remember.Content = resource.Get(TextResource.REMEMBER);

            regBtn.Content = resource.Get(TextResource.CREATEACC);
            loginBtn.Content = resource.Get(TextResource.LOGINBTN);
            Logger.Current.AppendText("Завершен метод перевода окна авторизации");
        }

        #endregion TRANSLATE

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password != string.Empty)
            {
                loginBtn.IsEnabled = true;
            }
            else
            {
                loginBtn.IsEnabled = false;
            }
        }

        private void passwordBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                loginBtn_Click(null, null);
            }
        }
    }
}