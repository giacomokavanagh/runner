using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace SeleniumFrameworkTry1
{
    public static class TestParametersClass
    {
        public static string strDateTime, ReportFilename, TestName, TestBeingRunFolder, 
            TestBeingRunReportFile, TestReportCSSPath, RuntimeTestReportCSSPath, strStartTime, strEndTime, 
            strStepStartTime, strStepEndTime, TestStatus, TestCompleted, StepStatus, strStepID, 
            ScreenshotsFolder, ScreenshotFilePath, Browser, TestDataFolder, TestDataFilename, 
            TestDataPath, LocalScreenshotFilePathForResultFile, ScreenshotFilename;
        public static int StepsPassed, StepsFailed, StepsBlocked, TestRunID, TestID, TestEnvironmentID;
        public static bool TestRunning, BrowserOpen, TestReady, ReportSent;

        public static List<ScreenshotDetails> ListOfScreenshotDetails = new List<ScreenshotDetails>();

        public static List<StepDetails> ListOfStepDetails = new List<StepDetails>();

        public static StepDetails CurrentStepDetails = new StepDetails();

        public static void setTestVariables()
        {
            strDateTime = DateTime.Now.ToString("u");
            strDateTime = StringClass.sanitiseDateTimeStringForFilename(strDateTime);
            ReportFilename = "Test Report for " + TestName + ".html";

            TestBeingRunFolder = Path.Combine(FrameworkParametersClass.ResultsFolder, TestRunID.ToString(), strDateTime);
            TestBeingRunReportFile = Path.Combine(TestBeingRunFolder, ReportFilename);
            ScreenshotsFolder = Path.Combine(TestBeingRunFolder, "Screenshots");
            Directory.CreateDirectory(ScreenshotsFolder);

            //Copy down the CSS as deleting the whole folder is pretty much default behaviour for humans
            TestReportCSSPath = Path.Combine(FrameworkParametersClass.ProjectRootFolder, "TestReport.css");
            RuntimeTestReportCSSPath = Path.Combine(FrameworkParametersClass.ResultsFolder, "TestReport.css");
            if (!File.Exists(RuntimeTestReportCSSPath))
            {
                File.Copy(TestReportCSSPath, RuntimeTestReportCSSPath);
            }

            StepsPassed = 0;
            StepsFailed = 0;
            StepsBlocked = 0;
            BrowserOpen = false;
        }

        public static void setScreenshotFilePath()
        {
            ScreenshotFilename = strStepID + ".png";
            ScreenshotFilePath = Path.Combine(ScreenshotsFolder, ScreenshotFilename);
            LocalScreenshotFilePathForResultFile = Path.Combine("Screenshots", ScreenshotFilename);
        }

        public static void setTestDataFolderAndPath(string strTestFolderName)
        {
            TestDataFolder = Path.Combine(FrameworkParametersClass.TestsFolder, strTestFolderName);
            TestDataPath = Path.Combine(TestDataFolder, TestDataFilename);
        }

        public static void resetCurrentStepDetails()
        {
            CurrentStepDetails.strStepID = null;
            CurrentStepDetails.StepStatus = null;
            CurrentStepDetails.Method = null;
            CurrentStepDetails.Attribute = null;
            CurrentStepDetails.Value = null;
            CurrentStepDetails.Input = null;
            CurrentStepDetails.strStepStartTime = null;
            CurrentStepDetails.strStepEndTime = null;
            CurrentStepDetails.ListOfTestExceptionDetails = null;
            CurrentStepDetails.CatastrophicFailure = false;
        }

        public static void addCurrentStepDetailsToListOfStepDetails()
        {
            var localStepDetails = new StepDetails();
            localStepDetails.strStepID = CurrentStepDetails.strStepID;
            localStepDetails.StepStatus = CurrentStepDetails.StepStatus;
            localStepDetails.Method = CurrentStepDetails.Method;
            localStepDetails.Attribute = CurrentStepDetails.Attribute;
            localStepDetails.Value = CurrentStepDetails.Value;
            localStepDetails.Input = CurrentStepDetails.Input;
            localStepDetails.strStepStartTime = CurrentStepDetails.strStepStartTime;
            localStepDetails.strStepEndTime = CurrentStepDetails.strStepEndTime;
            
            if(CurrentStepDetails.ListOfTestExceptionDetails != null)
            {
                localStepDetails.ListOfTestExceptionDetails = new List<TestExceptionDetails>();
                foreach(var exceptionDetails in CurrentStepDetails.ListOfTestExceptionDetails)
                {
                    localStepDetails.ListOfTestExceptionDetails.Add(
                        new TestExceptionDetails
                        {
                            ExceptionMessage = exceptionDetails.ExceptionMessage,
                            ExceptionType = exceptionDetails.ExceptionType
                        });
                }
            }

            localStepDetails.CatastrophicFailure = CurrentStepDetails.CatastrophicFailure;

            ListOfStepDetails.Add(localStepDetails);
        }

        public static string TestReportDetailsJSON()
        {
            var TestReportDetails = new TestReportDetails();
            TestReportDetails.TestName = TestName;
            TestReportDetails.TestStatus = TestStatus;
            TestReportDetails.StepsPassed = StepsPassed;
            TestReportDetails.StepsFailed = StepsFailed;
            TestReportDetails.StepsBlocked = StepsBlocked;
            TestReportDetails.strStartTime = strStartTime;
            TestReportDetails.Browser = Browser;
            TestReportDetails.strEndTime = strEndTime;
            TestReportDetails.strDuration = DateTime.Parse(strEndTime)
                .Subtract(DateTime.Parse(strStartTime)).ToString();

            return JsonConvert.SerializeObject(TestReportDetails);
        }
    }

    public class ScreenshotDetails
    {
        public string strStepID { get; set; }
        public string ScreenshotFilePath { get; set; }
    }

    public class StepDetails
    {
        public string strStepID { get; set; }
        public string StepStatus { get; set; }
        public string Method { get; set; }
        public string Attribute { get; set; }
        public string Value { get; set; }
        public string Input { get; set; }
        public string strStepStartTime { get; set; }
        public string strStepEndTime { get; set; }
        public List<TestExceptionDetails> ListOfTestExceptionDetails { get; set; }
        public bool CatastrophicFailure { get; set; }
    }

    public class TestReportDetails
    {
        public string TestName { get; set; }
        public string TestStatus { get; set; }
        public int StepsPassed { get; set; }
        public int StepsFailed { get; set; }
        public int StepsBlocked { get; set; }
        public string strStartTime { get; set; }
        public string strEndTime { get; set; }
        public string Browser { get; set; }
        public string strDuration { get; set; }
    }

    public class TestExceptionDetails
    {
        public string ExceptionType { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
