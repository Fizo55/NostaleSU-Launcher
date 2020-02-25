using System.Windows;
using System.Windows.Input;
using WowSuite.Language;
using WowSuite.Launcher.Utils;

namespace WowSuite.Launcher.Dialogs
{
    public partial class PopupDialogUpdate : Window
    {
        private readonly XmlHelper _xmlhelper;

        public PopupDialogUpdate()
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

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void updateBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Translate()
        {
            ResourceProvider resource = ResourceProvider.Instance;
            closeBtn.ToolTip = WowSuite.Language.ruRU.TOOLCLOSE; resource.Get(TextResource.TOOLCLOSE);
        }
    }
}