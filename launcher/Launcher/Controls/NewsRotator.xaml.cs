using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WowSuite.Utils.News;

namespace WowSuite.Launcher.Controls
{
    public partial class NewsRotator : UserControl
    {
        public static readonly DependencyProperty NewsItemsProperty = DependencyProperty.Register(
            "NewsItems", typeof(ExpressNewsItem[]), typeof(NewsRotator),
            new PropertyMetadata(default(ExpressNewsItem[]),
                new PropertyChangedCallback(NewsItemsPropertyChanged)));

        public static readonly DependencyProperty CurrentNewsItemProperty = DependencyProperty.Register(
            "CurrentNewsItem", typeof(ExpressNewsItem), typeof(NewsRotator),
            new PropertyMetadata(default(ExpressNewsItem)));

        private readonly Storyboard _storyboard;
        private int _currentIndex;

        private DispatcherTimer NextSliderTimer = new DispatcherTimer();

        private void ScheduleSliderTimer()
        {
            this.NextSliderTimer.Stop();
            this.NextSliderTimer.Tick += new EventHandler(this.NextSlider);
            this.NextSliderTimer.Interval = new TimeSpan(0, 0, 10);
            this.NextSliderTimer.Start();
        }

        private void Slider_MouseEnter(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = new double?((double)79);
            doubleAnimation.To = new double?((double)100);
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            this.panelText.BeginAnimation(FrameworkElement.HeightProperty, doubleAnimation);
            DoubleAnimation doubleAnimation2 = new DoubleAnimation();
            doubleAnimation2.From = new double?(0.0);
            doubleAnimation2.To = new double?((double)1);
            doubleAnimation2.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            this.newsBody.BeginAnimation(UIElement.OpacityProperty, doubleAnimation2);
        }

        private void Slider_MouseLeave(object sender, RoutedEventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = new double?((double)100);
            doubleAnimation.To = new double?((double)79);
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            this.panelText.BeginAnimation(FrameworkElement.HeightProperty, doubleAnimation);
            DoubleAnimation doubleAnimation2 = new DoubleAnimation();
            doubleAnimation2.From = new double?((double)1);
            doubleAnimation2.To = new double?(0.0);
            doubleAnimation2.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            this.newsBody.BeginAnimation(UIElement.OpacityProperty, doubleAnimation2);
        }

        public NewsRotator()
        {
            InitializeComponent();
            _storyboard = FindResource("ChangeItems") as Storyboard;
            Debug.Assert(_storyboard != null, "_storyboard не должна быть null");
            Storyboard.SetTarget(_storyboard, mainGrid);
        }

        /// <summary>
        /// Массив новостей
        /// </summary>
        public ExpressNewsItem[] NewsItems
        {
            get { return (ExpressNewsItem[])GetValue(NewsItemsProperty); }
            set { SetValue(NewsItemsProperty, value); }
        }

        /// <summary>
        /// Отображаемая новость
        /// </summary>
        public ExpressNewsItem CurrentNewsItem
        {
            get { return (ExpressNewsItem)GetValue(CurrentNewsItemProperty); }
            set { SetValue(CurrentNewsItemProperty, value); }
        }

        public BitmapImage Banner { get; set; }

        private static void NewsItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NewsRotator)d;
            var news = e.NewValue as ExpressNewsItem[];
            if (news != null && news.Length > 0)
            {
                control.CurrentNewsItem = news[0];
            }
        }

        private void NewsControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentNewsItem != null && !string.IsNullOrEmpty(CurrentNewsItem.NewsLink))
            {
                Process.Start(CurrentNewsItem.NewsLink);
            }
        }

        //Обработчик клика по кнопке "Прошлая новость"
        private void PreviousNewsItem(object sender, RoutedEventArgs e)
        {
            if (NewsItems != null)
            {
                if (--_currentIndex < 0)
                {
                    _currentIndex = NewsItems.Length - 1;
                }

                CurrentNewsItem = NewsItems[_currentIndex];
                ScheduleSliderTimer();
            }
        }

        //Обработчик клика по кнопке "Следующая новость"
        private void NextNewsItem(object sender, RoutedEventArgs e)
        {
            if (NewsItems != null)
            {
                if (++_currentIndex == NewsItems.Length)
                {
                    _currentIndex = 0;
                }

                CurrentNewsItem = NewsItems[_currentIndex];
                ScheduleSliderTimer();
            }
        }

        private void NextSlider(object sender, EventArgs e)
        {
            if (this.NewsItems != null)
            {
                if (++_currentIndex == NewsItems.Length)
                {
                    _currentIndex = 0;
                }

                CurrentNewsItem = NewsItems[_currentIndex];
            }
        }
    }
}