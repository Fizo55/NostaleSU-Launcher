using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WowSuite.Launcher.Animation
{
    public partial class ButtonLoader : UserControl
    {
        public static readonly DependencyProperty FramesProperty =
             DependencyProperty.Register("Frames", typeof(ImageBrush[]), typeof(ButtonLoader),
             new PropertyMetadata(FramesPropertyChanged));

        internal static readonly DependencyPropertyKey IsAnimationPlayingKey = DependencyProperty.RegisterReadOnly(
            "IsAnimationPlaying", typeof(bool), typeof(ButtonLoader), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty IsAnimationPlayingProperty = IsAnimationPlayingKey.DependencyProperty;

        /// <summary>
        /// Возвращает проигрывается ли в данный момент анимация.
        /// </summary>
        public bool IsAnimationPlaying
        {
            get { return (bool)GetValue(IsAnimationPlayingProperty); }
        }

        private int _millisecondsPerFrame;

        public ButtonLoader()
        {
            InitializeComponent();
            _animatedAreaMini.Frames = ContentProvider.Instance.BlizzardAnimatedBrushesMini; //ToDo: вынести в отдельный BlizzardWaitAnimationControl и там прописывать конкретные кадры.

            MillisecondsPerFrame = 35;
        }

        private static void FramesPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ButtonLoader)d;
            control.Frames = (ImageBrush[])e.NewValue;
        }

        /// <summary>
        /// Кадры анимации (например, набор изображений в формате PNG).
        /// </summary>
        public ImageBrush[] Frames
        {
            get { return (ImageBrush[])GetValue(FramesProperty); }
            set { SetValue(FramesProperty, value); }
        }

        /// <summary>
        /// Кол-во миллисекунд на кадр. Не может быть меньше 1.
        /// </summary>
        public int MillisecondsPerFrame
        {
            get { return _millisecondsPerFrame; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException("value");
                }
                _millisecondsPerFrame = value;
            }
        }

        public void StartAnimation()
        {
            Visibility = Visibility.Visible;
            var duration = new Duration(TimeSpan.FromMilliseconds(_animatedAreaMini.Frames.Length * MillisecondsPerFrame));
            _animatedAreaMini.BeginForeverAnimationWithPause(duration);
        }

        public void StopAnimation()
        {
            Visibility = Visibility.Hidden;
            _animatedAreaMini.StopAnimation();
        }
    }
}