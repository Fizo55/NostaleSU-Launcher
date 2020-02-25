using System.Windows;
using System.Windows.Input;
using WowSuite.Language;
using WowSuite.Launcher.Utils;

namespace WowSuite.Launcher.Dialogs
{
    /// <summary>
    /// Interaction logic for PopupDialog.xaml
    /// </summary>
    public partial class PopupDialogReset : Window
    {
        private readonly XmlHelper _xmlhelper;

        public PopupDialogReset()
        {
            InitializeComponent();
            _xmlhelper = new XmlHelper();
            Translate();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void restartLaterBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void restartBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.Application.Restart();
            System.Windows.Application.Current.Shutdown();
        }

        private void Translate()
        {
            ResourceProvider resource = ResourceProvider.Instance;

            restartBtn.Content = resource.Get(TextResource.RESTARTBTN);
            restartLaterBtn.Content = resource.Get(TextResource.RESTARTLATERBTN);
            closeBtn.ToolTip = resource.Get(TextResource.TOOLCLOSE);
            popupMessage.Text = resource.Get(TextResource.POPUPMSGRESTART);
        }
    }
}