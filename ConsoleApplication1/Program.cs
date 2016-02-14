using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Xml;
using System.Web;
using System.Net;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initiate logging
            HTMLFrameworkLog htmlFrameworkLog = new HTMLFrameworkLog();

            CommandAndControlClass commandClass = new CommandAndControlClass();
            commandClass.hndPopulateTestRunDataFromStarter.addHandlers(new string[] { "logFrameworkDebugEvent",
                "logAllStoredExceptionEventsToFramework", "quitFrameworkOnException" , "quitFrameworkAsNoTestsReady"});

            commandClass.hndRetrieveEnvironmentFile.addHandlers(new string[] { "logFrameworkDebugEvent",
                "logAllStoredExceptionEventsToFramework", "quitFrameworkOnException" });
            commandClass.hndRetrieveEnvironmentFile.addHandlers(new string[] { "logFrameworkDebugEvent",
                "logAllStoredExceptionEventsToFramework", "quitFrameworkOnException" });
            commandClass.hndReportResultsFromProgram.addHandlers(new string[] { "logFrameworkDebugEvent",
                "logAllStoredExceptionEventsToFramework", "quitFrameworkOnException" });
            commandClass.hndFinaliseTestRunFromProgram.addHandlers(new string[] { "logFrameworkDebugEvent",
                "logAllStoredExceptionEventsToFramework", "quitFrameworkOnException" });

            commandClass.PopulateTestRunDataFromStarter();

            if (FrameworkParametersClass.ClearFrameworkLog)
            {
                HTMLFrameworkLog.ClearDownFrameworkLog();
            }

            commandClass.retrieveEnvironmentFile();
            
            //Don't really need to subdivide environments if you can select a file that is specific to environments
            //EnvironmentParametersClass.SetdtEnvironment(row["ENVIRONMENT"].ToString());

            TestParametersClass.setTestVariables();
            TestParametersClass.setTestDataFolderAndPath(TestParametersClass.TestID.ToString());
            commandClass.retrieveTestDataFile();

            HTMLFrameworkLog.appendBeginMessage();

            BrowserClass browserClass = new BrowserClass();
            browserClass.SetBrowserName(TestParametersClass.Browser);

            TestClass testClass = new TestClass();
            testClass.hndPopulateExcelTest.addHandlers(new string[] { "logFrameworkDebugEvent", "logAllStoredExceptionEventsToFramework",
                    "quitFrameworkOnException", "ReportResultsFileOnQuitTestException", "FinaliseTestRunOnQuitTestException" });
            testClass.hndIterateThroughTestRows.addHandlers(new string[] { "logFrameworkDebugEvent", "logAllStoredExceptionEventsToFramework",
                    "quitFrameworkOnException", "ReportResultsFileOnQuitTestException", "FinaliseTestRunOnQuitTestException" });

            testClass.populateExcelTest();

            testClass.IterateThroughTestRows();

            if(!TestParametersClass.ReportSent)
            {
                commandClass.ReportResultsFromProgram();
                commandClass.FinaliseTestRunFromProgram();
            }

            HTMLFrameworkLog.appendEndMessage();
        }
    }
}
