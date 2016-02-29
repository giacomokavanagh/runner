using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Reflection;

namespace Runner
{
    public static class FrameworkParametersClass
    {
        public static readonly string ProjectRootFolder, TestsFolder, ResultsFolder, LogFolder, CommandExcelPath,
            MachineName, ControlDB, FrameworkLogPath, RuntimeCSSPath, HostAddress, EnvironmentsFolder;
        public static readonly string[] InputClasses;
        public static readonly bool UseControlDB;
        public static string XMLPath;
        public static bool ClearFrameworkLog;

        static FrameworkParametersClass()
        {
            ProjectRootFolder = Environment.GetEnvironmentVariable("AF_FRAMEWORK_ROOT_PATH");
            TestsFolder = Path.Combine(ProjectRootFolder, "Tests");
            ResultsFolder = Path.Combine(ProjectRootFolder, "Results");
            LogFolder = Path.Combine(ProjectRootFolder, "Log");
            FrameworkLogPath = Path.Combine(LogFolder, "FrameworkLog.html");
            EnvironmentsFolder = Path.Combine(ProjectRootFolder, "Environments");
            
            //strCommandExcelPath = Path.Combine(strProjectRootFolder, "ControlTable.xlsx");
            MachineName = Environment.MachineName;
            HostAddress = Environment.GetEnvironmentVariable("AF_HOST_ADDRESS");

            //This will reset in CommandAndControlClass
            //UseControlDB = true;
            //if (UseControlDB)
            //{
            //    ControlDB = Environment.GetEnvironmentVariable("AF_CONTROL_DB");
            //}

            //This is the list of all the classes that involve input and whose methods can be used by a user through the input method
            InputClasses = new string[] { "BrowserClass" };
        }

        public static void SetEnvironmentXMLPath(string EnvironmentXMLFileName)
        {
            XMLPath = Path.Combine(EnvironmentsFolder, EnvironmentXMLFileName);
        }
    }
}
