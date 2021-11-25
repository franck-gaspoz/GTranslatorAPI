using System;
using System.Collections.Generic;

namespace GTranslatorAPI
{
    /// <summary>
    /// utilities on languages codes and names
    /// </summary>
    public static class LanguagesUtil
    {
        static public string GetName(Languages languageCode)
        {
            string s = null;
            CodeToName.TryGetValue(languageCode, out s);
            return s;
        }

        static public string GetId(Languages languageCode)
        {
            var s = (languageCode + "").Replace("_", "-");
            if (s == "ice")
                s = "is";
            return s;
        }

        static public string GetId(string languageName)
        {
            var lc = GetCode(languageName);
            if (lc == null) return null;
            var c = (Languages)lc;
            return GetId(c);
        }

        static public Languages? GetCode(string languageName)
        {
            InitNameToCode();
            Languages l;
            if (!NameToCode.TryGetValue(languageName, out l))
                return null;
            return l;
        }

        static public Languages? GetCodeFromId(string languageId)
        {
            var s = languageId.Replace("-", "_");
            if (s == "is")
                s = "ice";
            Languages l;
            if (!Enum.TryParse<Languages>(s, out l))
                return null;
            return l;
        }

        static void InitNameToCode()
        {
            if (NameToCode == null)
            {
                NameToCode = new Dictionary<string, Languages>();
                foreach (var kvp in CodeToName)
                    NameToCode[kvp.Value] = kvp.Key;
            }
        }

        public static Dictionary<Languages, string> GetLanguagesCodesToNames()
        {
            var d = new Dictionary<Languages, string>();
            foreach (var kvp in CodeToName)
                d[kvp.Key] = kvp.Value;
            return d;
        }

        public static Dictionary<string, Languages> GetLanguagesNamesToCodes()
        {
            var d = new Dictionary<string, Languages>();
            InitNameToCode();
            foreach (var kvp in NameToCode)
                d[kvp.Key] = kvp.Value;
            return d;
        }

        static Dictionary<string, Languages> NameToCode = null;

        static Dictionary<Languages, string>
            CodeToName = new Dictionary<Languages, string>()
            {
                { Languages.af, "Afrikaans" },
                { Languages.auto, "Auto"},
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
