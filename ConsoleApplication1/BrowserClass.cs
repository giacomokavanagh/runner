using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using System.Data;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Runtime.Remoting;


namespace Runner
{
    class BrowserClass
    {
        private static string BrowserName_;
        public static IWebDriver driver;
        private Dictionary<string, string> Properties_;
        private object[] oParameters_ = new object[3];

        public Dictionary<string, string> Properties
        {
            get
            {
                Properties_ = new Dictionary<string, string>();
                Properties_.Add("BrowserName", BrowserName_.ToString());
                Properties_.Add("Attribute", oParameters_[0].ToString());
                Properties_.Add("Value", oParameters_[1].ToString());
                Properties_.Add("Input", oParameters_[2].ToString());
                return Properties_;
            }
        }

        public void SetBrowserName(string BrowserNameInput)
        {
            BrowserName_ = BrowserNameInput.ToUpper();
        }

        public static void openBrowser()
        {
            try
            {
                switch (BrowserName_)
                {
                    case "FIREFOX":
                        driver = new FirefoxDriver();
                        break;
                    case "CHROME":
                        driver = new ChromeDriver();
                        break;
                    case "IE":
                        driver = new InternetExplorerDriver();
                        break;
                    default:
                        driver = new ChromeDriver();
                        break;
                }
                System.Threading.Thread.Sleep(5000);
                TestParametersClass.BrowserOpen = true;
            }
            catch (Exception exception)
            {
                ExceptionClass.addTestExceptionToList(exception);
            }
        }

        public static void closeBrowser()
        {
            try
            {
                driver.Close();
                driver.Quit();
                TestParametersClass.BrowserOpen = false;
            }
            catch (Exception exception)
            {
                ExceptionClass.addTestExceptionToList(exception);
            }
        }

        public static void Navigate(object[] oInputParameters)
        {
            try
            {
                driver.Navigate().GoToUrl(oInputParameters[2].ToString());
            }
            catch (Exception exception)
            {
                ExceptionClass.addTestExceptionToList(exception);
            }
        }

        public static void NavigateToEnvironmentURL()
        {
            try
            {
                driver.Navigate().GoToUrl(EnvironmentParametersClass.dsEnvironment.Tables[0].Rows[0]["url"].ToString());
            }
            catch (Exception exception)
            {
                ExceptionClass.addTestExceptionToList(exception);
            }
        }


        public static void Click(object[] oInputParameters)
        {
            string strAttribute = oInputParameters[0].ToString();
            string strValue = oInputParameters[1].ToString();
            try
            {
                findWeblement(strAttribute, strValue).Click();
            }
            catch (Exception exception)
            {
                ExceptionClass.addTestExceptionToList(new Exception("I couldn't click that object, possibly because I couldnt find it"));
            }
        }

        public static void Clear(object[] oInputParameters)
        {
            string strAttribute = oInputParameters[0].ToString();
            string strValue = oInputParameters[1].ToString();
            try
            {
                findWeblement(strAttribute, strValue).Clear();
            }
            catch (Exception exception)
            {
                ExceptionClass.addTestExceptionToList(new Exception("I couldn't clear that object, possibly because I couldnt find it"));
            }
        }

        public static void Set(object[] oInputParameters)
        {
            string strAttribute = oInputParameters[0].ToString();
            string strValue = oInputParameters[1].ToString();
            string strInput = oInputParameters[2].ToString();
            IWebElement webElement = null;

            try
            {
                webElement = findWeblement(strAttribute, strValue);
            }
            catch (Exception exception)
            {
                //
            }

            if (webElement.TagName.ToUpper() == "INPUT")
            {
                webElement.SendKeys(strInput);
            }
            else if (webElement.TagName.ToUpper() == "SELECT")
            {
                Select(strAttribute, strValue, strInput, webElement);
            }
            else
            {
                ExceptionClass.addTestExceptionToList(new Exception("I don't know how to handle that kind of object. You need to contact the framework owner."));
            }
        }

        private static void Select(string strAttribute, string strValue, string strInput, IWebElement webElement)
        {
            bool blnSelectElementFound = false;

            try
            {
                new SelectElement(webElement).SelectByValue(strInput);
                blnSelectElementFound = true;
            }
            catch (Exception exception)
            {
                //Nothing found
            }

            if (!blnSelectElementFound)
            {
                try
                {
                    new SelectElement(webElement).SelectByText(strInput);
                    blnSelectElementFound = true;
                }
                catch (Exception exception)
                {
                    //Nothing found
                }
            }
            if (!blnSelectElementFound)
            {
                try
                {
                    int index = Int32.Parse(strInput);
                    new SelectElement(webElement).SelectByIndex(index);
                    blnSelectElementFound = true;
                }
                catch (Exception exception)
                {
                    ExceptionClass.addTestExceptionToList(new Exception("I can't select that input from those available"));
                }
            }
        }

