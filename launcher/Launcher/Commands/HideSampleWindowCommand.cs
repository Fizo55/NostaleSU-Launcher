using Samples.libs;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WowSuite.Launcher;
using WowSuite.Launcher.Config;

namespace Samples.Commands
{
    /// <summary>
    /// Hides the main window.
    /// </summary>
    public class HideSampleWindowCommand : CommandBase<HideSampleWindowCommand>
    {
        public override void Execute(object parameter)
        {
            GetTaskbarWindow(parameter).Hide();
            CommandManager.InvalidateRequerySuggested();
        }

        public override bool CanExecute(object parameter)
        {
            Window win = GetTaskbarWindow(parameter);
            return win != null && win.IsVisible;
        }
    }

    public class ClearCache : CommandBase<ClearCache>
    {
        public override void Execute(object parameter)
        {
            MainWindow main = new MainWindow();
            main.tb = new TaskbarIcon();

            string cache = LocalConfiguration.Instance.Folders.GetPath(WowLauncherFolder.Cache);
            if (Directory.Exists(cache))
            {
                Directory.Delete(cache, true);

                FancyBalloon balloon = new FancyBalloon();
                balloon.BalloonMessage = string.Format("Folder {0} successfully removed", AppFolder.CACHE_FOLDER_NAME);
                main.tb.ShowCustomBalloon(balloon, PopupAnimation.Slide, 5000);
            }
            else
            {
                FancyBalloon balloon = new FancyBalloon();
                balloon.BalloonMessage = string.Format("Folder {0} not found", AppFolder.CACHE_FOLDER_NAME);
                main.tb.ShowCustomBalloon(balloon, PopupAnimation.Slide, 5000);
            }
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}