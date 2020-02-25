using System.Net;
using System.Net.Cache;

namespace WowSuite.Launcher
{
    public class WebClientFactory
    {
        /// <summary>
        /// Создать экземпляр настроенного веб клиента
        /// </summary>
        /// <returns></returns>
        public WebClient Create()
        {
            var web = new WebClient();
            web.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            return web;
        }
    }
}