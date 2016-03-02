using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;

namespace Runner
{

    class CommandAndControlClass
    {
        public DataSet dsCommand = new DataSet();

        public EventHandlersClass hndPopulateDSCommand = new EventHandlersClass();
        public EventHandlersClass hndPopulateTestRunDataFromStarter = new EventHandlersClass();
        public EventHandlersClass hndRetrieveEnvironmentFile = new EventHandlersClass();
        public EventHandlersClass hndRetrieveTestDataFile = new EventHandlersClass();
        public EventHandlersClass hndReportResultsFromProgram = new EventHandlersClass();
        public EventHandlersClass hndFinaliseTestRunFromProgram = new EventHandlersClass();

        private int TestRunID_, TestID_, TestEnvironmentID_;
        private string TestName_, TestDataFilename_, Browser_, EnvironmentsFolder_, XMLPath_, HostAddress_, RequestAddress_,
            TestDataPath_, ListOfScreenshotDetailsJSON_, MachineName_, TestStatus_, EndTime_, TestBeingRunReportFile_,
            ListOfStepDetailsJSON_;
        private bool UseDB_, TestReady_;
        private Dictionary<string, string> Properties_;

        public Dictionary<string, string> Properties
        {
            get
            {
                Properties_ = new Dictionary<string, string>();
                Properties_.Add("TestReady", TestReady_.ToString());
                Properties_.Add("UseDB", UseDB_.ToString());
                Properties_.Add("TestRunID", TestRunID_.ToString());
                Properties_.Add("TestID", TestID_.ToString());
                Properties_.Add("TestName", TestName_);
                Properties_.Add("TestDataFilename", TestDataFilename_);
                Properties_.Add("TestEnvironmentID", TestEnvironmentID_.ToString());
                Properties_.Add("Browser", Browser_);
                Properties_.Add("EnvironmentsFolder", EnvironmentsFolder_);
                Properties_.Add("XMLPath", XMLPath_);
                Properties_.Add("HostAddress", HostAddress_);
                Properties_.Add("RequestAddress", RequestAddress_);
                Properties_.Add("TestDataPath", TestDataPath_);
                Properties_.Add("ListOfScreenshotDetailsJSON", ListOfScreenshotDetailsJSON_);
                Properties_.Add("ListOfStepDetailsJSON", ListOfStepDetailsJSON_);
                Properties_.Add("MachineName", MachineName_);
                Properties_.Add("TestStatus", TestStatus_);
                Properties_.Add("EndTime", EndTime_);
                Properties_.Add("TestBeingRunReportFile", TestBeingRunReportFile_);
                return Properties_;
            }
        }

        public void populateDSCommand()
        {
            UseDB_ = FrameworkParametersClass.UseControlDB;
            if (UseDB_)
            {
                DBClass dBClass = new DBClass();
                dBClass.hndRunStoredSelectProcedure.addHandlers(new string[] { "logFrameworkDebugEvent", "raiseExceptionForEmptyDatasetToActiveOutput",
                    "logAllStoredExceptionEventsToFramework", "quitFrameworkOnException" });
                dsCommand = dBClass.RunStoredSelectProcedure("SelectTestsFromtblCommandAndControl");
            }
            else
            {
                ExcelClass excelClass = new ExcelClass();
                excelClass.hndImportExcelXLS.addHandlers(new string[] { "logFrameworkDebugEvent", "raiseExceptionForEmptyDatasetToActiveOutput",
                    "logAllStoredExceptionEventsToFramework", "quitFrameworkOnException" });
                dsCommand = excelClass.ImportExcelXLS(FrameworkParametersClass.CommandExcelPath, true);
            }
            hndPopulateDSCommand.fire(this);
        }

