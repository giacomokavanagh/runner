using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Xml;

namespace Runner
{
    class XMLClass
    {
        public static DataSet XMLContents(String strXMLFile)
        {
            DataSet ds = new DataSet();
            ds.ReadXml(strXMLFile);
            return ds;
        }
    }
}
