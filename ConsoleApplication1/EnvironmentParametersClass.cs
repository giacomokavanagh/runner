using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace SeleniumFrameworkTry1
{
    public static class EnvironmentParametersClass
    {
        public static readonly DataSet dsEnvironment = new DataSet();
        public static string strEnvironment;
        public static DataTable dtEnvironment;
        
        static EnvironmentParametersClass()
        {
            //Store the environment XML in a dataset for later reference
            dsEnvironment = XMLClass.XMLContents(FrameworkParametersClass.XMLPath);
        }

        public static void SetdtEnvironment(String strEnvironmentInput)
        {
            strEnvironment = strEnvironmentInput;

            foreach (DataTable dtEnv in dsEnvironment.Tables)
            {
                if (String.Equals(dtEnv.ToString(), strEnvironment, StringComparison.OrdinalIgnoreCase))
                {
                    dtEnvironment = dtEnv;
                    break;
                }
            }
        }
    }
}
