using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    public static class ExceptionClass
    {
        private static List<Exception> ListOfFrameworkExceptions_ = new List<Exception>();
        private static List<Exception> ListOfTestExceptions_ = new List<Exception>();
        public static bool FrameworkExceptionRaised = false;
        public static bool TestExceptionRaised = false;
        public static bool RaiseFrameworkException = false;
        private static bool QuitTest_ = false;

        public static List<Exception> listExFramework
        {
            get
            {
                return ListOfFrameworkExceptions_;
            }
        }

        public static void addFrameworkExceptionToList(Exception exception)
        {
            ListOfFrameworkExceptions_.Add(exception);
            FrameworkExceptionRaised = true;
        }

        public static List<Exception> listExTest
        {
            get
            {
                return ListOfTestExceptions_;
            }
        }

        public static void addTestExceptionToList(Exception exception)
        {
            ListOfTestExceptions_.Add(exception);
            TestExceptionRaised = true;
        }

        public static void raiseExceptionToActiveOutput(Exception exInput)
        {
            if(RaiseFrameworkException)
            {
                addFrameworkExceptionToList(exInput);
            }
            else
            {
                addTestExceptionToList(exInput);
            }
        }

        public static bool QuitTest
        {
            get
            {
                return QuitTest_;
            }
            set
            {
                QuitTest_ = value;
            }
        }
    }
}
