using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Drawing.Imaging;
using OpenQA.Selenium;
using Newtonsoft.Json;

namespace SeleniumFrameworkTry1
{
    class EventHandlersClass
    {
        DelegateHandlerClass hndDelegateHandler = new DelegateHandlerClass();
        private List<dynamic> HandlersList;
        
        public EventHandlersClass()
        {
            HandlersList = new List<object>();
        }

        public void addHandlers(string[] HandlerNames)
        {
            hndDelegateHandler.setDelegateClass("EventHandlersClass");
            
            foreach (string HandlerName in HandlerNames)
            {
                HandlersList.Add(hndDelegateHandler.getMethodInfo(HandlerName));
            }
        }

        public void fire(dynamic oCaller)
        {
            object[] oArr = new object[1];
            foreach (MethodInfo mi in HandlersList)
            {
                oArr[0] = oCaller;
                hndDelegateHandler.executeMethodWithParametersAndMethodInfo(oArr, mi);
                oArr[0] = null;
            }
        }

        public static void logFrameworkDebugEvent(object[] oArr)
        {
            dynamic CallingClass = oArr[0];
            StackTrace st = new StackTrace();
            MethodBase mBase = st.GetFrame(3).GetMethod();
            string Method = mBase.ToString();
            string ClassName = mBase.ReflectedType.Name;

            HTMLFrameworkLog.appendEvent(ClassName, Method, CallingClass.Properties);
        }

        public static void raiseExceptionForEmptyDatasetToActiveOutput(object[] oArr)
        {
            dynamic CallingClass = oArr[0];
            int noOfTables = CallingClass.ds.Tables.Count;
            if (noOfTables == 0)
            {
                Exception exCustom = new Exception("There is no data in the database table.");
                ExceptionClass.raiseExceptionToActiveOutput(exCustom);
            }
        }

        public static void logThisExceptionEventToFramework(Exception exInput)
        {
            HTMLFrameworkLog.appendException(exInput.GetType().ToString(), exInput.Message);
        }

        public static void logAllStoredExceptionEventsToFramework(object[] oArr)
        {
            if (ExceptionClass.FrameworkExceptionRaised)
            {
                List<Exception> listExceptions = ExceptionClass.listExFramework;
                foreach(Exception exception in listExceptions)
                {
                    HTMLFrameworkLog.appendException(exception.GetType().ToString(), exception.Message);
                }
            }
        }

        public static void quitFrameworkOnException(object[] oArr)
        {
            if (ExceptionClass.FrameworkExceptionRaised)
            {
                HTMLFrameworkLog.appendTerminateMessage();
                Environment.Exit(0);
            }
        }

        public static void logStepEventToTest(object[] oArr)
        {
            dynamic CallingClass = oArr[0];
            StackTrace st = new StackTrace();
            MethodBase mBase = st.GetFrame(3).GetMethod();
            string Method = mBase.ToString();
            string ClassName = mBase.ReflectedType.Name;

            TestReportHTML.appendEvent(ClassName, Method, CallingClass.Properties);
        }

        public static void logThisExceptionEventToTest(Exception exInput)
        {
            if (ExceptionClass.TestExceptionRaised)
            {
                TestReportHTML.appendException(exInput.GetType().ToString(), exInput.Message);
            }
        }

        public static void logAllStoredExceptionEventsToTest(object[] oArr)
        {
            if (ExceptionClass.TestExceptionRaised)
            {
                List<Exception> listExceptions = ExceptionClass.listExTest;
                foreach (Exception exception in listExceptions)
                {
                    TestReportHTML.appendException(exception.GetType().ToString(), exception.Message);
                }
            }
        }

        public static void raiseQuitTestOnException(object[] oArr)
        {
            if (ExceptionClass.TestExceptionRaised)
            {
                ExceptionClass.QuitTest = true;
            }
        }

        public static void logQuitTestExceptionToTest(object[] oArr)
        {
            if (ExceptionClass.QuitTest)
            {
                TestParametersClass.strEndTime = StringClass.removeZFromEndOfString(DateTime.Now.ToString("u"));
                TestReportHTML.appendTerminateMessage();
                BrowserClass.closeBrowser();
            }
        }

        

        public static void failTestOnException(object[] oArr)
        {
            if (ExceptionClass.TestExceptionRaised)
            {
                TestParametersClass.TestStatus = "Failed";
            }
        }

        public static void failOrPassStepOnExceptionOrNot(object[] oArr)
        {
            if (ExceptionClass.TestExceptionRaised)
            {
                TestParametersClass.StepStatus = "Failed";
            }
            else
            {
                TestParametersClass.StepStatus = "Passed";
            }
        }

        public static void blockTestOnQuitTestException(object[] oArr)
        {
            if (ExceptionClass.QuitTest)
            {
                TestParametersClass.TestStatus = "Blocked";
            }
        }

        public static void blockStepOnQuitTestException(object[] oArr)
        {
            if (ExceptionClass.QuitTest)
            {
                TestParametersClass.StepStatus = "Blocked";
            }
        }