        public void PopulateTestRunDataFromStarter()
        {
            TestReady_ = false;

            WebRequest GetTestRun;

            HostAddress_ = FrameworkParametersClass.HostAddress;
            MachineName_ = FrameworkParametersClass.MachineName;

            GetTestRun = WebRequest.Create(HostAddress_ + "TestRunners/GetTestRunParametersForRobot/?RobotName=" + MachineName_ 
                + "&Key=" + FrameworkParametersClass.Key);

            Stream TestRunResponse;
            try
            {
                TestRunResponse = GetTestRun.GetResponse().GetResponseStream();

                StreamReader reader = new StreamReader(TestRunResponse);
                string ResponseText = reader.ReadToEnd();

                if (ResponseText != "No Test Run ready for robot")
                {
                    TestReady_ = true;
                    try
                    {
                        CommandAndControlParameters comParams = JsonConvert.DeserializeObject<CommandAndControlParameters>(ResponseText);
                        TestRunID_ = comParams.TestRunID;
                        TestParametersClass.TestRunID = TestRunID_;
                        TestID_ = comParams.TestID;
                        TestParametersClass.TestID = TestID_;
                        TestName_ = comParams.TestName;
                        TestParametersClass.TestName = TestName_;
                        TestDataFilename_ = comParams.TestDataFilename;
                        TestParametersClass.TestDataFilename = TestDataFilename_;
                        TestEnvironmentID_ = comParams.TestEnvironmentID;
                        TestParametersClass.TestEnvironmentID = TestEnvironmentID_;
                        Browser_ = comParams.Browser;
                        TestParametersClass.Browser = Browser_;
                        FrameworkParametersClass.ClearFrameworkLog = comParams.ClearRemoteLogOnNextAccess;
                        FrameworkParametersClass.TakeScreenshots = comParams.TakeScreenshots;

                        FrameworkParametersClass.SetEnvironmentXMLPath(comParams.TestEnvironmentFilename);
                    }
                    catch (Exception exception)
                    {
                        ExceptionClass.addFrameworkExceptionToList(exception);
                    }
                }
            }
            catch (Exception exception)
            {
                ExceptionClass.addFrameworkExceptionToList(exception);
            }

            TestParametersClass.TestReady = TestReady_;
            hndPopulateTestRunDataFromStarter.fire(this);
        }

        public void retrieveEnvironmentFile()
        {
            WebClient client = new WebClient();

            EnvironmentsFolder_ = FrameworkParametersClass.EnvironmentsFolder;
            if (!Directory.Exists(EnvironmentsFolder_))
            {
                Directory.CreateDirectory(EnvironmentsFolder_);
            }

            try
            {
                HostAddress_ = FrameworkParametersClass.HostAddress;
                TestEnvironmentID_ = TestParametersClass.TestEnvironmentID;
                XMLPath_ = FrameworkParametersClass.XMLPath;
                RequestAddress_ = HostAddress_ + "TestEnvironments/ReturnExternalTestEnvironmentsFile/?id=" + TestEnvironmentID_.ToString()
                    + "&Key=" + FrameworkParametersClass.Key + "&RobotName=" + FrameworkParametersClass.MachineName;

                client.DownloadFile(RequestAddress_, XMLPath_);
            }
            catch (Exception exception)
            {
                ExceptionClass.addFrameworkExceptionToList(exception);
            }

            hndRetrieveEnvironmentFile.fire(this);
        }

        public void retrieveTestDataFile()
        {
            WebClient client = new WebClient();

            if (!Directory.Exists(TestParametersClass.TestDataFolder))
            {
                Directory.CreateDirectory(TestParametersClass.TestDataFolder);
            }

            try
            {
                HostAddress_ = FrameworkParametersClass.HostAddress;
                TestID_ = TestParametersClass.TestID;
                TestDataPath_ = TestParametersClass.TestDataPath;
                RequestAddress_ = HostAddress_ + "Tests/ReturnExternalTestFile/?id=" + TestID_.ToString()
                    + "&Key=" + FrameworkParametersClass.Key + "&RobotName=" + FrameworkParametersClass.MachineName;

                client.DownloadFile(RequestAddress_, TestDataPath_);
            }
            catch (Exception exception)
            {
                ExceptionClass.addFrameworkExceptionToList(exception);
            }

            hndRetrieveTestDataFile.fire(this);
        }

