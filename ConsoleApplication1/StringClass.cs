using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFrameworkTry1
{
    class StringClass
    {
        public static string sanitiseDateTimeStringForFilename(string strInput)
        {
            strInput = strInput.Replace(":", "_");
            strInput = strInput.Replace("-", "_");
            strInput = strInput.Replace("Z", "");
            return strInput;
        }

        public static string removeZFromEndOfString(string strInput)
        {
            return strInput.TrimEnd('Z');
        }
    }
}