        public static void incrementStepsResultsCounterWithStepResult(object[] oArr)
        {
            if (TestParametersClass.StepStatus == "Passed")
            {
                TestParametersClass.StepsPassed = TestParametersClass.StepsPassed + 1;
            }
            else if (TestParametersClass.StepStatus == "Failed")
            {
                TestParametersClass.StepsFailed = TestParametersClass.StepsFailed + 1;
            }
            else if (TestParametersClass.StepStatus == "Blocked")
            {
                TestParametersClass.StepsBlocked = TestParametersClass.StepsBlocked + 1;
            }
        }

        public static void TakeScreenshot(object[] oArr)
        {
            if (TestParametersClass.BrowserOpen)
            {
                IWebDriver driver = BrowserClass.driver;
                ITakesScreenshot screenshotDriver = driver as ITakesScreenshot;
                Screenshot screenshot = screenshotDriver.GetScreenshot();
                screenshot.SaveAsFile(TestParametersClass.ScreenshotFilePath, ImageFormat.Png);
                TestParametersClass.ListOfScreenshotDetails.Add(new ScreenshotDetails
                {
                    ScreenshotFilePath = TestParametersClass.ScreenshotFilePath,
                    strStepID = TestParametersClass.strStepID
                });
            }
            
        }

        public static void ReportResultsFileOnQuitTestException(object[] oArr)
        {
            if (ExceptionClass.QuitTest)
            {
                TestParametersClass.strEndTime = StringClass.removeZFromEndOfString(DateTime.Now.ToString("u"));

                var RequestAddress = FrameworkParametersClass.HostAddress + "TestRunners/UploadResult/?id="
                    + TestParametersClass.TestRunID.ToString();

                var ListOfScreenshotDetailsJSON = JsonConvert.SerializeObject(TestParametersClass.ListOfScreenshotDetails);

                var ListOfStepDetailsJSON = JsonConvert.SerializeObject(TestParametersClass.ListOfStepDetails);

                var ListOfScreenshots = TestParametersClass.ListOfScreenshotDetails;
                string FrameworkLogFilename = "FrameworkLog" + FrameworkParametersClass.MachineName + ".html";

                string TestReportDetailsJSON = TestParametersClass.TestReportDetailsJSON();

                CommandAndControlClass.ReportResultsFile(FrameworkParametersClass.FrameworkLogPath,
                    FrameworkLogFilename, ListOfScreenshotDetailsJSON, "ListOfScreenshots", 
                    ListOfScreenshots, ListOfStepDetailsJSON, TestReportDetailsJSON, "TestReportDetails",
                    "ListOfStepDetails", RequestAddress);
            }
        }

        public void FinaliseTestRunOnQuitTestException(object[] oArr)
        {
            if (ExceptionClass.QuitTest)
            {
                CommandAndControlClass.FinaliseTestRun(FrameworkParametersClass.HostAddress, TestParametersClass.TestRunID.ToString(),
                    TestParametersClass.TestStatus, TestParametersClass.strEndTime);
            }
        }

        public static void quitFrameworkAsNoTestsReady(object[] oArr)
        {
            if (!TestParametersClass.TestReady)
            {
                HTMLFrameworkLog.appendNoTestForExecutionMessage();
                Environment.Exit(0);
            }
        }

        public static void quitBrowserDriver(object[] oArr)
        {
            if (!TestParametersClass.BrowserOpen)
            {
                HTMLFrameworkLog.appendNoTestForExecutionMessage();
                Environment.Exit(0);
            }
        }

        public static void resetCurrentStepDetails(object[] oArr)
        {
            TestParametersClass.resetCurrentStepDetails();
        }

        public static void logAllStoredExceptionEventsToStepDetailsExceptionList(object[] oArr)
        {
            if (ExceptionClass.TestExceptionRaised)
            {
                List<Exception> listExceptions = ExceptionClass.listExTest;
                TestParametersClass.CurrentStepDetails.ListOfTestExceptionDetails = new List<TestExceptionDetails>();
                foreach (Exception exception in listExceptions)
                {
                    TestParametersClass.CurrentStepDetails.ListOfTestExceptionDetails.Add(
                        new TestExceptionDetails
                        {
                            ExceptionMessage = exception.Message,
                            ExceptionType = exception.GetType().ToString()
                        });
                }
            }
        }

        public static void logQuitTestExceptionToStepDetails(object[] oArr)
        {
            if (ExceptionClass.QuitTest)
            {
                TestParametersClass.CurrentStepDetails.CatastrophicFailure = true;
            }
        }

        public static void addCurrentStepDetailsToListOfTestStepDetails(object[] oArr)
        {
            TestParametersClass.addCurrentStepDetailsToListOfStepDetails();
        }

        public static void addCurrentStepStatusToCurrentStepDetails(object[] oArr)
        {
            TestParametersClass.CurrentStepDetails.StepStatus = TestParametersClass.StepStatus;
        }
    }
}