        public void ReportResultsFromProgram()
        {
            HostAddress_ = FrameworkParametersClass.HostAddress;
            TestRunID_ = TestParametersClass.TestRunID;
            RequestAddress_ = HostAddress_ + "TestRunners/UploadResult/?id=" + TestRunID_.ToString() 
                + "&Key=" + FrameworkParametersClass.Key;

            ListOfScreenshotDetailsJSON_ = JsonConvert.SerializeObject(TestParametersClass.ListOfScreenshotDetails);
            var ListOfScreenshots = TestParametersClass.ListOfScreenshotDetails;

            ListOfStepDetailsJSON_ = JsonConvert.SerializeObject(TestParametersClass.ListOfStepDetails);

            var FrameworkLogFilename = "FrameworkLog" + FrameworkParametersClass.MachineName + ".html";

            var TestReportDetailsJSON = TestParametersClass.TestReportDetailsJSON();

            ReportResultsFile(FrameworkParametersClass.FrameworkLogPath, FrameworkLogFilename, ListOfScreenshotDetailsJSON_, 
                "ListOfScreenshots", ListOfScreenshots, ListOfStepDetailsJSON_, TestReportDetailsJSON, "TestReportDetails", 
                "ListOfStepDetails", RequestAddress_);

            hndReportResultsFromProgram.fire(this);
        }

        
        public static void ReportResultsFile(string FrameworkLogFilePath, string FrameworkLogFilename,
            string SerializedScreenshotDetails, string ScreenshotDetailsListName,
            List<ScreenshotDetails> ListOfScreenshots, string SerializedlistOfStepDetails, string TestReportDetailsJSON, 
            string TestReportDetailsName, string StepDetailsListName, string RequestAddress)
        {
            FileStream FrameworkLogStream = new FileStream(FrameworkLogFilePath, FileMode.Open, FileAccess.Read);
            HttpContent FrameworkLogStreamContent = new StreamContent(FrameworkLogStream);

            HttpContent httpContentSerializedReportDetails = new StringContent(TestReportDetailsJSON);

            HttpContent httpContentSerializedListOfScreenshotDetails = new StringContent(SerializedScreenshotDetails);

            HttpContent httpContentSerializedListOfStepDetails = new StringContent(SerializedlistOfStepDetails);

            using (var client = new HttpClient())
            using (var formData = new MultipartFormDataContent())
            {
                formData.Add(FrameworkLogStreamContent, FrameworkLogFilename);
                
                formData.Add(httpContentSerializedReportDetails, TestReportDetailsName);

                formData.Add(httpContentSerializedListOfStepDetails, StepDetailsListName);

                formData.Add(httpContentSerializedListOfScreenshotDetails, ScreenshotDetailsListName);

                foreach(var screenshot in ListOfScreenshots)
                {
                    var byteArrayScreenshot = File.ReadAllBytes(screenshot.ScreenshotFilePath);

                    string jsonedByteArray = JsonConvert.SerializeObject(byteArrayScreenshot);
                    HttpContent jsonContent = new StringContent(jsonedByteArray);

                    formData.Add(jsonContent, screenshot.strStepID);
                }

                var response = client.PostAsync(RequestAddress, formData).Result;

                if (!response.IsSuccessStatusCode)
                {
                    Exception exCustom = new Exception
                        ("There was no response from Starter - files might not have been uploaded.");
                    ExceptionClass.raiseExceptionToActiveOutput(exCustom);
                }
                else
                {
                    var testy = response.Content.ReadAsStreamAsync().Result;

                    TestParametersClass.ReportSent = true;
                }
            }
        }

        public void FinaliseTestRunFromProgram()
        {
            HostAddress_ = FrameworkParametersClass.HostAddress;
            TestRunID_ = TestParametersClass.TestRunID;
            TestStatus_ = TestParametersClass.TestStatus;
            EndTime_ = TestParametersClass.strEndTime;

            FinaliseTestRun(HostAddress_, TestRunID_.ToString(), TestStatus_, EndTime_);

            hndFinaliseTestRunFromProgram.fire(this);
        }

        public static void FinaliseTestRun(string HostAddress, string strTestID, string TestStatus, string EndTime)
        {
            WebRequest webRequest;

            webRequest = WebRequest.Create(HostAddress + "TestRunners/FinaliseTestRun/?id=" + strTestID
                + "&Key=" + FrameworkParametersClass.Key + "&status=" + TestStatus + "&endTime=" + EndTime);

            webRequest.GetResponse();
        }
    }

    public class CommandAndControlParameters
    {
        public int TestRunID { get; set; }
        public int TestID { get; set; }
        public string TestName { get; set; }
        public string TestDataFilename { get; set; }
        public int TestEnvironmentID { get; set; }
        public string TestEnvironmentFilename { get; set; }
        public string Browser { get; set; }
        public bool ClearRemoteLogOnNextAccess { get; set; }
        public bool TakeScreenshots { get; set; }
    }
}
