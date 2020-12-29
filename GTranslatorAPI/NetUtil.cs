using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

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
        /// constructeur
        /// </summary>
        internal NetUtil(
            GTranslatorAPISettings settings
            )
        {
            this.NetworkQueryTimeout = settings.NetworkQueryTimeout;
            this.UserAgent = settings.UserAgent;
        }

        /// <summary>
        /// http escape string
        /// </summary>
        /// <param name="s">string to be escaped</param>
        /// <returns>escaped string</returns>
        public string Escape(string s)
        {
            return Uri.EscapeDataString(s);
        }

        /// <summary>
        /// preform query at url and return result (or null) , status description
        /// </summary>
        /// <param name="url">uri</param>
        /// <returns>resut|null,status description</returns>
        public Tuple<string, string,Exception> GetQueryResponse(
            string url
            )
        {
            string r = null;
            try
            {
                var q = GetQuery(url);
                using (var rep = GetResponse(q))
                {
                    if (rep.StatusCode == HttpStatusCode.OK)
                    {
                        using (var sr = rep.GetResponseStream())
                        {
                            using (var str = new StreamReader(sr))
                            {
                                r = str.ReadToEnd();
                                return Tuple.Create<string, string, Exception>(r, rep.StatusDescription,null);
                            }
                        }
                    }
                    else
                        return Tuple.Create<string, string,Exception>(null, rep.StatusDescription,null);
                }
            }
            catch (Exception Ex)
            {
                return Tuple.Create<string, string,Exception>(null, Ex.Message,Ex);
            }
        }

        /// <summary>
        /// preform query at url and return result (or null) , status description
        /// </summary>
        /// <param name="url">uri</param>
        /// <returns>resut|null,status description</returns>
        public async Task<Tuple<string, string, Exception>> GetQueryResponseAsync(
            string url
            )
        {
            try
            {
                var q = GetQuery(url);
                var rep = await GetResponseAsync(q);
                return Tuple.Create<string, string, Exception>(rep, HttpStatusCode.OK+"", null);
            }
            catch (Exception Ex)
            {
                return Tuple.Create<string, string, Exception>(null, Ex.Message, Ex);
            }
        }

        /// <summary>
        /// http get at uri
        /// </summary>
        /// <param name="uri">uri</param>
        /// <param name="rethrowException">retain exceptions if true</param>
        /// <returns></returns>
        public string HTTPGet(
                    string uri,
                    bool rethrowException = true)
        {
            try
            {
                var q = GetQuery(uri);
                using (var r = GetResponse(q))
                {
                    if (r.StatusCode == HttpStatusCode.OK)
                    {
                        using (var sr = new StreamReader(r.GetResponseStream(), true))
                        {
                            var rs = sr.ReadToEnd();
                            return rs;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                if (rethrowException)
                    throw Ex;
            }
            return null;
        }

        /// <summary>
        /// obtain response from query
        /// </summary>
        /// <param name="query">query object</param>
        /// <exception cref="System.InvalidOperationException"></exception>
        /// <exception cref="System.Net.ProtocolViolationException"></exception>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <exception cref="System.Net.WebException"></exception>
        /// <returns>web repsonse</returns>
        public HttpWebResponse GetResponse(HttpWebRequest query)
        {
            return (HttpWebResponse)query.GetResponse();
        }

        /// <summary>
        /// async obtain response from query
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<string> GetResponseAsync(HttpWebRequest query)
        {
            var client = new HttpClient();
            var content = await client.GetStringAsync(query.RequestUri.AbsoluteUri);
            return content;
        }

        /// <summary>
        /// get http get query object
        /// </summary>
        /// <param name="uri">target uri</param>
        /// <returns>web request</returns>
        public HttpWebRequest GetQuery(string uri)
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
