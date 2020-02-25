using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WowSuite.Utils.News.ViewModels
{
    public class ExpressNewsItemViewModel : ExpressNewsItem
    {
        public ExpressNewsItemViewModel(ExpressNewsItem item, IDictionary<string, string> correlations)
        {
            Item = new ExpressNewsItem();
            if (correlations != null && !string.IsNullOrEmpty(item.Realms) && correlations.ContainsKey(item.Realms))
            {
                RealmsFriendly = correlations[item.Realms];
            }
            else
            {
                RealmsFriendly = item.Realms;
            }
        }

        public string RealmsFriendly { get; protected set; }

        public ExpressNewsItem Item { get; protected set; }
    }
}
