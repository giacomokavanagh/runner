using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumFrameworkTry1
{
    class DBClass
    {
        private string strConnection_, strStoredProcedureName_;
        public DataSet ds = new DataSet();
        public EventHandlersClass hndRunStoredSelectProcedure = new EventHandlersClass();
        private Dictionary<string, string> dictProperties_;

        public DBClass()
        {
            strConnection_ = ("Data Source=" + FrameworkParametersClass.ControlDB +
                "; Initial Catalog = Starter; Integrated Security = True");
        }

        public Dictionary<string, string> Properties
        {
            get
            {
                dictProperties_ = new Dictionary<string, string>();
                dictProperties_.Add("strConnection_", strConnection_.ToString());
                dictProperties_.Add("strStoredProcedureName_", strStoredProcedureName_.ToString());
                return dictProperties_;
            }
        }

        public DataSet RunStoredSelectProcedure(string strStoredProcedureName)
        {
            using (SqlConnection conn = new SqlConnection(strConnection_))
            {
                strStoredProcedureName_ = strStoredProcedureName;
                conn.Open();
                // 1.  create a command object identifying the stored procedure
                SqlCommand cmd = new SqlCommand(strStoredProcedureName, conn);
                // 2. set the command object so it knows to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();

                da.SelectCommand = cmd;

                try
                {
                    da.Fill(ds);
                }
                catch (Exception exception)
                {
                    ExceptionClass.addFrameworkExceptionToList(exception);
                }
                conn.Close();
                // 3. add parameter to command, which will be passed to the stored procedure
                //cmd.Parameters.Add(new SqlParameter("@DEVICE_NAME", strDeviceName));

                hndRunStoredSelectProcedure.fire(this);

                return ds;
            }
        }
    }
}