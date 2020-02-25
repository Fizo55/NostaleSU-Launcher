using Samples;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Effects;
using WowSuite.Language;
using WowSuite.Launcher.Config;
using WowSuite.Launcher.Dialogs;
using WowSuite.Launcher.Utils;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace WowSuite.Launcher
{
    public partial class SettingsWindow : Window
    {
        private readonly XmlHelper _xmlhelper;

        public List<string> config = new List<string>();

        public const string path = "Config.dat";

        public SettingsWindow()
        {
            InitializeComponent();
            SetResultText(string.Empty);
            _xmlhelper = new XmlHelper();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Translate();
            LoadData();
            Check_CheckBox();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void closeButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetResultText(string text)
        {
            result.Text = text;
        }

        #region byte to string (algo)

        public string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);

                if (hex.ToString().Count() == 4)
                {
                    config.Add(hex.ToString());
                    hex = new StringBuilder();
                }
            }

            return hex.ToString();
        }

        public static byte[] StrToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        #endregion byte to string (algo)

        public void Check_CheckBox()
        {
            if (!File.Exists(path)) return;

            byte[] data = File.ReadAllBytes(path);
            ByteArrayToString(data);

            if (config.Any())
            {
                switch (config[0])
                {
                    case "0101":
                        FullScreen.IsChecked = true;
                        size1.IsChecked = true;
                        break;

                    case "0102":
                        FullScreen.IsChecked = true;
                        size3.IsChecked = true;
                        break;

                    case "0103":
                        FullScreen.IsChecked = true;
                        size2.IsChecked = true;
                        break;

                    case "0104":
                        FullScreen.IsChecked = true;
                        size4.IsChecked = true;
                        break;

                    case "0105":
                        FullScreen.IsChecked = true;
                        size.IsChecked = true;
                        break;

                    case "0106":
                        FullScreen.IsChecked = true;
                        size5.IsChecked = true;
                        break;

                    case "0001":
                    case "0000":
                        size1.IsChecked = true;
                        break;

                    case "0002":
                        size3.IsChecked = true;
                        break;

                    case "0003":
                        size2.IsChecked = true;
                        break;

                    case "0004":
                        size4.IsChecked = true;
                        break;

                    case "0005":
                        size.IsChecked = true;
                        break;

                    case "0006":
                        size5.IsChecked = true;
                        break;
                }

                switch (config[8])
                {
                    case "0001":
                        DirectX.IsChecked = true;
                        FpsSlider.Value = 1;
                        break;

                    case "0002":
                        DirectX.IsChecked = true;
                        FpsSlider.Value = 2;
                        break;

                    case "0003":
                        DirectX.IsChecked = true;
                        FpsSlider.Value = 3;
                        break;

                    case "0004":
                        DirectX.IsChecked = true;
                        FpsSlider.Value = 4;
                        break;

                    case "0005":
                        DirectX.IsChecked = true;
                        FpsSlider.Value = 5;
                        break;

                    case "0006":
                        DirectX.IsChecked = true;
                        FpsSlider.Value = 6;
                        break;

                    case "0007":
                        DirectX.IsChecked = true;
                        FpsSlider.Value = 7;
                        break;

                    case "0101":
                        OpenGL.IsChecked = true;
                        FpsSlider.Value = 1;
                        break;

                    case "0102":
                        OpenGL.IsChecked = true;
                        FpsSlider.Value = 2;
                        break;

                    case "0103":
                        OpenGL.IsChecked = true;
                        FpsSlider.Value = 3;
                        break;

                    case "0104":
                        OpenGL.IsChecked = true;
                        FpsSlider.Value = 4;
                        break;

                    case "0105":
                        OpenGL.IsChecked = true;
                        FpsSlider.Value = 5;
                        break;

                    case "0106":
                        OpenGL.IsChecked = true;
                        FpsSlider.Value = 6;
                        break;

                    case "0107":
                        OpenGL.IsChecked = true;
                        FpsSlider.Value = 7;
                        break;
                }

                switch (config[1])
                {
                    case "0100":
                        bit1.IsChecked = true;
                        break;

                    case "0000":
                        bit.IsChecked = true;
                        break;
                }

                switch (config[6])
                {
                    case "0001":
                        Bgm.IsChecked = true;
                        break;

                    case "0100":
                        FxOn.IsChecked = true;
                        break;

                    case "0101":
                        Bgm.IsChecked = true;
                        FxOn.IsChecked = true;
                        break;
                }

                switch (config[7])
                {
                    case "0100":
                        D.IsChecked = true;
                        break;
                }
            }
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ResourceProvider resource = ResourceProvider.Instance;
            string rootDirectory = _xmlhelper.GetSettingValue("realm1_client_location");

            try
            {
                if (File.Exists(Path.Combine(rootDirectory, path)))
                {
                    if ((!(bool)DirectX.IsChecked && !(bool)OpenGL.IsChecked) ||
                        (!(bool)size.IsChecked && !(bool)size1.IsChecked && !(bool)size2.IsChecked && !(bool)size3.IsChecked && !(bool)size4.IsChecked && !(bool)size5.IsChecked) ||
                        (!(bool)bit.IsChecked && !(bool)bit1.IsChecked))
                    {
                        FancyBalloon balloon = new FancyBalloon
                        {
                            BalloonMessage = resource.Get(TextResource.CHECKONECHEKBOX)
                        };
                        tb.ShowCustomBalloon(balloon, PopupAnimation.Slide, 5000);
                        SetResultText("");
                        return;
                    }

                    byte[] data = File.ReadAllBytes(path);

                    if (config.Any())
                    {
                        config.Clear();
                    }

                    ByteArrayToString(data);

                    if ((bool)DirectX.IsChecked)
                    {
                        switch (FpsSlider.Value)
                        {
                            case 1:
                                config[8] = "0001";
                                break;

                            case 2:
                                config[8] = "0002";
                                break;

                            case 3:
                                config[8] = "0003";
                                break;

                            case 4:
                                config[8] = "0004";
                                break;

                            case 5:
                                config[8] = "0005";
                                break;

                            case 6:
                                config[8] = "0006";
                                break;

                            case 7:
                                config[8] = "0007";
                                break;
                        }
                    }

                    if ((bool)OpenGL.IsChecked)
                    {
                        switch (FpsSlider.Value)
                        {
                            case 1:
                                config[8] = "0101";
                                break;

                            case 2:
                                config[8] = "0102";
                                break;

                            case 3:
                                config[8] = "0103";
                                break;

                            case 4:
                                config[8] = "0104";
                                break;

                            case 5:
                                config[8] = "0105";
                                break;

                            case 6:
                                config[8] = "0106";
                                break;

                            case 7:
                                config[8] = "0107";
                                break;
                        }
                    }

                    if ((bool)bit.IsChecked) config[1] = "0000";
                    if ((bool)bit1.IsChecked) config[1] = "0100";

                    switch (FullScreen.IsChecked)
                    {
                        case true:
                            if ((bool)size.IsChecked) config[0] = "0105";
                            if ((bool)size1.IsChecked) config[0] = "0101";
                            if ((bool)size2.IsChecked) config[0] = "0103";
                            if ((bool)size3.IsChecked) config[0] = "0102";
                            if ((bool)size4.IsChecked) config[0] = "0104";
                            if ((bool)size5.IsChecked) config[0] = "0106";
                            break;

                        case false:
                            if ((bool)size.IsChecked) config[0] = "0005";
                            if ((bool)size1.IsChecked) config[0] = "0001";
                            if ((bool)size2.IsChecked) config[0] = "0003";
                            if ((bool)size3.IsChecked) config[0] = "0002";
                            if ((bool)size4.IsChecked) config[0] = "0004";
                            if ((bool)size5.IsChecked) config[0] = "0006";
                            break;
                    }

                    if ((bool)Bgm.IsChecked) config[6] = "0001";

                    if ((bool)FxOn.IsChecked) config[6] = "0100";

                    if ((bool)D.IsChecked) config[7] = "0100";

                    if ((bool)Bgm.IsChecked && (bool)FxOn.IsChecked) config[6] = "0101";

                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (string str in config)
                    {
                        stringBuilder.Append(str);
                    }

                    BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate));
                    writer.Write(StrToByteArray(stringBuilder.ToString()));
                    writer.Close();
                    writer.Dispose();
                }

                _xmlhelper.UpdateSettingValue("user_login", textBoxLogin.Text);
                _xmlhelper.UpdateSettingValue("user_password", EncDec.Shifrovka(passwordBox.Password, "private_string"));
                _xmlhelper.UpdateSettingValue("realm1_client_location", location1.Text);

                if (copyApp.IsChecked == true)
                {
                    _xmlhelper.UpdateSettingValue("run_copy_app", "false");
                }
                else
                {
                    _xmlhelper.UpdateSettingValue("run_copy_app", "true");
                }

                FancyBalloon balloon1 = new FancyBalloon
                {
                    BalloonMessage = resource.Get(TextResource.SETTINGOK)
                };
                tb.ShowCustomBalloon(balloon1, PopupAnimation.Slide, 5000);
                Logger.Current.AppendText("Пользовательские данные успешно сохранены");
                SetResultText("");

                var popupReset = new PopupDialogReset
                {
                    Owner = this
                };
                ApplyEffect(this);
                bool? dialogResult = popupReset.ShowDialog();
                ClearEffect(this);

                if (dialogResult == true)
                {
                    Close();
                }
            }
            catch (WellknownKeyNotFoundException ex)
            {
                MessageBox.Show(ex.Key.ToString());
                FancyBalloon balloon = new FancyBalloon
                {
                    BalloonMessage = resource.Get(TextResource.SETTINGERR)
                };
                tb.ShowCustomBalloon(balloon, PopupAnimation.Slide, 5000);
                Logger.Current.AppendText("Ошибка сохранения пользовательских данных");
            }
        }

        private void copyApp_Loaded(object sender, RoutedEventArgs e)
        {
            string copyRunApp = _xmlhelper.GetSettingValue("run_copy_app");

            switch (copyRunApp)
            {
                case "false":
                    copyApp.IsChecked = true;
                    break;

                case "true":
                    copyApp.IsChecked = false;
                    break;

                default:
                    break;
            }
        }

        private void changeLang_Loaded(object sender, RoutedEventArgs e)
        {
            string language = _xmlhelper.GetSettingValue("app_language");

            switch (language)
            {
                case "enUS":
                    changeLang.SelectedIndex = 0;
                    break;

                case "frFRA":
                    changeLang.SelectedIndex = 1;
                    break;

                case "esESP":
                    changeLang.SelectedIndex = 2;
                    break;

                case "itITA":
                    changeLang.SelectedIndex = 3;
                    break;

                case "deDEU":
                    changeLang.SelectedIndex = 4;
                    break;

                case "czCZ":
                    changeLang.SelectedIndex = 5;
                    break;

                case "trTR":
                    changeLang.SelectedIndex = 6;
                    break;

                case "ruRU":
                    changeLang.SelectedIndex = 7;
                    break;

                default:
                    break;
            }
        }

        // TODO : Move this to their respective language because for now the patchlist doesn't (API & WebSite) doesn't support multilanguage
        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            ComboBoxItem comboBoxItem = e.OriginalSource as ComboBoxItem;
            if (comboBoxItem != null)
            {
                if (comboBoxItem.Content.ToString() == "English(Us)")
                {
                    _xmlhelper.UpdateSettingValue("app_language", "enUS");
                    _xmlhelper.UpdateSettingValue("client_lang", "en");
                }
                else if (comboBoxItem.Content.ToString() == "Français(FRA)")
                {
                    _xmlhelper.UpdateSettingValue("app_language", "frFRA");
                    _xmlhelper.UpdateSettingValue("client_lang", "fr");
                }
                else if (comboBoxItem.Content.ToString() == "Español(ESP)")
                {
                    _xmlhelper.UpdateSettingValue("app_language", "esESP");
                    _xmlhelper.UpdateSettingValue("client_lang", "es");
                }
                else if (comboBoxItem.Content.ToString() == "Italiano(ITA)")
                {
                    _xmlhelper.UpdateSettingValue("app_language", "itITA");
                    _xmlhelper.UpdateSettingValue("client_lang", "it");
                }
                else if (comboBoxItem.Content.ToString() == "Deutsch(DEU)")
                {
                    _xmlhelper.UpdateSettingValue("app_language", "deDEU");
                    _xmlhelper.UpdateSettingValue("client_lang", "de");
                }
                else if (comboBoxItem.Content.ToString() == "Český(CZ)")
                {
                    _xmlhelper.UpdateSettingValue("app_language", "czCZ");
                    _xmlhelper.UpdateSettingValue("client_lang", "cz");
                }
                else if (comboBoxItem.Content.ToString() == "Türk(TR)")
                {
                    _xmlhelper.UpdateSettingValue("app_language", "trTR");
                    _xmlhelper.UpdateSettingValue("client_lang", "tr");
                }
                else if (comboBoxItem.Content.ToString() == "Русский(Ru)")
                {
                    _xmlhelper.UpdateSettingValue("app_language", "ruRU");
                    _xmlhelper.UpdateSettingValue("client_lang", "ru");
                }
            }
        }

        private void LoadData()
        {
            if (File.Exists(LocalConfiguration.Instance.Files.ConfDataFile))
            {
                textBoxLogin.Text = _xmlhelper.GetSettingValue("user_login");
                string getPass = EncDec.DeShifrovka(_xmlhelper.GetSettingValue("user_password"), "private_string");
                passwordBox.Password = getPass;

                location1.Text = _xmlhelper.GetSettingValue("realm1_client_location");
                Logger.Current.AppendText("Загрузка пользовательских данных");

                if (textBoxLogin.Text != string.Empty && passwordBox.Password != string.Empty)
                {
                    exitBtn.Visibility = Visibility.Visible;
                }
                else
                {
                    exitBtn.Visibility = Visibility.Hidden;
                }
            }
        }

        private void Translate()
        {
            ResourceProvider resource = ResourceProvider.Instance;

            saveSettingsButton.Content = resource.Get(TextResource.SAVEBTN);
            settingsLabelTooltip.Content = resource.Get(TextResource.SETTLABEL1);
            settingsLabelTooltip2.Text = resource.Get(TextResource.SETTLABEL2);
            closeButtonSettings.ToolTip = resource.Get(TextResource.TOOLCLOSE);
            SettingsLabel.Text = resource.Get(TextResource.SETHEADER);
            EnterLogin.Text = resource.Get(TextResource.ENTLOG);
            EnterPass.Text = resource.Get(TextResource.ENTPASS);
            tabMenu1.Text = resource.Get(TextResource.TABMENU1);
            tabMenu2.Text = resource.Get(TextResource.TABMENU2);
            tabMenu.Text = resource.Get(TextResource.TABMENU);

            labelGame.Text = resource.Get(TextResource.TABMENU2);

            string changeLoc = resource.Get(TextResource.CHANGELOC);
            client1Btn.Content = changeLoc;

            string entExe = resource.Get(TextResource.ENTWOWEXE);
            location1.Text = entExe;

            clearSetting.Content = resource.Get(TextResource.BTNCLEAR);
            exitBtn.Content = resource.Get(TextResource.EXITBTN);
            appLang.Text = resource.Get(TextResource.LANGUAGEMENU);
            labelGeneral.Text = resource.Get(TextResource.GENERALSETLABEL);
            copyApp.Content = resource.Get(TextResource.COPYAPP);
            Logger.Current.AppendText("Завершен метод перевода формы с настройками");
        }

        private void textBoxLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBoxLogin = (TextBox)sender;
            textBoxLogin.GotFocus -= textBoxLogin_GotFocus;
        }

        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox password = (PasswordBox)sender;
            password.GotFocus -= passwordBox_GotFocus;
        }

        private void location1_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox location1 = (TextBox)sender;
            location1.GotFocus -= location1_GotFocus;
        }

        private void client1Btn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string foldername = dialog.SelectedPath;
                location1.Text = foldername;
            }
        }

        // BLUR EFFECTS
        private void ApplyEffect(Window win)
        {
            BlurEffect objBlur = new BlurEffect();
            objBlur.Radius = 4;
            win.Effect = objBlur;
        }

        private void ClearEffect(Window win)
        {
            win.Effect = null;
        }

        // BLUR EFFECTS

        private void clearSetting_Click(object sender, RoutedEventArgs e)
        {
            var popup = new PopupDialog();
            popup.Owner = this;
            ApplyEffect(this);
            bool? dialogResult = popup.ShowDialog();
            ClearEffect(this);

            if (dialogResult == true)
            {
                try
                {
                    var popupReset = new PopupDialogReset();
                    popupReset.Owner = this;
                    ApplyEffect(this);
                    bool? dialogResultRestart = popupReset.ShowDialog();
                    ClearEffect(this);

                    if (dialogResultRestart == true)
                    {
                        Close();
                    }
                }
                catch
                {
                    Logger.Current.AppendText("Возникла ошибка при сохранении файла конфигурации");
                }

                Close();
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password != string.Empty)
            {
                exitBtn.IsEnabled = true;
            }
            else
            {
                exitBtn.IsEnabled = false;
            }
        }
    }
}