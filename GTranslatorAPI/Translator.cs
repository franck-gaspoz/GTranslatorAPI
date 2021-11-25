using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace GTranslatorAPI
{
    /// <summary>
    /// Google Trad API client
    /// </summary>
    public class Translator : ITranslator
    {
        /// <summary>
        /// net utilities
        /// </summary>
        NetUtil Net;

        /// <summary>
        /// API settings 
        /// </summary>
        public Settings Settings;

        /// <summary>
        /// prepare a new instance
        /// </summary>
        public Translator(
            Settings settings = null
            )
        {
            Settings = (settings == null) ?
                new Settings()
                : settings;
            Net = new NetUtil(Settings);
        }

        /// <summary>
        /// translate text from source and target languages codes
        /// </summary>
        /// <param name="sourceLanguage">source language code</param>
        /// <param name="targetLanguage">target language code</param>
        /// <param name="text">text to be translated</param>
        /// <returns>Translation object</returns>
        public async Task<Translation> TranslateAsync(
            Languages sourceLanguage,
            Languages targetLanguage,
            string text
            )
        {
            var splits = new List<string>() { text };
            if (Settings.SplitStringBeforeTranslate)
                splits = SplitText(text);

            Translation translation = null;

            // parallelized segments translation
            IEnumerable<Task<Translation>> trTasksQuery =
                splits.Select(textSplit => _TranslateAsync(
                    sourceLanguage, targetLanguage, textSplit));

            var trTasks = trTasksQuery.ToArray();
            await Task.WhenAll(trTasks);
            foreach (var tr in trTasks)
            {
                if (translation == null)
                    translation = tr.Result;
                else
                    translation.TranslatedText += tr.Result.TranslatedText;
            }

            return translation;
        }

        /// <summary>
        /// split text into segments according to the rules of division
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        List<string> SplitText(string txt)
        {
            txt = txt.Replace("\r\n", "\n");
            txt = txt.Replace("\n\r", "\n");
            var t = Split(new string[] { txt }, '.');
            t = Split(t, (char)10);
            t = Split(t, ';');
            t = Split(t, '!');
            t = Split(t, '?');
            for (int i = 0; i < t.Count(); i++)
                t[i] = t[i].Replace("\n", "\r\n");
            return t.ToList();
        }

        /// <summary>
        /// build segments according to the carving with a character
        /// </summary>
        /// <param name="txts"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        string[] Split(string[] txts, char c)
        {
            var r = new List<string>();
            foreach (var s in txts)
            {
                var t = s.Split(c);
                var k = t.Count();
                for (int i = 0; i < k; i++)
                {
                    var ts = t[i];
                    if (i < k - 1)
                        ts += c;
                    if (!string.IsNullOrEmpty(ts))
                        r.Add(ts);
                }
            }
            return r.ToArray();
        }

        /// <summary>
        /// translate async
        /// </summary>
        /// <param name="sourceLanguage"></param>
        /// <param name="targetLanguage"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        async Task<Translation> _TranslateAsync(
            Languages sourceLanguage,
            Languages targetLanguage,
            string text
            )
        {
            var q = GetServiceUriPathAndQuery(text, sourceLanguage, targetLanguage);
            var r = await Net.GetQueryResponseAsync(q);
            if (!string.IsNullOrWhiteSpace(r.Item1))
            {
                var o = JsonConvert.DeserializeObject(r.Item1) as JArray;
                if (o != null)
                {
                    try
                    {
                        var translatedText = o[0][0][0].Value<string>();
                        var originalText = o[0][0][1].Value<string>();
                        return new Translation()
                        {
                            TranslatedText = translatedText,
                            OriginalText = originalText
                        };
                    }
                    catch (Exception Ex)
                    {
                        throw new TranslateException($"Translate error: invalid result {o}", Ex);
                    }
                }
            }
            throw new TranslateException($"Translate error: {r.Item2}");
        }

        /// <summary>
        /// translate text from source and target languages codes
        /// </summary>
        /// <param name="sourceLanguage">source language code</param>
        /// <param name="targetLanguage">target language code</param>
        /// <param name="text">text to be translated</param>
        /// <exception cref="TranslateException">exception during translate operation</exception>
        /// <returns>Translation object</returns>
        Translation _Translate(
            Languages sourceLanguage,
            Languages targetLanguage,
            string text
            )
        {
            var q = GetServiceUriPathAndQuery(text, sourceLanguage, targetLanguage);

            var r = Net.GetQueryResponse(q);

            if (!string.IsNullOrWhiteSpace(r.Item1))
            {
                var o = JsonConvert.DeserializeObject(r.Item1) as JArray;
                if (o != null)
                {
                    try
                    {
                        var translatedText = o[0][0][0].Value<string>();
                        var originalText = o[0][0][1].Value<string>();
                        return new Translation()
                        {
                            TranslatedText = translatedText,
                            OriginalText = originalText
                        };
                    }
                    catch (Exception Ex)
                    {
                        throw new TranslateException($"Translate error: invalid result {o}", Ex);
                    }
                }
            }
            throw new TranslateException($"Translate error: {r.Item2}");
        }

        /// <summary>
        /// get uri path and query
        /// </summary>
        /// <param name="text"></param>
        /// <param name="sourceLanguage"></param>
        /// <param name="targetLanguage"></param>
        /// <returns></returns>
        string GetServiceUriPathAndQuery(
            string text,
            Languages sourceLanguage,
            Languages targetLanguage)
        {
            CheckIsNotNull(text, "text");
            var q = //Properties.Settings.Default.GTranslatorAPIURL
                Settings.GTranslatorAPIURL
                .Replace("{srcl}", LanguagesUtil.GetId(sourceLanguage))
                .Replace("{tgtl}", LanguagesUtil.GetId(targetLanguage))
                .Replace("{txt}", Net.Escape(text));
            return q;
        }

        /// <summary>
        /// translate text from source and target language names if valids
        /// </summary>
        /// <param name="sourceLanguageName">source language name</param>
        /// <param name="targetLanguageName">target language name</param>
        /// <param name="text">text to be translated</param>
        /// <returns>Translation object</returns>
        public async Task<Translation> TranslateFromNamesAsync(
            string sourceLanguageName,
            string targetLanguageName,
            string text
            )
        {
            var srcLng = LanguagesUtil.GetCode(sourceLanguageName);
            var tgtLng = LanguagesUtil.GetCode(targetLanguageName);
            CheckCodeIsValid(srcLng);
            CheckCodeIsValid(tgtLng);
            return await TranslateAsync(
                (Languages)srcLng,
                (Languages)tgtLng,
                text);
        }

        /// <summary>
        /// translate text from source and target languages ids given as string if valids
        /// </summary>
        /// <param name="sourceLanguageId">source language id</param>
        /// <param name="targetLanguageId">target language id</param>
        /// <param name="text">text to be translated</param>
        /// <returns>Translation object</returns>
        public async Task<Translation> TranslateAsync(
            string sourceLanguageId,
            string targetLanguageId,
            string text
            )
        {
            var srcLng = LanguagesUtil.GetCodeFromId(sourceLanguageId);
            var tgtLng = LanguagesUtil.GetCodeFromId(targetLanguageId);
            CheckCodeIsValid(srcLng);
            CheckCodeIsValid(tgtLng);
            return await TranslateAsync(
                (Languages)srcLng,
                (Languages)tgtLng,
                text);
        }

        /// <summary>
        /// check if an object is null. abort if true
        /// </summary>
        /// <param name="o"></param>
        /// <param name="name"></param>
        void CheckIsNotNull(object o, string name)
        {
            if (o == null)
                throw new ArgumentNullException(name);
        }

        /// <summary>
        /// check if a language code is valid. abort if false
        /// </summary>
        /// <param name="languageCode"></param>
        void CheckCodeIsValid(Languages? languageCode)
        {
            if (languageCode == null)
                throw new ArgumentNullException($"language is not defined: {languageCode}");
        }
    }
}
