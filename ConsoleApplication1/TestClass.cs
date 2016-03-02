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
    class TestClass
    {
        private DataSet TestDataSet_ = new DataSet();
        private DataTable TestDataTable_ = new DataTable();
        public EventHandlersClass hndPopulateExcelTest = new EventHandlersClass();
        public EventHandlersClass hndIterateThroughTestRows = new EventHandlersClass();
        private Dictionary<string, string> Properties_;

        public Dictionary<string, string> Properties
        {
            get
            {
                Properties_ = new Dictionary<string, string>();
                Properties_.Add("TestName", TestParametersClass.TestName);
                Properties_.Add("TestDataPath", TestParametersClass.TestDataPath);
                Properties_.Add("TestInputFileName", TestParametersClass.TestDataFilename);
                Properties_.Add("TestStatus", TestParametersClass.TestStatus);
                Properties_.Add("TestCompleted", TestParametersClass.TestCompleted);
                return Properties_;
            }
        }

        public void populateExcelTest()
        {
            //Copy down a reference version to the results directory
            FileManagerClass fileManager = new FileManagerClass();
            fileManager.hndCopyFileToLocation.addHandlers(new string[] { "logFrameworkDebugEvent", "logAllStoredExceptionEventsToFramework",
                    "quitFrameworkOnException" });
            fileManager.copyFileToLocation(TestParametersClass.TestDataFolder, TestParametersClass.TestBeingRunFolder, TestParametersClass.TestDataFilename);

            ExcelClass TestExcelImport = new ExcelClass();
            TestExcelImport.hndImportExcelXLS.addHandlers(new string[] { "logFrameworkDebugEvent", "raiseExceptionForEmptyDatasetToActiveOutput",
                    "logAllStoredExceptionEventsToFramework", "quitFrameworkOnException" });
            TestDataSet_ = TestExcelImport.ImportExcelXLS(TestParametersClass.TestDataPath, true);

            TestDataTable_ = TestDataSet_.Tables[0];

            hndPopulateExcelTest.fire(this);
        }

        public void IterateThroughTestRows()
        {
            StepClass stepClass = new StepClass();
            
            if (FrameworkParametersClass.TakeScreenshots)
            {
                stepClass.hndExecuteStep.addHandlers(new string[] { "TakeScreenshot", "failTestOnException", "failOrPassStepOnExceptionOrNot", "raiseQuitTestOnException",
                "blockTestOnQuitTestException", "blockStepOnQuitTestException", "logStepEventToTest", "logAllStoredExceptionEventsToTest",
                "logAllStoredExceptionEventsToStepDetailsExceptionList", "logQuitTestExceptionToTest", "logQuitTestExceptionToStepDetails"
                , "incrementStepsResultsCounterWithStepResult", "addCurrentStepStatusToCurrentStepDetails",
                "addCurrentStepDetailsToListOfTestStepDetails",
                 "resetCurrentStepDetails" });
            }
            else
            {
                stepClass.hndExecuteStep.addHandlers(new string[] { "failTestOnException", "failOrPassStepOnExceptionOrNot", "raiseQuitTestOnException",
                "blockTestOnQuitTestException", "blockStepOnQuitTestException", "logStepEventToTest", "logAllStoredExceptionEventsToTest",
                "logAllStoredExceptionEventsToStepDetailsExceptionList", "logQuitTestExceptionToTest", "logQuitTestExceptionToStepDetails"
                , "incrementStepsResultsCounterWithStepResult", "addCurrentStepStatusToCurrentStepDetails",
                "addCurrentStepDetailsToListOfTestStepDetails",
                 "resetCurrentStepDetails" });
            }
            

            //Switch the default exception reporting over to the test until it is finished
            ExceptionClass.RaiseFrameworkException = false;
            ExceptionClass.QuitTest = false;
            TestParametersClass.TestStatus = "Started";

            Directory.CreateDirectory(TestParametersClass.TestBeingRunFolder);
            TestParametersClass.strStartTime = StringClass.removeZFromEndOfString(DateTime.Now.ToString("u"));
            TestReportHTML TestReport = new TestReportHTML();

            foreach (DataRow row in TestDataTable_.Rows)
            {
                //This should end the test if there are extra rows in the excel that are empty
                if (row[1].ToString() != "")
                {
                    stepClass.executeStep(row);
                    if (ExceptionClass.QuitTest)
                    {
                        break;
                    }
                }
            }
            
            TestParametersClass.strEndTime = StringClass.removeZFromEndOfString(DateTime.Now.ToString("u"));

            ExceptionClass.RaiseFrameworkException = true;
            //Switch the default exception reporting over to the framework once the test is finished

            if (!ExceptionClass.TestExceptionRaised)
            {
                TestParametersClass.TestStatus = "Passed";
            }

            if (!ExceptionClass.QuitTest)
            {
                TestParametersClass.TestCompleted = "Passed";
            }
            else
            {
                TestParametersClass.TestCompleted = "Failed";
            }

            totalTestStepResults();

            TestReportHTML.updateSummaryDetails();

            hndIterateThroughTestRows.fire(this);
        }

        private void totalTestStepResults()
        {
            int TotalSteps = TestDataTable_.Rows.Count;
            int StepsNotExecuted = TotalSteps - (TestParametersClass.StepsPassed + TestParametersClass.StepsFailed);
            TestParametersClass.StepsBlocked = StepsNotExecuted;
        }
    }
}
