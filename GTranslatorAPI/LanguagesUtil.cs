using System;
using System.Collections.Generic;

namespace GTranslatorAPI
{
    /// <summary>
    /// utilities on languages codes and names
    /// </summary>
    public static class LanguagesUtil
    {
        static public string GetId(Languages languageCode)
        {
            var normalizedLanguageCode = (languageCode.ToString()).Replace("_", "-");
            if (normalizedLanguageCode == "ice")
                normalizedLanguageCode = "is";
            return normalizedLanguageCode;
        }

        static public Languages? GetLanguageCode(string languageName)
        {
            InitNameToCode();
            if (!_languageNameToLanguageCode!.TryGetValue(languageName, out var languageCode))
                return null;
            return languageCode;
        }

        static public Languages? GetLanguageCodeFromLanguageId(string languageId)
        {
            var s = languageId.Replace("-", "_");
            if (s == "is")
                s = "ice";
            if (!Enum.TryParse<Languages>(s, out var languageCode))
                return null;
            return languageCode;
        }

        static void InitNameToCode()
        {
            if (_languageNameToLanguageCode == null)
            {
                _languageNameToLanguageCode = new Dictionary<string, Languages>();
                foreach (var kvp in _languageCodeToLanguageName)
                    _languageNameToLanguageCode[kvp.Value] = kvp.Key;
            }
        }

        public static Dictionary<Languages, string> GetLanguagesCodesToNames()
        {
            var d = new Dictionary<Languages, string>();
            foreach (var kvp in _languageCodeToLanguageName)
                d[kvp.Key] = kvp.Value;
            return d;
        }

        static Dictionary<string, Languages>? _languageNameToLanguageCode;

        static readonly Dictionary<Languages, string> _languageCodeToLanguageName
            = new()
            {
                { Languages.af, "Afrikaans" },
                { Languages.auto, "Auto" },
                { Languages.sq, "Albanian" },
                { Languages.ar, "Arabic" },
                { Languages.az, "Azerbaijani" },
                { Languages.eu, "Basque" },
                { Languages.be, "Belarusian" },
                { Languages.bn, "Bengali" },
                { Languages.bg, "Bulgarian" },
                { Languages.ca, "Catalan" },
                { Languages.zh_CN, "Chinese Simplified" },
                { Languages.zh_TW, "Chinese Traditional" },
                { Languages.hr, "Croatian" },
                { Languages.cs, "Czech" },
                { Languages.da, "Danish" },
                { Languages.nl, "Dutch" },
                { Languages.en, "English" },
                { Languages.eo, "Esperanto" },
                { Languages.et, "Estonian" },
                { Languages.tl, "Filipino" },
                { Languages.fi, "Finnish" },
                { Languages.fr, "French" },
                { Languages.gl, "Galician" },
                { Languages.ka, "Georgian" },
                { Languages.de, "German" },
                { Languages.el, "Greek" },
                { Languages.gu, "Gujarati" },
                { Languages.ht, "Haitian Creole" },
                { Languages.iw, "Hebrew" },
                { Languages.hi, "Hindi" },
                { Languages.hu, "Hungarian" },
                { Languages.ice, "Icelandic" },
                { Languages.id, "Indonesian" },
                { Languages.ga, "Irish" },
                { Languages.it, "Italian" },
                { Languages.ja, "Japanese" },
                { Languages.kn, "Kannada" },
                { Languages.ko, "Korean" },
                { Languages.la, "Latin" },
                { Languages.lv, "Latvian" },
                { Languages.lt, "Lithuanian" },
                { Languages.mk, "Macedonian" },
                { Languages.ms, "Malay" },
                { Languages.mt, "Maltese" },
                { Languages.no, "Norwegian" },
                { Languages.fa, "Persian" },
                { Languages.pl, "Polish" },
                { Languages.pt, "Portuguese" },
                { Languages.ro, "Romanian" },
                { Languages.ru, "Russian" },
                { Languages.sr, "Serbian" },
                { Languages.sk, "Slovak" },
                { Languages.sl, "Slovenian" },
                { Languages.es, "Spanish" },
                { Languages.sw, "Swahili" },
                { Languages.sv, "Swedish" },
                { Languages.ta, "Tamil" },
                { Languages.te, "Telugu" },
                { Languages.th, "Thai" },
                { Languages.tr, "Turkish" },
                { Languages.uk, "Ukrainian" },
                { Languages.ur, "Urdu" },
                { Languages.vi, "Vietnamese" },
                { Languages.cy, "Welsh" },
                { Languages.yi, "Yiddish" }
            };
    }
}
