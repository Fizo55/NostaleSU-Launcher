using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using WowLauncher.Utils;
using WowSuite.Utils;
using WowSuite.Utils.Patching;
using MessageBox = System.Windows.MessageBox;

namespace WowSuite.Launcher.Logic.Updating
{
    internal class WowUpdater
    {
        /// <summary>Корневая папка, в которой находится апдейтер</summary>
        private readonly string _rootDirectory;

        /// <summary>Адрес pid файла на сервере</summary>
        private readonly string _serverPidFile;

        private readonly string _localPidFile;

        /// <summary>Адрес файла c описанием файлов для обновления на сервере</summary>
        private readonly string _serverPatchListFile;

        private readonly string _serverFilesRoot;

        private readonly SynchronizationContext _syncContext;
        private readonly WebClientFactory _webClientFactory;
        private readonly WebClient _webClient;

        private UpdateState _currentState; //Текущее состояние апдейтера
        private string _tempFolder;

        private Pid _pidFromServer;

        public const string TEMP_FOLDER_NAME = "NostaleTemp";

        /// <summary>
        /// Инициализирует экземпляр класса.
        /// </summary>
        /// <param name="rootDirectory">Корневая папка, в которой происходят обновления</param>
        /// <param name="serverPidFile">Адрес pid файла на сервере</param>
        /// <param name="localPidFile">Путь к локальному Pid файлу</param>
        /// <param name="serverPatchListFile">Адрес файла c описанием файлов для обновления на сервере</param>
        /// <param name="serverFilesRoot">Корневая папка на сервере, в которой лежат файлы для обновления</param>
        public WowUpdater(string rootDirectory, string serverPidFile, string localPidFile, string serverPatchListFile, string serverFilesRoot)
        {
            if (!Directory.Exists(rootDirectory))
            {
                throw new DirectoryNotFoundException("rootDirectory");
            }

            _rootDirectory = rootDirectory;
            _serverPidFile = serverPidFile;
            _localPidFile = localPidFile;
            _serverPatchListFile = serverPatchListFile;
            _serverFilesRoot = serverFilesRoot;
            _syncContext = SynchronizationContext.Current ?? new SynchronizationContext();

            _webClientFactory = new WebClientFactory();
            _webClient = _webClientFactory.Create();

            _currentState = UpdateState.None;

            // TempFolder = rootDirectory;

            TempFolder = Path.Combine(Path.GetTempPath(), TEMP_FOLDER_NAME);
        }

        /// <summary>
        /// Путь к временной папке, в которую будут сохраняться файлы обновления.
        /// </summary>
        public string TempFolder
        {
            get { return _tempFolder; }
            set
            {
                if (_currentState != UpdateState.None)
                    throw new InvalidOperationException("Нельзя изменить временную папку в процессе работы апдейтера");

                if (value == null)
                    throw new ArgumentNullException("value");

                _tempFolder = value;
            }
        }

        /// <summary>Уведомляет об изменении состояния апдейтера</summary>
        public event EventHandler<UpdateStateEventArgs> UpdateStateChanged;

        /// <summary>Прогресс выполнения обновления изменился от 0 до 100%</summary>
        public event EventHandler<ProgressEventArgs> UpdateProgressChanged;

        public event EventHandler<InfoEventArgs> UpdateDownloadInfo;

        protected void OnUpdateStateChanged(UpdateState newState)
        {
            UpdateState oldState = _currentState;
            _currentState = newState;
            var handler = UpdateStateChanged;
            if (handler != null)
            {
                //перенаправляем выполнение кода подписчиков в тот поток, в котором был создан экземпляр этого класса
                _syncContext.Post((unused) =>
                {
                    handler(this, new UpdateStateEventArgs(newState, oldState));
                }, null);
            }
        }

        protected void OnUpdateProgressChanged(int percentage)
        {
            var handler = UpdateProgressChanged;
            if (handler != null)
            {
                //перенаправляем выполнение кода подписчиков в тот поток, в котором был создан экземпляр этого класса
                _syncContext.Post((unused) =>
                {
                    handler(this, new ProgressEventArgs(percentage));
                }, null);
            }
        }

