using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Xml;
using System.Web;



namespace Runner
{
    class ExcelClass
    {
        private string strFilename_, strConn_;
        private bool blnHasHeaders_;
        public DataSet ds = new DataSet();
        public EventHandlersClass hndImportExcelXLS = new EventHandlersClass();
        private Dictionary<string, string> dictProperties_;

        public Dictionary<string, string> Properties
        {
            get
            {
                dictProperties_ = new Dictionary<string, string>();
                dictProperties_.Add("strFilename_", strFilename_);
                dictProperties_.Add("blnHasHeaders_", blnHasHeaders_.ToString());
                dictProperties_.Add("strConn_", strConn_);
                return dictProperties_;
            }
        }

        public DataSet ImportExcelXLS(string strFilename, bool blnHasHeaders)
        {
            strFilename_ = strFilename;
            blnHasHeaders_ = blnHasHeaders;

            string HDR = blnHasHeaders ? "Yes" : "No";
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFilename + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=1\"";
            strConn_ = strConn;

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                conn.Open();

                DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                foreach (DataRow row in dt.Rows)
                {
                    string sheet = row["TABLE_NAME"].ToString();

                    OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheet + "]", conn);
                    cmd.CommandType = CommandType.Text;

                    DataTable outputTable = new DataTable(sheet);
                    ds.Tables.Add(outputTable);
                    new OleDbDataAdapter(cmd).Fill(outputTable);
                }
            }
            hndImportExcelXLS.fire(this);

            return ds;
        }
    }
}