using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using WowSuite.Utils;

namespace WowSuite.Launcher.Logic
{
    /// <summary>
    /// Основной класс логики лаунчера
    /// </summary>
    public class LauncherLogic
    {
        private readonly SynchronizationContext _syncContext;
        private readonly AddressSet _addressSet;
        private readonly IpPortConfig _mySqlConfig;
        private readonly IpPortConfig _worldConfig;

        private readonly WebClientFactory _webClientFactory;

        public LauncherLogic(AddressSet addressSet, IpPortConfig mySqlConfig, IpPortConfig worldConfig)
        {
            _syncContext = SynchronizationContext.Current;
            _addressSet = addressSet;
            _mySqlConfig = mySqlConfig;
            _worldConfig = worldConfig;
            _webClientFactory = new WebClientFactory();
        }

        public void Initialize()
        {
            Task.Factory.StartNew(() =>
            {
                bool availiable = CheckConnectionToServerInternal(_mySqlConfig.Ip, _mySqlConfig.Port);
                if (!availiable)
                {
                    OnDataLoadingStateChanged(LauncherState.ConnectionFailed);
                }
                //Уведомляем подписчиков, что инициализация запущена
                OnDataLoadingStateChanged(LauncherState.InitializationStarted);

                Task[] tasks =
                {
                    //Загрузка  новостей 1 реалм
                    new Task(() =>
                    {
                        OnNewsLoadBanner(LoadingState.Started);
                        WebClient web = _webClientFactory.Create();

                        QueryResult<string> queryResult;
                        try
                        {
                            string bannerNews = web.DownloadString(_addressSet.LoadBannerNews);

                            queryResult = new QueryResult<string>(bannerNews != "note", bannerNews);
                        }
                        catch (Exception)
                        {
                            queryResult = new QueryResult<string>(false, null);
                        }

                        OnNewsLoadBanner(LoadingState.Completed, queryResult);
                    }),

                    //Загрузка статистики игроков
                    new Task(() =>
                    {
                        OnStatOnline(LoadingState.Started);
                        WebClient web = _webClientFactory.Create();
                        QueryResult<string> queryResult;
                        try
                        {
                            string updateStatsOnline = web.DownloadString(_addressSet.LoadStatOnline);

                            queryResult = new QueryResult<string>(updateStatsOnline != "note", updateStatsOnline);
                        }
                        catch (Exception)
                        {
                            queryResult = new QueryResult<string>(false, null);
                        }

                        OnStatOnline(LoadingState.Completed, queryResult);
                    }),

                    //Загрузка информации о персонаже
                    new Task(() =>
                    {
                        OnShowPlayerInfo(LoadingState.Started);
                        WebClient web = _webClientFactory.Create();
                        QueryResult<string> queryResult;
                        try
                        {
                            string showPlayerInfo = web.DownloadString(_addressSet.ShowPlayerInfo);

                            queryResult = new QueryResult<string>(showPlayerInfo != "note", showPlayerInfo);
                        }
                        catch (Exception)
                        {
                            queryResult = new QueryResult<string>(false, null);
                        }

                        OnShowPlayerInfo(LoadingState.Completed, queryResult);
                    }),
                    //Загрузка информации о персонаже

                    //Загрузка информации о вещах чара
                    new Task(() =>
                    {
                        OnPlayerImage(LoadingState.Started);
                        WebClient web = _webClientFactory.Create();
                        QueryResult<string> queryResult;
                        try
                        {
                            string playerImage = web.DownloadString(_addressSet.PlayerImage);

                            queryResult = new QueryResult<string>(playerImage != "note", playerImage);
                        }
                        catch (Exception)
                        {
                            queryResult = new QueryResult<string>(false, null);
                        }

                        OnPlayerImage(LoadingState.Completed, queryResult);
                    }),
                    //Загрузка информации о вещах чара

                    //Загрузка горячих новостей для реалма 1
                    new Task(() =>
                    {
                        OnNewsLoadStateChanged(LoadingState.Started);
                        WebClient web = _webClientFactory.Create();
                        QueryResult<string> queryResult;
                        try
                        {
                            string hotNews = web.DownloadString(_addressSet.HotNews);
                            queryResult = new QueryResult<string>(hotNews != "note", hotNews);
                        }
                        catch (Exception)
                        {
                            queryResult = new QueryResult<string>(false, null);
                        }

                        OnNewsLoadStateChanged(LoadingState.Completed, queryResult);
                    }),
                };

                //Запускаем выполнение всех задач
                Array.ForEach(tasks, (task) => { task.Start(); });
                //Ожидаем останавливая выполнение текущего потока, пока не выполнятся все задачи
                Task.WaitAll(tasks);
                //Уведомляем подписчиков, что все операции выполнены
                OnDataLoadingStateChanged(LauncherState.InitializationCompleted);
            });
        }

        /// <summary>
        /// Обновить пользователей, которые онлайн
        /// </summary>
        public void UpdateOnlinePlayers()
        {
            Task.Factory.StartNew(() =>
            {
            });
        }

