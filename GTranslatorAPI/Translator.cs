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
        readonly NetUtil _net;

        /// <summary>
        /// API settings 
        /// </summary>
        public Settings Settings { get; protected set; }

        /// <summary>
        /// build a new instance
        /// </summary>
        public Translator( Settings? settings = null )
        {
            Settings = settings ?? new Settings();
            _net = new NetUtil(Settings);
        }

        /// <summary>
        /// translate text from source and target languages codes
        /// </summary>
        /// <param name="sourceLanguage">source language code</param>
        /// <param name="targetLanguage">target language code</param>
        /// <param name="text">text to be translated</param>
        /// <returns>Translation object</returns>
        public async Task<Translation?> TranslateAsync(
            Languages sourceLanguage,
            Languages targetLanguage,
            string text
            )
        {
            IEnumerable<string> splits;
            if (Settings.SplitStringBeforeTranslate)
                splits = SplitText(text);
            else
                splits = new List<string> { text };

            Translation? translation = null;

            // parallelized segments translation
            IEnumerable<Task<Translation>> trTasksQuery =
                splits.Select(textSplit => RunTranslateQueryAsync(
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

        static readonly char[] _splitSymbols = new char[] { '.' , ',' , ';' , '!' , '?' , '\n' };

        /// <summary>
        /// split text into segments according to the rules of division
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        static IEnumerable<string> SplitText(string text)
        {
            text = NormalizeLineBreaks(text);
            var result = new List<string>();

            int i = 0;
            int j = 0;

            while (i < text.Length && j!=-1)
            {
                foreach (var splitChar in _splitSymbols)
                {
                    if ((j = text.IndexOf(splitChar, i)) > -1)
                    {
                        result.Add(text.Substring(i, j - i + 1));
                        i = j + 1;
                        break;
                    }
                }
            }
            if (i < text.Length) result.Add(text[i..]);

            return result;
        }

        static string NormalizeLineBreaks(string text)
            => text.Replace("\r\n", "\n").Replace("\n\r", "\n");

        /// <summary>
        /// translate async
        /// </summary>
        /// <param name="sourceLanguage"></param>
        /// <param name="targetLanguage"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        async Task<Translation> RunTranslateQueryAsync(
            Languages sourceLanguage,
            Languages targetLanguage,
            string text
            )
        {
            var q = GetServiceUriPathAndQuery(text, sourceLanguage, targetLanguage);
            var r = await _net.GetQueryResponseAsync(q);
            if (!string.IsNullOrWhiteSpace(r.Item1))
            {
                if (JsonConvert.DeserializeObject(r.Item1) is JArray o)
                {
                    try
                    {
#pragma warning disable CS8602
#pragma warning disable CS8604
                        var translatedText = o[0][0][0].Value<string>();
                        var originalText = o[0][0][1].Value<string>();
#pragma warning restore CS8602
#pragma warning restore CS8604
                        return new Translation()
                        {
                            TranslatedText = translatedText,
                            OriginalText = originalText
                        };
                    }
                    catch (Exception Ex)
                    {
                        throw new TranslateException($"translate error: '{Ex.Message}': invalid result: {o}", Ex);
                    }
                }
            }
            throw new TranslateException($"translate error: {r.Item2}");
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
            var q =
                Settings.GTranslatorAPIURL
                .Replace("{srcl}", LanguagesUtil.GetId(sourceLanguage))
                .Replace("{tgtl}", LanguagesUtil.GetId(targetLanguage))
                .Replace("{txt}", NetUtil.Escape(text));
            return q;
        }

        /// <summary>
        /// translate text from source and target language names if valids
        /// </summary>
        /// <param name="sourceLanguageName">source language name</param>
        /// <param name="targetLanguageName">target language name</param>
        /// <param name="text">text to be translated</param>
        /// <returns>Translation object</returns>
        public async Task<Translation?> TranslateFromNamesAsync(
            string sourceLanguageName,
            string targetLanguageName,
            string text
            )
        {
            var srcLng = LanguagesUtil.GetLanguageCode(sourceLanguageName);
            var tgtLng = LanguagesUtil.GetLanguageCode(targetLanguageName);
            CheckCodeIsValid(srcLng);
            CheckCodeIsValid(tgtLng);
            return await TranslateAsync(
                (Languages)srcLng!,
                (Languages)tgtLng!,
                text);
        }

        /// <summary>
        /// translate text from source and target languages ids given as string if valids
        /// </summary>
        /// <param name="sourceLanguageId">source language id</param>
        /// <param name="targetLanguageId">target language id</param>
        /// <param name="text">text to be translated</param>
        /// <returns>Translation object</returns>
        public async Task<Translation?> TranslateAsync(
            string sourceLanguageId,
            string targetLanguageId,
            string text
            )
        {
            var srcLng = LanguagesUtil.GetLanguageCodeFromLanguageId(sourceLanguageId);
            var tgtLng = LanguagesUtil.GetLanguageCodeFromLanguageId(targetLanguageId);
            CheckCodeIsValid(srcLng);
            CheckCodeIsValid(tgtLng);
            return await TranslateAsync(
                (Languages)srcLng!,
                (Languages)tgtLng!,
                text);
        }

        /// <summary>
        /// check if an object is null. abort if true
        /// </summary>
        /// <param name="o"></param>
        /// <param name="name"></param>
        static void CheckIsNotNull(object? o, string name)
        {
            if (o == null)
                throw new ArgumentNullException(name);
        }

        /// <summary>
        /// check if a language code is valid. abort if false
        /// </summary>
        /// <param name="languageCode"></param>
        static void CheckCodeIsValid(Languages? languageCode)
        {
            if (languageCode == null)
                throw new ArgumentNullException($"language is not defined: {languageCode}");
        }
    }
}
