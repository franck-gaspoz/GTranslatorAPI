using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTranslatorAPI.CLI
{
    /// <summary>
    /// commande line interface
    /// </summary>
    public class Launcher
    {
        /// <summary>
        /// quiet mode enabled if true
        /// </summary>
        static bool IsQuiet = false;

        /// <summary>
        /// main entry point
        /// </summary>
        /// <param name="args">command line arguments</param>
        public static void Main(string[] args)
        {
            new Launcher(args);
        }

        /// <summary>
        /// constructor
        /// </summary>
        public Launcher(string[] args) { 

            // 1. get the google trad api client
            var c = new GTranslatorAPIClient();

            /*
             * syntaxes: 
             * 
             * source_langid target_langid text [-q]
             *   source_langid: original text lang id
             *   target_langid: translated text lang id
             *   -q : turn off all outputs excepting errors
             * or
             *   -list : dump list of langugaes ids & names
             */

            Languages srcLangId;
            Languages tgtLangId;
            string text = null;
            bool cmdList = false;

            try
            {
                cmdList = CheckOpt(args, "-list");
                IsQuiet = CheckOpt(args, "-q");
                if (!cmdList)
                {
                    srcLangId = TryGetLangId(args, 0);
                    tgtLangId = TryGetLangId(args, 1);
                    text = TryGetArg(args, 2);
                    var r = c.Translate(
                        srcLangId,
                        tgtLangId,
                        text
                        );
                    Ln();
                    Ln(r.TranslatedText);
                } else
                {
                    Languages();
                }
            } catch (Exception Ex)
            {
                Err(Ex.Message);
            }
        }

        #region arguments

        Languages TryGetLangId(string[] args, int n)
        {
            return TryGetLangId(TryGetArg(args, n));
        }

        Languages TryGetLangId(string s)
        {
            Languages f;
            if (!Enum.TryParse<Languages>(s, out f))
                LanguagesAndAbort($"wrong language id: {s}");
            return f;
        }

        /// <summary>
        /// check if opt present in arguments
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="opt">searched opt</param>
        /// <returns>true if found, else false/returns>
        bool CheckOpt(string[] args, string opt)
        {
            var r = false;
            foreach (var a in args)
                if (a.ToLower() == opt)
                    return true;
            return r;
        }

        /// <summary>
        /// try to obtain arg of index n from args array
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="n">arg index (0 based)</param>
        /// <returns>arg value if found, else exception</returns>
        string TryGetArg(string[] args, int n)
        {
            if (n < args.Length)
                return args[n];
            else
                UsageAndAbort("wrong number of arguments");
            return null;
        }

        #endregion

        #region traces

        /// <summary>
        /// about...
        /// </summary>
        void Info()
        {
            Ln("GTranslatorAPI command line interface " + this.GetType().Assembly.GetName().Version);
            Ln("(c)MIT 2020 franck.gaspoz@gmail.com");
            Ln();
        }

        /// <summary>
        /// dump available languages id and names
        /// </summary>
        void Languages()
        {
            Ln();
            Ln("available languages:");
            Ln();
            var d = LanguagesUtil.GetLanguagesCodesToNames();
            foreach ( var kvp in d )
            {
                var s = (kvp.Key + "").PadRight(10) + " : " + kvp.Value;
                Ln(s);
            }
            Ln();
        }

        /// <summary>
        /// print program usage
        /// </summary>
        void Usage()
        {
            Info();
            //Ln();
            Ln("command line syntax:");
            Ln("source_langid target_langid text[-q]");
            Ln("    source_langid : original text lang id");
            Ln("    target_langid : translated text lang id");
            Ln("    -q : turn off all outputs excepting errors");
            Ln("or");
            Ln("-list : dump list of languages ids & names");
            Ln();
        }

        /// <summary>
        /// print usage and abort program
        /// </summary>
        /// <param name="s">abort message</param>
        void UsageAndAbort(string s)
        {
            Usage();
            Abort(s);
        }

        /// <summary>
        /// print usage and abort program
        /// </summary>
        /// <param name="s">abort message</param>
        void LanguagesAndAbort(string s)
        {
            Languages();
            Abort(s);
        }

        /// <summary>
        /// abort program
        /// </summary>
        /// <param name="s">abort message</param>
        void Abort(string s)
        {
            Ln(s);
            Environment.Exit(0);
        }

        /// <summary>
        /// output method
        /// </summary>
        /// <param name="s">string to be outputed</param>
        void Ln(string s="")
        {
            if (!IsQuiet)
                Console.WriteLine(s);
        }

        /// <summary>
        /// output error method
        /// </summary>
        /// <param name="s">string to be outputed</param>
        void Err(string s)
        {
            Console.Error.WriteLine(s);
        }

        #endregion
    }
}
