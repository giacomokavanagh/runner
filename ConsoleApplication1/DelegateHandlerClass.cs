using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Runner
{
    class DelegateHandlerClass
    {
        private MethodInfo[] miArray_;
        private MethodInfo mi_;
        private Type tInputClass_;
        public delegate void objectArrayMethodDelegate(object[] oArr);

        public string findMethodNameInAvailableClassesAndFixCaseErrors(string MethodInfo)
        {
            bool MethodAvailable = false;
            foreach (string AvailableClassName in FrameworkParametersClass.InputClasses)
            {
                setDelegateClass(AvailableClassName);
                miArray_ = tInputClass_.GetMethods();
                foreach (MethodInfo mi in miArray_)
                {
                    string miString = mi.Name;
                    if (MethodInfo.ToUpper() == miString.ToUpper())
                    {
                        if (MethodInfo != miString)
                        {
                            MethodInfo = miString;
                        }
                        MethodAvailable = true;
                        break;
                    }
                }

                if (MethodAvailable)
                {
                    break;
                }
            }
            if (MethodAvailable == false)
            {
                MethodInfo = null;
                ExceptionClass.raiseExceptionToActiveOutput(new Exception("I can't find any function that supports that input <"
                        + MethodInfo + ">"));
            }
            return MethodInfo;
        }

        //Can't check all input errors, but check the class is in the list, and correct any capitalisation problems
        public string checkClassAvailableAndFixCaseErrors(string Class)
        {
            bool ClassAvailable = false;
            foreach (string AvailableClassName in FrameworkParametersClass.InputClasses)
            {
                if (AvailableClassName.ToUpper() == Class.ToUpper())
                {
                    if (Class != AvailableClassName)
                    {
                        Class = AvailableClassName;
                    }
                    ClassAvailable = true;
                    break;
                }
            }
            if (ClassAvailable == false)
            {
                ExceptionClass.raiseExceptionToActiveOutput(new Exception("Class from excel sheet is not available for input <"
                    + Class + ">"));
            }
            return Class;
        }

        public void setDelegateClass(string InputClass)
        {
            //Make input class a usable object
            tInputClass_ = TypeDelegator.GetType("Runner." + InputClass);
        }

        public string findMethodNameInClassAndFixCaseErrors(string MethodInfo)
        {
            miArray_ = tInputClass_.GetMethods();

            bool MethodAvailable = false;
            
            foreach (MethodInfo mi in miArray_)
            {
                string miString = mi.Name;
                if (MethodInfo.ToUpper() == miString.ToUpper())
                {
                    if (MethodInfo != miString)
                    {
                        MethodInfo = miString;
                    }
                    MethodAvailable = true;
                    break;
                }
            }
            if (MethodAvailable == false)
            {
                ExceptionClass.raiseExceptionToActiveOutput(new Exception("Method from excel sheet is not available for input <"
                        + MethodInfo + ">"));
            }
            return MethodInfo;
        }

        public void setMethodInfo(string MethodInfo)
        {
            mi_ = tInputClass_.GetMethod(MethodInfo);
        }

        public MethodInfo getMethodInfo(string MethodInfo)
        {
            return tInputClass_.GetMethod(MethodInfo);
        }

        public void executeMethodWithParameters(object[] oInputParameters)
        {
            objectArrayMethodDelegate myParamateredAction;
            try
            {
                myParamateredAction = (objectArrayMethodDelegate)Delegate.CreateDelegate(typeof(objectArrayMethodDelegate), null, mi_);
                myParamateredAction(oInputParameters);
            }
            catch (Exception exception)
            {
                ExceptionClass.raiseExceptionToActiveOutput(exception);
            }
        }

        public void executeMethodWithParametersAndMethodInfo(object[] oInputParameters, MethodInfo miInput)
        {
            objectArrayMethodDelegate myParamateredAction;
            try
            {
                myParamateredAction = (objectArrayMethodDelegate)Delegate.CreateDelegate(typeof(objectArrayMethodDelegate), null, miInput);
                myParamateredAction(oInputParameters);
            }
            catch (Exception exception)
            {
                ExceptionClass.raiseExceptionToActiveOutput(exception);
            }
            
        }

        public void executeMethodWithoutParameter()
        { 
            try
            {
                Action myParameterlessAction = (Action)Delegate.CreateDelegate(typeof(Action), mi_);
                myParameterlessAction.Invoke();
            }
            catch (Exception exception)
            {
                ExceptionClass.raiseExceptionToActiveOutput(exception);
            }
        }
    }
}
