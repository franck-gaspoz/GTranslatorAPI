using System;
using System.Threading.Tasks;

namespace GTranslatorAPI.CLI
{
    /// <summary>
    /// commande line interface
    /// </summary>
    class Program
    {
        /// <summary>
        /// quiet mode enabled if true
        /// </summary>
        static bool _isQuiet = false;

        /// <summary>
        /// wait a key press before exit if true
        /// </summary>
        static bool _waitKeyBeforeExit = false;

        /// <summary>
        /// main entry point
        /// </summary>
        /// <param name="args">command line arguments</param>
        static async Task<int> Main(string[] args)
        {
            var returnCode = 0;
            try
            {
                Run(args);
            }
            catch
            {
                returnCode = 1;
            }
            if (_waitKeyBeforeExit)
                Console.ReadKey();
            return returnCode;
        }

        /// <summary>
        /// run
        /// </summary>
        static void Run(string[] args)
        {
            // 1. get the google trad api client
            var translatorAPI = new Client();

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

            bool runCommandListAction;

            try
            {
                runCommandListAction = CheckOpt(args, "--list")
                    || CheckOpt(args, "-l");
                _isQuiet = CheckOpt(args, "-q");
                _waitKeyBeforeExit = CheckOpt(args, "-w");

                if (!runCommandListAction)
                {
                    var srcLangId = TryGetLangId(args, 0);
                    var tgtLangId = TryGetLangId(args, 1);
                    var text = TryGetArg(args, 2);

                    // call translate api
                    var r = translatorAPI.Translate(
                        srcLangId,
                        tgtLangId,
                        text
                        );

                    // output result
                    Ln();
                    Ln(r.TranslatedText);
                }
                else
                    OutputLanguages();
            }
            catch (Exception Ex)
            {
                Err(Ex.Message);
            }
        }

        #region arguments

        /// <summary>
        /// try to get language id from args at position n, throws exception if fail
        /// </summary>
        /// <param name="args"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        static Languages TryGetLangId(string[] args, int n)
            => TryGetLangId(TryGetArg(args, n));

        /// <summary>
        /// try get lang id from args, throws exception if fail
        /// </summary>
        /// <param name="languageName">language name</param>
        /// <returns>language id</returns>
        static Languages TryGetLangId(string languageName)
        {
            if (!Enum.TryParse<Languages>(languageName, out var languageId))
            {
                OutputLanguages();
                throw new ArgumentException($"unknown language id: {languageName}");
            }
            return languageId;
        }

        /// <summary>
        /// check if opt present in arguments
        /// </summary>
        /// <param name="args">arguments</param>
        /// <param name="opt">searched option</param>
        /// <returns>true if found, else false/returns>
        static bool CheckOpt(string[] args, string opt)
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
        static string TryGetArg(string[] args, int n)
        {
            if (n < args.Length)
                return args[n];
            else
            {
                OutputUsage();
                throw new ArgumentException("wrong number of arguments");
            }
        }

        #endregion

        #region outputs

        /// <summary>
        /// about...
        /// </summary>
        static void OutputInfo()
        {
            Ln("GTranslator API CLI " + typeof(Program).Assembly.GetName().Version);
            Ln("(c) franck.gaspoz@gmail.com 2021 License MIT");
            Ln();
        }

        /// <summary>
        /// output available languages id and names
        /// </summary>
        static void OutputLanguages()
        {
            Ln();
            Ln("available languages:");
            Ln();
            var d = LanguagesUtil.GetLanguagesCodesToNames();
            foreach (var kvp in d)
            {
                var s = (kvp.Key + "").PadRight(10) + " : " + kvp.Value;
                Ln(s);
            }
            Ln();
        }

        /// <summary>
        /// output infos about program usage
        /// </summary>
        static void OutputUsage()
        {
            OutputInfo();
            Ln("command line syntaxes:");
            Ln();
            Ln("    source_langid target_langid text [-q] [-w]");
            Ln("        source_langid : original text lang id");
            Ln("        target_langid : translated text lang id");
            Ln("        q : turn off all outputs excepting errors");
            Ln();
            Ln("    -l | --list [-w]");
            Ln("        l | -list : dump list of languages ids & names");
            Ln();
            Ln("        w : wait a key press before exit");
            Ln();
        }

        /// <summary>
        /// output method
        /// </summary>
        /// <param name="text">string to be outputed</param>
        static void Ln(string text = "")
        {
            if (!_isQuiet)
                Console.WriteLine(text);
        }

        /// <summary>
        /// output error method
        /// </summary>
        /// <param name="text">string to be outputed</param>
        static void Err(string text)
            => Console.Error.WriteLine(text);

        #endregion
    }
}