        public static IWebElement findWeblement(string strAttribute, string strValue)
        {
            IWebElement webElement = null;
            
            bool blnObjectFound = false;
            if (strAttribute != "" & strValue != "")
            {
                try
                {
                    webElement = FindElementByXPathAttributeAndValue(strAttribute, strValue);
                    if (!Object.Equals(webElement, null))
                    {
                        blnObjectFound = true;
                    }
                }
                catch (Exception exception)
                {
                    //Not found here
                }
            }

            //If that hasn't worked, try all the other options one by one until one succeeds
            if (!blnObjectFound)
            {
                try
                {
                    webElement = FindElementById(strValue);
                    if (!Object.Equals(webElement, null))
                    {
                        blnObjectFound = true;
                    }
                }
                catch (Exception exception)
                {
                    //Not found here
                }
            }

            if (!blnObjectFound)
            {
                try
                {
                    webElement = FindElementByName(strValue);
                    if (!Object.Equals(webElement, null))
                    {
                        blnObjectFound = true;
                    }
                }
                catch (Exception exception)
                {
                    //Not found here
                }
            }

            if (!blnObjectFound)
            {
                try
                {
                    webElement = FindElementByXPath(strValue);
                    if (!Object.Equals(webElement, null))
                    {
                        blnObjectFound = true;
                    }
                }
                catch (Exception exception)
                {
                    ExceptionClass.addTestExceptionToList(new Exception("I couldn't find the object on the page, using Attribute <" + strAttribute
                            + "> Value <" + strValue + ">"));
                }
            }

            return webElement;
        }
        
        public static void ClearElementById(object[] oInputParameters)
        {
            try
            {
                FindElementById(oInputParameters[1].ToString()).Clear();
            }
            catch (Exception exception)
            {
                ExceptionClass.addTestExceptionToList(exception);
            }
        }

        public static void ClearElementByName(object[] oInputParameters)
        {
            try
            {
                FindElementByName(oInputParameters[1].ToString()).Clear();
            }
            catch (Exception exception)
            {
                ExceptionClass.addTestExceptionToList(exception);
            }
        }

        public static void ClearElementByTagName(object[] oInputParameters)
        {
            try
            {
                FindElementByTagName(oInputParameters[0].ToString()).Clear();
            }
            catch (Exception exception)
            {
                ExceptionClass.addTestExceptionToList(exception);
            }
        }

        public static void ClearElementByXPath(object[] oInputParameters)
        {
            try
            {
                FindElementByXPath(oInputParameters[1].ToString()).Clear();
            }
            catch (Exception exception)
            {
                ExceptionClass.addTestExceptionToList(exception);
            }
        }

        public static void ClearElementByXPathAtrributeValue(object[] oInputParameters)
        {
            try
            {
                FindElementByXPathAttributeAndValue(oInputParameters[0].ToString(), oInputParameters[1].ToString()).Clear();
            }
            catch (Exception exception)
            {
                ExceptionClass.addTestExceptionToList(exception);
            }
        }

        private static IWebElement FindElementById(string strInput)
        {
            try
            {
                return driver.FindElement(By.Id(strInput));
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        private static IWebElement FindElementByTagName(string strInput)
        {
            try
            {
                return driver.FindElement(By.TagName(strInput));
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        private static IWebElement FindElementByXPath(string strInput)
        {
            try
            {
                return driver.FindElement(By.XPath(strInput));
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        private static IWebElement FindElementByName(string strInput)
        {
            try
            {
                return driver.FindElement(By.Name((strInput)));
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        private static IWebElement FindElementByXPathAttributeAndValue(string strAttribute, string strValue)
        {
            try
            {
                return driver.FindElement(By.XPath(("//*[@" + strAttribute + "=\"" + strValue + "\"]")));
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        private static void CopyParametersArrayToLocal(object[] oInputParameters)
        {
            int i = 0;
            foreach (object obj in oInputParameters)
            {
                oInputParameters[i] = obj;
                i++;
            }
        }

        private static void quit(object[] oInputParameters)
        {
            
        }
    }
}