        protected void OnUpdateDownloadInfo(string name, long size, long currentDownloaded)
        {
            float totalSize = (float)size / 1048576L;
            float remainingSize = (float)(size - currentDownloaded) / 1048576L;
            var handler = UpdateDownloadInfo;
            if (handler != null)
            {
                // redirige l'exécution du code des abonnés vers le thread où l'instance de cette classe a été créée
                _syncContext.Post((unused) =>
                {
                    handler(this, new InfoEventArgs(name, totalSize, remainingSize));
                }, null);
            }
        }

        protected bool FileExists(string path)
        {
            return File.Exists(path) ? true : false;
        }

        /// <summary>
        /// Проверить наличие обновлений на сервере.
        /// </summary>
        /// <returns></returns>
        public Task<bool> CheckUpdateAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                //Уведомляем подписчиков, что проверка наличия новой версии запущена.
                OnUpdateStateChanged(UpdateState.Checking);

                string localPidFile = _localPidFile;
                string localPidHash = null;
                if (File.Exists(localPidFile)) //если локальный файл с хэшем версии обновления найден
                {
                    localPidHash = Pid.FromTextFile(localPidFile).Hash;
                }

                WebClient web = _webClientFactory.Create();
                string pidText = web.DownloadString(_serverPidFile);
                Pid downloadedPid = Pid.FromString(pidText);

                if (localPidHash == null)
                {
                    Pid newLocalPid = Pid.FromVersionNumber(-1);
                    localPidHash = newLocalPid.Hash;
                    using (StreamWriter writer = File.CreateText(localPidFile))
                    {
                        writer.Write(newLocalPid.ToString());
                    }
                }

                OnUpdateStateChanged(UpdateState.None);
                if (downloadedPid.Hash != localPidHash)
                {
                    _pidFromServer = downloadedPid;
                    return true;
                }

