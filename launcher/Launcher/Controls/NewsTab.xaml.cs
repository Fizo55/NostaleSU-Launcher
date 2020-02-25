using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WowSuite.Language;
using WowSuite.Launcher.Logic;
using WowSuite.Launcher.Properties;
using WowSuite.Launcher.Utils;
using WowSuite.Utils.News;

namespace WowSuite.Launcher.Controls
{
    public partial class NewsTab : UserControl
    {
        private ResourceProvider resource = ResourceProvider.Instance;
        private const string AllRealms = "All Realms";
        private IEnumerable<ExpressNewsItem> _news;
        private Dictionary<string, string> _newsKeys;

        private readonly LauncherLogic _launcher;
        private readonly AddressSet _addressSet;
        private readonly IpPortConfig _mySqlConfig;
        private readonly IpPortConfig _worldConfig;
        private readonly WebClientFactory _webClientFactory;
        private readonly WebClient _web;

        public static readonly DependencyProperty NewsItemsProperty = DependencyProperty.Register(
            "NewsItems", typeof(ExpressNewsItem[]), typeof(NewsTab),
            new PropertyMetadata(default(ExpressNewsItem[]),
                new PropertyChangedCallback(NewsItemsPropertyChanged)));

        public static readonly DependencyProperty CurrentNewsItemProperty = DependencyProperty.Register(
            "CurrentNewsItem", typeof(ExpressNewsItem), typeof(NewsTab),
            new PropertyMetadata(default(ExpressNewsItem)));

        private readonly Storyboard _storyboard;
        private int _currentIndex;

        public NewsTab()
        {
            InitializeComponent();

            var mySqlConfig = new IpPortConfig
            {
                Ip = Settings.Default.mysql_ip,
                Port = Settings.Default.mysql_port
            };
            var worldConfig = new IpPortConfig
            {
                Ip = Settings.Default.world_ip,
                Port = Settings.Default.world_port
            };

            var addressSet = new AddressSet
            {
                LoadBannerNews = string.Format(Settings.Default.api_url + "?_key={0}&_url=getNewsTab", Settings.Default.skey_api),
            };

            _addressSet = addressSet;
            _mySqlConfig = mySqlConfig;
            _worldConfig = worldConfig;

            _launcher = new LauncherLogic(addressSet, mySqlConfig, worldConfig);
            _launcher.NewsLoadBanner += Launcher_NewsLoadBanner;

            _storyboard = FindResource("ChangeItems") as Storyboard;
            Debug.Assert(_storyboard != null, "_storyboard не должна быть null");
            Storyboard.SetTarget(_storyboard, LayoutRoot);

            _newsKeys = new Dictionary<string, string>()
            {
                { AllRealms, resource.Get(TextResource.ALLREALMS) },
                { "realm1", Settings.Default.server_name_wotlk },
            };
        }

        public IEnumerable<ExpressNewsItem> News
        {
            get { return _news; }
            set { _news = value; }
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            cbNews.ItemsSource = _newsKeys;
            cbNews.SelectedIndex = 0;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var items = News;
            if (items == null)
            {
                return;
            }

            var pair = (KeyValuePair<string, string>)((ComboBox)sender).SelectedItem;
            string pattern = pair.Key;

            if (pattern.Equals(AllRealms) && charList.ItemsSource != News)
            {
                charList.ItemsSource = News;
                return;
            }

            var filtered = new List<ExpressNewsItem>(items.Count());
            foreach (ExpressNewsItem item in items)
            {
                if (item.Realms.Equals(pattern, StringComparison.OrdinalIgnoreCase))
                {
                    filtered.Add(item);
                }
            }

            charList.ItemsSource = filtered;
            if (filtered.Count > 0)
            {
                charList.SelectedIndex = 0;
            }
        }

        private void NewsControl_Loaded(object sender, RoutedEventArgs e)
        {
            _launcher.Initialize();
            cbNews.ItemsSource = _newsKeys;
            cbNews.SelectedIndex = 0;
        }

        public ExpressNewsItem[] NewsItems
        {
            get { return (ExpressNewsItem[])GetValue(NewsItemsProperty); }
            set { SetValue(NewsItemsProperty, value); }
        }

        private void Launcher_NewsLoadBanner(object sender, LoadTextEventArgs e)
        {
            switch (e.State)
            {
                case LoadingState.Canceled:
                    break;

                case LoadingState.Started:
                    charList.Visibility = Visibility.Hidden;
                    StartWaitAnimation();
                    break;

                case LoadingState.Completed:
                    try
                    {
                        if (e.Result.Success)
                        {
                            ExpressNewsSet newsSet = ExpressNewsSet.FromXml(e.Result.Data);
                            newsSet.Update(_newsKeys);
                            News = newsSet.ExpressNews;
                            charList.ItemsSource = newsSet.ExpressNews;
                            charList.SelectedIndex = 0;
                        }
                    }
                    catch (Exception err)
                    {
                        Logger.Current.AppendException(err);
                    }
                    StopWaitAnimation();
                    charList.Visibility = Visibility.Visible;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ExpressNewsItem CurrentNewsItem
        {
            get { return (ExpressNewsItem)GetValue(CurrentNewsItemProperty); }
            set { SetValue(CurrentNewsItemProperty, value); }
        }

        public BitmapImage Banner { get; set; }

        private static void NewsItemsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (NewsTab)d;
            var news = e.NewValue as ExpressNewsItem[];
            if (news != null && news.Length > 0)
            {
                control.CurrentNewsItem = news[0];
            }
        }

        #region ANIMATION

        public void StartWaitAnimation()
        {
            initIndicator.Visibility = Visibility.Visible;
        }

        public void StopWaitAnimation()
        {
            initIndicator.Visibility = Visibility.Hidden;
        }

        #endregion ANIMATION

        private void newsImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            string newsToLink = (charList.SelectedItem as ExpressNewsItem).NewsLink;
            Process.Start(newsToLink);
        }
    }
}