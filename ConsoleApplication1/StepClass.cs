using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.Security.Permissions;
using System.Runtime.Remoting;

namespace Runner
{
    class StepClass
    {
        private BrowserClass browserClass_ = new BrowserClass();
        private string strStepID_, Method_, Attribute_, Value_, Input_;
        private bool HasParameters_;
        private object[] oParameters_;
        DelegateHandlerClass DelegateHandler = new DelegateHandlerClass();
        public EventHandlersClass hndExecuteStep = new EventHandlersClass();
        private Dictionary<string, string> Properties_;

        public Dictionary<string, string> Properties
        {
            get
            {
                Properties_ = new Dictionary<string, string>();
                Properties_.Add("Step ID", strStepID_);
                Properties_.Add("Step Status", TestParametersClass.StepStatus);
                Properties_.Add("Method", Method_);
                Properties_.Add("Attribute", Attribute_);
                Properties_.Add("Value", Value_);
                Properties_.Add("Input", Input_);
                Properties_.Add("Step Start Time", TestParametersClass.strStepStartTime);
                Properties_.Add("Step End Time", TestParametersClass.strStepEndTime);
                return Properties_;
            }
        }

        public void executeStep(DataRow StepRow)
        {
            //Group assign the private variables for step class
            assignPrivateStepVariablesFromDataRow(StepRow);
            updateCurrentStepDetailsWithStepData();

            string MethodFound = Method_;

            try
            {
                MethodFound = DelegateHandler.findMethodNameInAvailableClassesAndFixCaseErrors(Method_);
            }
            catch (Exception exception)
            {
                ExceptionClass.raiseExceptionToActiveOutput(exception);
            }

            DelegateHandler.setMethodInfo(MethodFound);

            TestParametersClass.strStepID = strStepID_;
            TestParametersClass.setScreenshotFilePath();
            TestParametersClass.strStepStartTime = StringClass.removeZFromEndOfString(DateTime.Now.ToString("u"));
            if (HasParameters())
            {
                DelegateHandler.executeMethodWithParameters(oParameters_);
            }
            else
            {
                DelegateHandler.executeMethodWithoutParameter();
            }
            TestParametersClass.strStepEndTime = StringClass.removeZFromEndOfString(DateTime.Now.ToString("u"));

            TestParametersClass.CurrentStepDetails.strStepStartTime = TestParametersClass.strStepStartTime;
            TestParametersClass.CurrentStepDetails.strStepEndTime = TestParametersClass.strStepEndTime;
            TestParametersClass.CurrentStepDetails.StepStatus = TestParametersClass.StepStatus;

            hndExecuteStep.fire(this);
        }

        private void assignPrivateStepVariablesFromDataRow(DataRow row)
        {
            strStepID_ = row["STEP_ID"].ToString();
            Method_ = row["METHOD"].ToString();
            Attribute_ = row["ATTRIBUTE"].ToString();
            Value_ = row["VALUE"].ToString();
            Input_ = row["INPUT"].ToString();

            oParameters_ = new object[] { Attribute_, Value_, Input_ };
        }

        private void updateCurrentStepDetailsWithStepData()
        {
            TestParametersClass.CurrentStepDetails.strStepID = strStepID_;
            TestParametersClass.CurrentStepDetails.Method = Method_;
            TestParametersClass.CurrentStepDetails.Attribute = Attribute_;
            TestParametersClass.CurrentStepDetails.Value = Value_;
            TestParametersClass.CurrentStepDetails.Input = Input_;
        }

        private bool HasParameters()
        {
            HasParameters_ = true;
            if (Attribute_ == "" & Value_ == "" & Input_ == "")
            {
                HasParameters_ = false;
            }
            return HasParameters_;
        }
    }
}