        /// <summary>
        /// Проверить соединение с сервером
        /// </summary>
        /// <param name="host">Адрес</param>
        /// <param name="port">Порт</param>
        /// <returns></returns>
        public bool CheckConnectionToServer(string host, int port)
        {
            return CheckConnectionToServerInternal(host, port);
        }

        /// <summary>
        /// Проверить соединение с сервером асинхронно.
        /// </summary>
        /// <param name="host">Адрес</param>
        /// <param name="port">Порт</param>
        /// <returns></returns>
        public Task<bool> CheckConnectionToServerAsync(string host, int port)
        {
            return Task.Factory.StartNew(() => CheckConnectionToServerInternal(host, port));
        }

        private void UpdateOnlinePlayersCounter(string onlinePlayersAddress, string worldIp, int worldPort)
        {
            bool connected = CheckConnectionToServer(worldIp, worldPort);
            if (connected)
            {
                WebClient web = _webClientFactory.Create();
            }
            else { }
        }

        /// <summary>
        ///     Состояние инициализации изменилось
        /// </summary>
        public event EventHandler<LauncherStateEventArgs> DataLoadingStateChanged;

        /// <summary>
        /// Состояние загрузки новостей изменилось
        /// </summary>
        public event EventHandler<LoadTextEventArgs> NewsLoadStateChanged;

        /// <summary>
        /// Состояние загрузки информации об проследнем обновлении изменилось
        /// </summary>
        public event EventHandler<LoadTextEventArgs> UpdatingNewsLoadStateChanged;

        public event EventHandler<LoadTextEventArgs> NewsLoadBanner;

        public event EventHandler<LoadTextEventArgs> StatOnline;

        public event EventHandler<LoadTextEventArgs> ShowPlayerInfo;

        public event EventHandler<LoadTextEventArgs> PlayerImage;

        protected void OnDataLoadingStateChanged(LauncherState state)
        {
            var handler = DataLoadingStateChanged;
            if (handler != null)
            {
                //перенаправляем выполнение кода подписчиков в тот поток, в котором был создан экземпляр этого класса
                _syncContext.Post((unused) => { handler(this, new LauncherStateEventArgs(state)); }, null);
            }
        }

        //ЗАГРУЗКА ИНФЫ О ЧАРАХ ВСЕХ РЕАЛМОВ
        protected void OnStatOnline(LoadingState state, QueryResult<string> result = null)
        {
            var handler = StatOnline;
            if (handler != null)
            {
                _syncContext.Post((unused) => { handler(this, new LoadTextEventArgs(state, result)); }, null);
            }
        }

        // ЗАГРУЗКА ГОРЯЧИХ НОВОСТЕЙ ДЛЯ РЕАЛМОВ
        protected void OnNewsLoadStateChanged(LoadingState state, QueryResult<string> result = null)
        {
            var handler = NewsLoadStateChanged;
            if (handler != null)
            {
                //перенаправляем выполнение кода подписчиков в тот поток, в котором был создан экземпляр этого класса
                _syncContext.Post((unused) => { handler(this, new LoadTextEventArgs(state, result)); }, null);
            }
        }

        protected void OnPlayerImage(LoadingState state, QueryResult<string> result = null)
        {
            var handler = PlayerImage;
            if (handler != null)
            {
                //перенаправляем выполнение кода подписчиков в тот поток, в котором был создан экземпляр этого класса
                _syncContext.Post((unused) => { handler(this, new LoadTextEventArgs(state, result)); }, null);
            }
        }

        protected void OnShowPlayerInfo(LoadingState state, QueryResult<string> result = null)
        {
            var handler = ShowPlayerInfo;
            if (handler != null)
            {
                //перенаправляем выполнение кода подписчиков в тот поток, в котором был создан экземпляр этого класса
                _syncContext.Post((unused) => { handler(this, new LoadTextEventArgs(state, result)); }, null);
            }
        }

        protected virtual void OnUpdatingNewsLoadStateChanged(LoadingState state, QueryResult<string> result = null)
        {
            var handler = UpdatingNewsLoadStateChanged;
            if (handler != null)
            {
                //перенаправляем выполнение кода подписчиков в тот поток, в котором был создан экземпляр этого класса
                _syncContext.Post((unused) => { handler(this, new LoadTextEventArgs(state, result)); }, null);
            }
        }

        protected virtual void OnNewsLoadBanner(LoadingState state, QueryResult<string> result = null)
        {
            var handler = NewsLoadBanner;
            if (handler != null)
            {
                //перенаправляем выполнение кода подписчиков в тот поток, в котором был создан экземпляр этого класса
                _syncContext.Post((unused) => { handler(this, new LoadTextEventArgs(state, result)); }, null);
            }
        }

        private bool CheckConnectionToServerInternal(string host, int port)
        {
            if (host == null)
                throw new ArgumentNullException("host");

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(host, port);
                return socket.Connected;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (socket.Connected)
                {
                    socket.Close();
                }
                socket.Dispose();
            }
        }
    }
}