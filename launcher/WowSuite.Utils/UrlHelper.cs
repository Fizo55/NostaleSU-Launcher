using System.Linq;

namespace WowLauncher.Utils
{
    public static class UrlHelper
    {
        /// <summary>
        /// Составить путь из сегментов разделяя каждый сегмент слэшем. В конце слэш не ставится.
        /// </summary>
        /// <param name="parts">Части/сегменты пути.</param>
        /// <returns>Возвращает строковое представление пути.</returns>
        public static string Combine(params string[] parts)
        {
            if (parts == null || parts.Length == 0) return string.Empty;
            string urlResult = string.Empty;
            urlResult = parts.Aggregate(urlResult,
                (current, segment) =>
                {
                    return string.Format("{0}{1}{2}", current, current != string.Empty ? "/" : "", segment.Trim('/'));
                });

            return urlResult;
        }
    }
}
