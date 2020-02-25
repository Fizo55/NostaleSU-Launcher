using System.Net;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Effects;
using WowSuite.Language;
using WowSuite.Launcher.Dialogs;
using WowSuite.Launcher.Utils;

namespace WowSuite.Launcher.Controls
{
    public partial class WelcomeBlock : System.Windows.Controls.UserControl
    {
        private readonly XmlHelper _xmlhelper;
        private readonly WebClient _web;
        private readonly WebClientFactory _webClientFactory;

        public WelcomeBlock()
        {
            InitializeComponent();
            _xmlhelper = new XmlHelper();
            Translate();
            DataContext = this;
        }

        public void ShowBlock()
        {
            LayoutRoot.Visibility = Visibility.Visible;
        }

        public void HideBlock()
        {
            LayoutRoot.Visibility = Visibility.Hidden;
        }

        private void settingsBtn_Click(object sender, RoutedEventArgs e)
        {
            var settings = new SettingsWindow();
            settings.Settings.SelectedIndex = 2;
            settings.ShowDialog();
        }

        private void Translate()
        {
            ResourceProvider resource = ResourceProvider.Instance;

            welcomeBlock.Text = resource.Get(TextResource.WELCBLOCK);
            welcomeDescript.Text = resource.Get(TextResource.WELCDESCR);
            welcomeBtn.Content = resource.Get(TextResource.WELCBTN);
            settingsBtn.Content = resource.Get(TextResource.WELCBTNSET);
            change.Content = resource.Get(TextResource.CHANGEGAMELANG);
        }

        private async void welcomeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (changeLang.SelectedIndex == 1)
            {
                FolderBrowserDialog updPathRu = new FolderBrowserDialog();
                updPathRu.ShowDialog();
                string rootDirectory = updPathRu.SelectedPath;
                _xmlhelper.UpdateSettingValue("realm1_client_location", rootDirectory);
                _xmlhelper.UpdateSettingValue("client_lang", "en");

                try
                {
                    var popupReset = new PopupDialogReset();
                    bool? dialogResultRestart = popupReset.ShowDialog();

                    if (dialogResultRestart == true)
                    {
                        System.Windows.Forms.Application.Restart();
                        System.Windows.Application.Current.Shutdown();
                    }
                }
                catch
                {
                    Logger.Current.AppendText("Возникла ошибка при обновлении файла конфигурации");
                }
            }
            else if (changeLang.SelectedIndex == 2)
            {
                FolderBrowserDialog updPathFr = new FolderBrowserDialog();
                updPathFr.ShowDialog();
                string rootDirectory = updPathFr.SelectedPath;
                _xmlhelper.UpdateSettingValue("realm1_client_location", rootDirectory);
                _xmlhelper.UpdateSettingValue("client_lang", "fr");

                try
                {
                    var popupReset = new PopupDialogReset();
                    bool? dialogResultRestart = popupReset.ShowDialog();

                    if (dialogResultRestart == true)
                    {
                        System.Windows.Forms.Application.Restart();
                        System.Windows.Application.Current.Shutdown();
                    }
                }
                catch
                {
                    Logger.Current.AppendText("Возникла ошибка при обновлении файла конфигурации");
                }
            }
            else if (changeLang.SelectedIndex == 3)
            {
                FolderBrowserDialog updPathEs = new FolderBrowserDialog();
                updPathEs.ShowDialog();
                string rootDirectory = updPathEs.SelectedPath;
                _xmlhelper.UpdateSettingValue("realm1_client_location", rootDirectory);
                _xmlhelper.UpdateSettingValue("client_lang", "es");

                try
                {
                    var popupReset = new PopupDialogReset();
                    bool? dialogResultRestart = popupReset.ShowDialog();

                    if (dialogResultRestart == true)
                    {
                        System.Windows.Forms.Application.Restart();
                        System.Windows.Application.Current.Shutdown();
                    }
                }
                catch
                {
                    Logger.Current.AppendText("Возникла ошибка при обновлении файла конфигурации");
                }
            }
            else if (changeLang.SelectedIndex == 4)
            {
                FolderBrowserDialog updPathIt = new FolderBrowserDialog();
                updPathIt.ShowDialog();
                string rootDirectory = updPathIt.SelectedPath;
                _xmlhelper.UpdateSettingValue("realm1_client_location", rootDirectory);
                _xmlhelper.UpdateSettingValue("client_lang", "it");

                try
                {
                    var popupReset = new PopupDialogReset();
                    bool? dialogResultRestart = popupReset.ShowDialog();

                    if (dialogResultRestart == true)
                    {
                        System.Windows.Forms.Application.Restart();
                        System.Windows.Application.Current.Shutdown();
                    }
                }
                catch
                {
                    Logger.Current.AppendText("Возникла ошибка при обновлении файла конфигурации");
                }
            }
            else if (changeLang.SelectedIndex == 5)
            {
                FolderBrowserDialog updPathDe = new FolderBrowserDialog();
                updPathDe.ShowDialog();
                string rootDirectory = updPathDe.SelectedPath;
                _xmlhelper.UpdateSettingValue("realm1_client_location", rootDirectory);
                _xmlhelper.UpdateSettingValue("client_lang", "de");

                try
                {
                    var popupReset = new PopupDialogReset();
                    bool? dialogResultRestart = popupReset.ShowDialog();

                    if (dialogResultRestart == true)
                    {
                        System.Windows.Forms.Application.Restart();
                        System.Windows.Application.Current.Shutdown();
                    }
                }
                catch
                {
                    Logger.Current.AppendText("Возникла ошибка при обновлении файла конфигурации");
                }
            }
            else if (changeLang.SelectedIndex == 6)
            {
                FolderBrowserDialog updPathCz = new FolderBrowserDialog();
                updPathCz.ShowDialog();
                string rootDirectory = updPathCz.SelectedPath;
                _xmlhelper.UpdateSettingValue("realm1_client_location", rootDirectory);
                _xmlhelper.UpdateSettingValue("client_lang", "cz");

                try
                {
                    var popupReset = new PopupDialogReset();
                    bool? dialogResultRestart = popupReset.ShowDialog();

                    if (dialogResultRestart == true)
                    {
                        System.Windows.Forms.Application.Restart();
                        System.Windows.Application.Current.Shutdown();
                    }
                }
                catch
                {
                    Logger.Current.AppendText("Возникла ошибка при обновлении файла конфигурации");
                }
            }
            else if (changeLang.SelectedIndex == 7)
            {
                FolderBrowserDialog updPathTr = new FolderBrowserDialog();
                updPathTr.ShowDialog();
                string rootDirectory = updPathTr.SelectedPath;
                _xmlhelper.UpdateSettingValue("realm1_client_location", rootDirectory);
                _xmlhelper.UpdateSettingValue("client_lang", "tr");

                try
                {
                    var popupReset = new PopupDialogReset();
                    bool? dialogResultRestart = popupReset.ShowDialog();

                    if (dialogResultRestart == true)
                    {
                        System.Windows.Forms.Application.Restart();
                        System.Windows.Application.Current.Shutdown();
                    }
                }
                catch
                {
                    Logger.Current.AppendText("Возникла ошибка при обновлении файла конфигурации");
                }
            }
        }

        private void changeLang_Loaded(object sender, RoutedEventArgs e)
        {
            changeLang.SelectedIndex = 0;
        }

        #region BLUR EFFECTS

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

        #endregion BLUR EFFECTS
    }
}