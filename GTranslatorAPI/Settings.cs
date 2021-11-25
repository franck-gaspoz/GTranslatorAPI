using System;
using System.IO;

using Newtonsoft.Json;

namespace GTranslatorAPI
{
    public class Settings
    {
        /// <summary>
        /// if true, the texts are split and translated using multiple queries
        /// </summary>
        public bool SplitStringBeforeTranslate { get; set; } = true;

        /// <summary>
        /// if true and SplitStringBeforeTranslate, the translation of segments is parrallelized
        /// </summary>
        public bool ParallelizeTranslationOfSegments { get; set; } = true;

        /// <summary>
        /// Google trad api uri (path and query)
        /// </summary>
        public string GTranslatorAPIURL { get; set; } = "https://translate.googleapis.com/translate_a/single?client=gtx&sl={srcl}&tl={tgtl}&dt=t&q={txt}";

        /// <summary>
        /// query time out (ms)
        /// </summary>
        public int NetworkQueryTimeout { get; set; } = 2000;

        /// <summary>
        /// user agent
        /// </summary>
        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36";

        /// <summary>
        /// builds a new instance of the API settings using default settings
        /// </summary>
        public Settings() { }

        /// <summary>
        /// builds a new instance of the API settings from settings file
        /// </summary>
        /// <param name="settingsFilePath">path of the file settings</param>
        public static Settings CreateFromFile(string settingsFilePath)
        {
            var settings = JsonConvert.DeserializeObject<Settings>(
                File.ReadAllText(settingsFilePath));
            if (settings == null)
                throw new InvalidOperationException("failed to create settings");
            return settings;
        }
    }
}
