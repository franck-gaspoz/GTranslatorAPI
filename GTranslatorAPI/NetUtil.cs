using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GTranslatorAPI
{
    /// <summary>
    /// internal net utils
    /// </summary>
    internal class NetUtil
    {
        /// <summary>
        /// user agent
        /// </summary>
        internal string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36";

        /// <summary>
        /// query time out
        /// </summary>
        internal int NetworkQueryTimeout { get; set; } = 2000;

        /// <summary>
        /// build a new instance
        /// </summary>
        internal NetUtil() { }

        /// <summary>
        /// build a new instance from settings
        /// </summary>
        /// <param name="settings">network settings</param>
        internal NetUtil(Settings settings)
        {
            NetworkQueryTimeout = settings.NetworkQueryTimeout;
            UserAgent = settings.UserAgent;
        }

        /// <summary>
        /// http escape string
        /// </summary>
        /// <param name="text">text to be escaped</param>
        /// <returns>escaped string</returns>
        public static string Escape(string text)
            => Uri.EscapeDataString(text);

        /// <summary>
        /// preform query at url and return result (or null), eventually exception message and object else status description
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>resut|null,status description|error message,null|exception</returns>
        public async Task<Tuple<string?, string, Exception?>> GetQueryResponseAsync(string url)
        {
            try
            {
                var q = CreateQuery(url);
                var rep = await GetResponseAsync(q);
                return Tuple.Create<string?, string, Exception?>(rep, HttpStatusCode.OK.ToString(), null);
            }
            catch (Exception Ex)
            {
                return Tuple.Create<string?, string, Exception?>(null, Ex.Message, Ex);
            }
        }

        /// <summary>
        /// async obtain response from query
        /// </summary>
        /// <param name="query">query to be performed</param>
        /// <returns>query stream result</returns>
        public static async Task<string> GetResponseAsync(HttpWebRequest query)
        {
            using var client = new HttpClient();
            var content = await client.GetStringAsync(query.RequestUri.AbsoluteUri);
            return content;
        }

        /// <summary>
        /// get http get query object
        /// </summary>
        /// <param name="uri">target uri</param>
        /// <returns>web request</returns>
        public HttpWebRequest CreateQuery(string uri)
        {
            var u = new Uri(uri);
            var query = (HttpWebRequest)WebRequest.Create(u);
            query.UserAgent = UserAgent;
            query.KeepAlive = false;
            query.Timeout = NetworkQueryTimeout;
            return query;
        }

    }
}
