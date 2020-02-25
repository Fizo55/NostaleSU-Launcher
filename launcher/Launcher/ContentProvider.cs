using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WowSuite.Launcher
{
    internal class ContentProvider
    {
        private const string UriPack = "pack://application:,,,";

        /// <summary>Uri путь к ресурсам текущей сборки</summary>
        private static readonly string ResourcesCurrentAssembly = "/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/Resources/";

        private static readonly ContentProvider _instance;

        static ContentProvider()
        {
            _instance = new ContentProvider();
        }

        private ContentProvider()
        {
            const double WH = 40d; //ширина и высота кадра
            var blizzardWait = new BitmapImage(new Uri(UriPack + ResourcesCurrentAssembly + "spinner-40-battlenet.png"));

            int brushCount = (int)(blizzardWait.Width / WH);
            var brushParts = new ImageBrush[brushCount];
            for (int i = 0; i < brushCount; i++)
            {
                brushParts[i] = new ImageBrush(blizzardWait)
                {
                    ViewportUnits = BrushMappingMode.Absolute,
                    Stretch = Stretch.None,
                    TileMode = TileMode.None,
                    Viewport = new Rect(new Point(-i * WH, 0), new Size(blizzardWait.Width, WH))
                };
            }

            const double WHmini = 20d; //ширина и высота кадра
            var blizzardWaitMini = new BitmapImage(new Uri(UriPack + ResourcesCurrentAssembly + "spinner-battlenet.small.png"));

            int brushCountMini = (int)(blizzardWaitMini.Width / WHmini);
            var brushPartsMini = new ImageBrush[brushCountMini];
            for (int i = 0; i < brushCountMini; i++)
            {
                brushPartsMini[i] = new ImageBrush(blizzardWaitMini)
                {
                    ViewportUnits = BrushMappingMode.Absolute,
                    Stretch = Stretch.None,
                    TileMode = TileMode.None,
                    Viewport = new Rect(new Point(-i * WHmini, 0), new Size(blizzardWaitMini.Width, WHmini))
                };
            }

            BlizzardAnimatedBrushes = brushParts;
            BlizzardWait = blizzardWait;

            BlizzardAnimatedBrushesMini = brushPartsMini;
            BlizzardWaitMini = blizzardWaitMini;
        }

        /// <summary>
        /// Единственный экземпляр класса
        /// </summary>
        public static ContentProvider Instance
        {
            get { return _instance; }
        }

        public ImageBrush[] BlizzardAnimatedBrushes { get; private set; }

        public BitmapImage BlizzardWait { get; private set; }

        public ImageBrush[] BlizzardAnimatedBrushesMini { get; private set; }

        public BitmapImage BlizzardWaitMini { get; private set; }

        public BitmapImage On { get; private set; }

        public BitmapImage Off { get; private set; }
    }
}