                //Уведомляем подписчиков, что обновление не требуется.
                OnUpdateStateChanged(UpdateState.NotNeeded);
                OnUpdateStateChanged(UpdateState.None);
                return false;
            });
        }

        public void Update()
        {
            if (_currentState != UpdateState.None)
            {
                OnUpdateStateChanged(_currentState);
                return;
            }

            // Pour changer l'état le plus rapidement possible sans permettre de vérifier l'état avant qu'il ne change d'un autre flux
            _currentState = UpdateState.Started;

            // Démarrer l'exécution du code dans un thread à partir d'un pool de threads.
            ThreadPool.QueueUserWorkItem(async unused =>
            {
                // Informer les abonnés que la mise à jour a été lancée.
                OnUpdateStateChanged(UpdateState.Started);

                Task<string> task = Task.Factory.StartNew(() => _webClient.DownloadString(_serverPatchListFile));
                string patchlist = await task;

                var patch = new PatchInfo(patchlist);

                // Obtenez le chemin d'accès au dossier dans lequel les données du client sont mises à jour (le dossier où se trouve le dispositif de mise à jour/le lanceur et les données à mettre à jour).
                string dataDirectory = Path.Combine(_rootDirectory);
                if (!Directory.Exists(dataDirectory))
                {
                    Directory.CreateDirectory(dataDirectory);
                }

                // Obtenir tous les fichiers mpq situés sur le disque local (y compris tous les sous-dossiers).
                string[] localFiles = Directory.GetFiles(dataDirectory, ".", SearchOption.AllDirectories);

                // Filtrer uniquement les fichiers locaux dont les noms coïncident avec les noms de fichiers dans le patch
                Dictionary<string, FileInfo> matchingFiles =
                    localFiles.Where( // Filtre où
                    f => patch.UpdateFiles.Any( // le fichier local f correspond
                        pf => pf.FileName == Path.GetFileName(f) && !File.Exists(f))) // avec un fichier dans le patch nommé
                        .Select(mf => new FileInfo(mf)) // Projeter chaque élément de collection dans un nouveau type de données
                        .ToDictionary(info => info.Name); // convertir la collection filtrée en dictionnaire avec la clé "nom de fichier"

                // Dernier pourcentage d'avancement de la mise à jour
                int lastPercentValue = 0;

                // Créer un dossier temporaire s'il n'existe pas. Dossier où nous allons télécharger les nouveaux fichiers de mise à jour
                if (!Directory.Exists(TempFolder))
                    Directory.CreateDirectory(TempFolder);

                // Créer une liste comparable de fichiers téléchargés avec des informations sur ce fichier
                // précédemment reçu du serveur (à partir d'une feuille de patch)
                var downloaded = new List<Tuple<string, UpdateFile>>(patch.UpdateFiles.Length);

                // créer un compteur variable du nombre d'octets téléchargés de tous les fichiers
                long downloadedBytesLength = 0L;
                // Télécharger tous les fichiers
                for (int i = 0; i < patch.UpdateFiles.Length; i++)
                {
                    UpdateFile updateFile = patch.UpdateFiles[i];

                    string tempFile = Path.Combine(TempFolder, updateFile.FileName);

                    if (matchingFiles.ContainsKey(updateFile.FileName))
                    {
                        FileInfo file = matchingFiles[updateFile.FileName];
                        string localFileHash = HashHelper.GetMD5HashOfFile(file.FullName);
                        if (localFileHash == updateFile.Hash)
                        {
                            // on ajoute sa taille à la somme des octets téléchargés,
                            // que le pourcentage de progression du téléchargement est correctement pris en compte
                            downloadedBytesLength += updateFile.FileSize;
                            lastPercentValue = UpdateProgress(downloadedBytesLength, updateFile.FileName, patch.PatchLength, lastPercentValue, updateFile.FileSize);
                            continue; // quitter l'itération en cours du cycle et le cycle passe à l'étape suivante
                        }
                    }

                    long offset = 0L;
                    // Si le fichier existe (a été téléchargé plus tôt) et qu'il est entièrement téléchargé (le hachage correspond au hachage de la feuille de patch)
                    if (File.Exists(tempFile))
                    {
                        string hash = HashHelper.GetMD5HashOfFile(tempFile);
                        if (hash == updateFile.Hash) // vérifier les hashes.
                        {
                            // on ajoute sa taille à la somme des octets téléchargés,
                            // que le pourcentage de progression du téléchargement est correctement pris en compte
                            downloadedBytesLength += updateFile.FileSize;
                            downloaded.Add(Tuple.Create(tempFile, updateFile));
                            lastPercentValue = UpdateProgress(downloadedBytesLength, updateFile.FileName, patch.PatchLength, lastPercentValue, updateFile.FileSize);
                            continue; // quitter l'itération en cours du cycle et le cycle passe au cycle suivant
                        }
                        /*-------------   pompant -------------------------*/
                        else
                        {
                            var fileInfo = new FileInfo(tempFile);
                            offset = fileInfo.Length; // Combien d'octets ont été téléchargés. Combien vous devez déplacer lors du téléchargement.
                            downloadedBytesLength += fileInfo.Length; // les progrès seront désormais dûment pris en compte
                        }
                    }

                    Uri url = new Uri(UrlHelper.Combine(_serverFilesRoot, updateFile.FileName));
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

                    if (offset > 0L) // L signifie que les littéraux de type 0 sont longs ou Int64, ce qui est la même chose. Sans L, il y aura un type int.
                    {
                        request.AddRange(offset);
                    }
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            string message = string.Format("POST failed. Received HTTP {0}", response.StatusCode);
                            MessageBox.Show(message);
                            //throw new ApplicationException(message); Don't crash
                        }

                        Stream source = response.GetResponseStream();
                        if (source != null)
                        {
                            // пытаемся создать папки, которые отсутствуют из пути
                            var address = new Uri(Path.Combine(_rootDirectory + '/' + updateFile.FileName), UriKind.Absolute);
                            string directory = Path.GetDirectoryName(address.LocalPath);
                            Debug.Assert(directory != null);
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            byte[] buffer = new byte[400]; //102400 байт = 100 Килобайт  (400)

                            if (!FileExists(_rootDirectory + "/" + "d3dcompiler_47.dll"))
                            {
                                using (FileStream fs = new FileStream(_rootDirectory + '/' + updateFile.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                                {
                                    fs.Position = offset;
                                    while (true)
                                    {
                                        //кол-во реально прочитанных байт (буфер может быть больше,
                                        //чем на последней итерации цикла "do while" реально прочитали)
                                        int readed = await source.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                                        if (readed == 0) break;

                                        await fs.WriteAsync(buffer, 0, readed).ConfigureAwait(false);
                                        downloadedBytesLength += readed;
                                        lastPercentValue = UpdateProgress(downloadedBytesLength, updateFile.FileName, patch.PatchLength, lastPercentValue, updateFile.FileSize, fs.Length);
                                    }
                                }
                            }
                            else if (updateFile.FileName != "d3dcompiler_47.dll")
                            {
                                using (FileStream fs = new FileStream(_rootDirectory + '/' + updateFile.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                                {
                                    fs.Position = offset;
                                    while (true)
                                    {
                                        //кол-во реально прочитанных байт (буфер может быть больше,
                                        //чем на последней итерации цикла "do while" реально прочитали)
                                        int readed = await source.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                                        if (readed == 0) break;

                                        await fs.WriteAsync(buffer, 0, readed).ConfigureAwait(false);
                                        downloadedBytesLength += readed;
                                        lastPercentValue = UpdateProgress(downloadedBytesLength, updateFile.FileName, patch.PatchLength, lastPercentValue, updateFile.FileSize, fs.Length);
                                    }
                                }
                            }

                            downloaded.Add(Tuple.Create(tempFile, updateFile));
                        }
                    }
                }

                /*
                foreach (Tuple<string, UpdateFile> tuple in downloaded) //<путь к временному файлу, соответствующее описание этому файлу>
                {
                    UpdateFile updateFile = tuple.Item2;
                    string tempFile = tuple.Item1;
                    string directory = string.Empty;

                    if (updateFile.FolderName == Path.Combine(AppDomain.CurrentDomain.BaseDirectory))
                    {   // ../Data
                        directory = Path.Combine(_rootDirectory);
                    }
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    string file = Path.Combine(directory, updateFile.FileName);
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                    File.Move(tempFile, file);
                }
                */

                if (_pidFromServer != null)
                {
                    using (StreamWriter writer = File.CreateText(_localPidFile))
                    {
                        await writer.WriteAsync(_pidFromServer.ToString()).ConfigureAwait(false);
                        writer.Dispose();
                    }
                }

                OnUpdateStateChanged(UpdateState.Completed);
                OnUpdateStateChanged(UpdateState.None);
            });
        }

        /// <summary>
        /// Обновить прогресс выполнения, если прогресс выполнения изменился хотя
        /// бы на один процент, уведомить подписчиков о изменении прогресса выполнения
        /// </summary>
        /// <param name="downloadedBytesLength">Кол-во скачанных байт</param>
        /// <param name="patchLength">Общий размер всех файлов в патче</param>
        /// <param name="lastPercentValue">Предыдущее прогресса выполнения в процентах</param>
        /// <returns>Вернёт новое значение прогресса выполнения в процентах</returns>
        private int UpdateProgress(long downloadedBytesLength, string patchName, long patchLength, int lastPercentValue, long fileDownloaded, long readed = 0)
        {
            int percentage = (int)(downloadedBytesLength / (double)patchLength * 100d);
            if (percentage < 0 || percentage > 100)
            {
                Debug.WriteLine("percentage = {0}", percentage);
            }
            if (lastPercentValue != percentage)
            {
                OnUpdateProgressChanged(percentage);
            }
            OnUpdateDownloadInfo(patchName, fileDownloaded, readed);
            return percentage;
        }
    }
}