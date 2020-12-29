using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTranslatorAPI
{
    public class TranslateException
        : Exception
    {
        /// <summary>
        /// exception during translate operation
        /// </summary>
        /// <param name="txt">error text</param>
        public TranslateException(string txt)
            : base(txt)
        {

        }

        /// <summary>
        /// exception during translate operation
        /// </summary>
        /// <param name="txt">error text</param>
        /// <param name="ex">inner exception</param>
        public TranslateException(string txt,Exception ex)
            : base(txt,ex)
        {

        }
    }
}
