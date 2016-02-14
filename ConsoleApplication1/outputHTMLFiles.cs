using System;
using System.IO;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SeleniumFrameworkTry1
{
    public class outputHTMLFiles
    {
        protected static StreamWriter sw;

        protected static void deleteLastXLinesInFile(string FilePath, int LinesToDelete)
        {
            string[] lines = System.IO.File.ReadAllLines(FilePath);
            System.IO.File.WriteAllLines(FilePath, lines.Take(lines.Length - LinesToDelete).ToArray());
        }
    }

    public class HTMLFrameworkLog : outputHTMLFiles
    {
        private static string FilePath;
        
        public static void ClearDownFrameworkLog()
        {
            if(File.Exists(FilePath))
            {
                File.Delete(FilePath);
                WriteFrameworkHeader();
            }
        }

        static HTMLFrameworkLog()
        {
            FilePath = FrameworkParametersClass.FrameworkLogPath;
            if (!File.Exists(FilePath))
            {
                WriteFrameworkHeader();
            }
        }

        private static void WriteFrameworkHeader()
        {
            sw = new StreamWriter(FilePath);
            sw.WriteLine("<!DOCTYPE html>");
            sw.WriteLine("<HTML>");
            sw.WriteLine("  <HEAD>");
            sw.WriteLine("    <title>Framework Log</title>");
            sw.WriteLine("    <style>");
            sw.WriteLine("        body{ font-family:\"Lucida Console\",Monaco,monospace; font - size:13px} table,td,th{border: 1px solid #000}table{border-collapse:collapse;width:100%;table-layout:fixed;word-wrap:break-word}caption{display:table-caption;text-align:center;font-size:18px}td{text-align:left;vertical-align:top;padding:2px}td.PropertyName{background-color:#000;color:#E3AE57;width:30%}#Class,td.Begin{background-color:#1FDA9A;font-weight:700}td.LogTableHeaders{font-weight:700;font-size:14px}td.Begin{color:#000;width:100%;table-layout:fixed;word-wrap:break-word;font-size:14px;text-align:center;vertical-align:middle}#DateTime,#Method,#PropertyValue{background-color:#000}#DateTime,#Method,#Properties{color:#1FDA9A;table-layout:fixed;word-wrap:break-word}#DateTime{width:8%}#Class{color:#000;width:14%;table-layout:fixed;word-wrap:break-word}#Method{width:21%}#Properties{background-color:#fff;width:57%;vertical-align:middle}#PropertyName{background-color:#E3AE57;color:#000;width:25%;table-layout:fixed;word-wrap:break-word}#PropertyValue{color:#E3AE57;width:75%;table-layout:fixed;word-wrap:break-word}td.ExceptionHeader,td.NoTestsReadyHeader{color:#000;font-size:16px;text-align:center;width:100%}td.ExceptionHeader{background-color:#DC3D24}td.NoTestsReadyHeader{background-color:#1FDA9A}td.ExceptionDetails,td.ExceptionType,tr.ExceptionDetails{background-color:#000;color:#DC3D24;font-size:12px;table-layout:fixed;word-wrap:break-word}tr.ExceptionDetails{width:50%}td.ExceptionType{width:30%}td.ExceptionDetails{width:70%}");
            sw.WriteLine("    </style>");
            sw.WriteLine("  </HEAD>");
            sw.WriteLine("  <BODY>");
            sw.WriteLine("    <TABLE>");
            sw.WriteLine("      <CAPTION><H1>LOG</H1></CAPTION>");
            sw.WriteLine("      <TR>");
            sw.WriteLine("        <TD class='LogTableHeaders' id='DateTime'><H2>Date Time</H2></TD>");
            sw.WriteLine("        <TD class='LogTableHeaders' id='Class'><H2>Class Name</H2></TD>");
            sw.WriteLine("        <TD class='LogTableHeaders' id='Method'><H2>Method</H2></TD>");
            sw.WriteLine("        <TD class='LogTableHeaders' id='Properties'>");
            sw.WriteLine("          <TABLE class='PropertiesTable'>");
            sw.WriteLine("            <TR>");
            sw.WriteLine("              <TD class='LogTableHeaders' id='PropertyName'><H2>Property Name</H2></TD>");
            sw.WriteLine("              <TD class='LogTableHeaders' id='PropertyValue'><H2>Property Value</H2></TD>");
            sw.WriteLine("            </TR>");
            sw.WriteLine("          </TABLE>");
            sw.WriteLine("        </TD>");
            sw.WriteLine("      </TR>");
            sw.WriteLine("    </TABLE>");
            sw.WriteLine("  </BODY>");
            sw.WriteLine("</HTML>");
            sw.Dispose();
        }

        public static void appendBeginMessage()
        {
            deleteLastXLinesInFile(FilePath, 2);

            sw = new StreamWriter(FilePath, true);
            sw.WriteLine("    <TABLE>");
            sw.WriteLine("      <TR>");
            sw.WriteLine("        <TD class='Begin'>Begin test</TD>");
            sw.WriteLine("      </TR>");
            sw.WriteLine("    </TABLE>");
            sw.WriteLine("  </BODY>");
            sw.WriteLine("</HTML>");
            sw.Dispose();
        }

        public static void appendEndMessage()
        {
            deleteLastXLinesInFile(FilePath, 2);

            sw = new StreamWriter(FilePath, true);
            sw.WriteLine("    <TABLE>");
            sw.WriteLine("      <TR>");
            sw.WriteLine("        <TD class='Begin'>End test</TD>");
            sw.WriteLine("      </TR>");
            sw.WriteLine("    </TABLE>");
            sw.WriteLine("  </BODY>");
            sw.WriteLine("</HTML>");
            sw.Dispose();
        }

        public static void appendEvent(string ClassName, string Method, Dictionary<string, string> Properties)
        {
            deleteLastXLinesInFile(FilePath, 2);

            sw = new StreamWriter(FilePath, true);
            sw.WriteLine("    <TABLE>");
            sw.WriteLine("      <TR>");
            sw.WriteLine("        <TD class='LogTable' id='DateTime'>" + DateTime.Now.ToString() + "</TD>");
            sw.WriteLine("        <TD class='LogTable' id='Class'>" + ClassName + "</TD>");
            sw.WriteLine("        <TD class='LogTable' id='Method'>" + Method + "</TD>");
            sw.WriteLine("        <TD class='LogTable' id='Properties'>");
            sw.WriteLine("          <TABLE class='PropertiesTable'>");

            foreach (KeyValuePair<string, string> entry in Properties)
            {
                string Name = entry.Key;
                string Value = entry.Value;
                sw.WriteLine("            <TR>");
                sw.WriteLine("              <TD class='LogTable' id='PropertyName'>" + Name + "</TD>");
                sw.WriteLine("              <TD class='LogTable' id='PropertyValue'>" + Value + "</TD>");
                sw.WriteLine("            </TR>");
            }

            sw.WriteLine("          </TABLE>");
            sw.WriteLine("        </TD>");
            sw.WriteLine("      </TR>");
            sw.WriteLine("    </TABLE>");
            sw.WriteLine("  </BODY>");
            sw.WriteLine("</HTML>");
            sw.Dispose();
        }

        public static void appendException(string ExceptionType, string ExceptionMessage)
        {
            deleteLastXLinesInFile(FilePath, 2);

            sw = new StreamWriter(FilePath, true);
            sw.WriteLine("    <TABLE>");
            sw.WriteLine("      <TR>");
            sw.WriteLine("        <TD class='ExceptionHeader'>EXCEPTION</TD>");
            sw.WriteLine("      </TR>");
            sw.WriteLine("    </TABLE>");
            sw.WriteLine("    <TABLE>");
            sw.WriteLine("      <TR class='ExceptionDetails'>");
            sw.WriteLine("        <TD class='ExceptionType'>Exception Type</TD>");
            sw.WriteLine("        <TD class='ExceptionMessage'>Exception Message</TD>");
            sw.WriteLine("      </TR>");
            sw.WriteLine("      <TR class='ExceptionDetails'>");
            sw.WriteLine("        <TD class='ExceptionType'>" + ExceptionType + "</TD>");
            sw.WriteLine("        <TD class='ExceptionMessage'>" + ExceptionMessage + "</TD>");
            sw.WriteLine("      </TR>");
            sw.WriteLine("      <TR class='ExceptionHeader'></TR>");
            sw.WriteLine("    </TABLE>");
            sw.WriteLine("  </BODY>");
            sw.WriteLine("</HTML>");
            sw.Dispose();
        }

        public static void appendTerminateMessage()
        {
            deleteLastXLinesInFile(FilePath, 2);

            sw = new StreamWriter(FilePath, true);
            sw.WriteLine("    <TABLE>");
            sw.WriteLine("      <TR>");
            sw.WriteLine("        <TD class='ExceptionHeader'>TERMiNATE</TD>");
            sw.WriteLine("      </TR>");
            sw.WriteLine("    </TABLE>");
            sw.WriteLine("  </BODY>");
            sw.WriteLine("</HTML>");
            sw.Dispose();
        }

        public static void appendNoTestForExecutionMessage()
        {
            deleteLastXLinesInFile(FilePath, 2);

            sw = new StreamWriter(FilePath, true);
            sw.WriteLine("    <TABLE>");
            sw.WriteLine("      <TR>");
            sw.WriteLine("        <TD class='NoTestsReadyHeader'>No test For Execution</TD>");
            sw.WriteLine("      </TR>");
            sw.WriteLine("    </TABLE>");
            sw.WriteLine("  </BODY>");
            sw.WriteLine("</HTML>");
            sw.Dispose();
        }
    }

    public class TestReportHTML : outputHTMLFiles
    {
        private static string FilePath;

        public TestReportHTML()
        {
            FilePath = TestParametersClass.TestBeingRunReportFile;
            if (!File.Exists(FilePath))
            {
                sw = new StreamWriter(FilePath);
                sw.WriteLine("<!DOCTYPE html>");
                sw.WriteLine("<HTML>");
                sw.WriteLine("  <HEAD>");
                sw.WriteLine("    <title>Test Report for " + TestParametersClass.TestName + " at " + TestParametersClass.strDateTime + "</title>");
                sw.WriteLine("    <script src='https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js'></script>");
                sw.WriteLine("    <script src='http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js'></script>");
                sw.WriteLine("    <link rel='stylesheet' href='http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css'>");
                sw.WriteLine("    <link rel='stylesheet' type='text/css' href='../../TestReport.css'>");
                sw.WriteLine("  </HEAD>");
                sw.WriteLine("  <BODY class='Passed'>");
                sw.WriteLine("    <div>");
                sw.WriteLine("      <H1>Test Report for " + TestParametersClass.TestName + " at " + TestParametersClass.strDateTime + "</H1>");
                sw.WriteLine("    </div>");
                sw.WriteLine("    <div class='Summary'>");
                sw.WriteLine("      <H2>Test Summary</H2>");
                sw.WriteLine("      <div class='TestResultsContainer'>");
                sw.WriteLine("        <p>Test status not available</p>");
                sw.WriteLine("        <table class='TestReportTable'>");
                sw.WriteLine("          <tr>");
                sw.WriteLine("            <td class='TestReportTableHeader'>Steps passed</td>");
                sw.WriteLine("            <td class='TestReportTableHeader'>Steps failed</td>");
                sw.WriteLine("            <td class='TestReportTableHeader'>Steps blocked</td>");
                sw.WriteLine("            <td class='TestReportTableHeader'>Time started</td>");
                sw.WriteLine("            <td class='TestReportTableHeader'>Time ended</td>");
                sw.WriteLine("            <td class='TestReportTableHeader'>Test duration</td>");
                sw.WriteLine("            <td class='TestReportTableHeader'>Browser</td>");
                sw.WriteLine("          </tr>");
                sw.WriteLine("          <tr>");
                sw.WriteLine("            <td class='Passed'>Steps passed not available</td>");
                sw.WriteLine("            <td class='Failed'>Steps failed not available</td>");
                sw.WriteLine("            <td class='Blocked'>Steps blocked not available</td>");
                sw.WriteLine("            <td class='center'>" + TestParametersClass.strStartTime + "</td>");
                sw.WriteLine("            <td class='center'>Time ended not available</td>");
                sw.WriteLine("            <td class='center'>Test duration not available</td>");
                sw.WriteLine("            <td class='center'>" + TestParametersClass.Browser + "</td>");
                sw.WriteLine("        </table>");
                sw.WriteLine("      </div>");
                sw.WriteLine("    </div>");
                sw.WriteLine("    <div class='StepResults'>");
                sw.WriteLine("      <H2>Test Details</H2>");
                sw.WriteLine("    </div>");
                sw.WriteLine("  </BODY>");
                sw.WriteLine("</HTML>");
                sw.Dispose();
            }
        }

        public static void updateSummaryDetails()
        {
            sw = new StreamWriter("temp.html");
            sw.WriteLine("<!DOCTYPE html>");
            sw.WriteLine("<HTML>");
            sw.WriteLine("  <HEAD>");
            sw.WriteLine("    <title>Test Report for " + TestParametersClass.TestName + " at " + TestParametersClass.strDateTime + "</title>");
            sw.WriteLine("    <script src='https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js'></script>");
            sw.WriteLine("    <script src='http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/js/bootstrap.min.js'></script>");
            sw.WriteLine("    <link rel='stylesheet' href='http://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css'>");
            sw.WriteLine("    <link rel='stylesheet' type='text/css' href='../../TestReport.css'>");
            sw.WriteLine("  </HEAD>");
            sw.WriteLine("  <BODY class='" + TestParametersClass.TestStatus + "'>");
            sw.WriteLine("    <div>");
            sw.WriteLine("      <H1>Test Report for " + TestParametersClass.TestName + " at " + TestParametersClass.strDateTime + "</H1>");
            sw.WriteLine("    </div>");
            sw.WriteLine("    <div class='Summary'>");
            sw.WriteLine("      <H2>Test Summary</H2>");
            sw.WriteLine("      <div class='TestResultsContainer'>");
            sw.WriteLine("        <p class='" + TestParametersClass.TestStatus + "' id='TestResult'>Test " + TestParametersClass.TestStatus + "</p>");
            sw.WriteLine("        <table class='TestReportTable'>");
            sw.WriteLine("          <tr>");
            sw.WriteLine("            <td class='TestReportTableHeader'>Steps passed</td>");
            sw.WriteLine("            <td class='TestReportTableHeader'>Steps failed</td>");
            sw.WriteLine("            <td class='TestReportTableHeader'>Steps blocked</td>");
            sw.WriteLine("            <td class='TestReportTableHeader'>Time started</td>");
            sw.WriteLine("            <td class='TestReportTableHeader'>Time ended</td>");
            sw.WriteLine("            <td class='TestReportTableHeader'>Test duration</td>");
            sw.WriteLine("            <td class='TestReportTableHeader'>Browser</td>");
            sw.WriteLine("          </tr>");
            sw.WriteLine("          <tr>");
            sw.WriteLine("            <td class='Passed'>" + TestParametersClass.StepsPassed + "</td>");
            sw.WriteLine("            <td class='Failed'>" + TestParametersClass.StepsFailed  + "</td>");
            sw.WriteLine("            <td class='Blocked'>" + TestParametersClass.StepsBlocked + "</td>");
            sw.WriteLine("            <td class='center'>" + TestParametersClass.strStartTime + "</td>");
            sw.WriteLine("            <td class='center'>" + TestParametersClass.strEndTime + "</td>");
            sw.WriteLine("            <td class='center'>" + TestParametersClass.Browser + "</td>");
            sw.WriteLine("            <td class='center'>" + DateTime.Parse(TestParametersClass.strEndTime)
                .Subtract(DateTime.Parse(TestParametersClass.strStartTime)).ToString() + "</td>");
            sw.WriteLine("        </table>");
            sw.WriteLine("      </div>");
            sw.WriteLine("    </div>");
            sw.WriteLine("    <div class='StepResults'>");
            sw.WriteLine("      <H2>Test Details</H2>");
            
            sw.Dispose();

            string[] summary = System.IO.File.ReadAllLines("temp.html");
            string[] lines = System.IO.File.ReadAllLines(FilePath);

            int i = 0;
            foreach (string str in summary)
            {
                lines[i] = summary[i];
                i++;
            }
            System.IO.File.WriteAllLines(FilePath, lines.ToArray());
            File.Delete("temp.html");
        }

        public static void appendEvent(string ClassName, string Method, Dictionary<string, string> Properties)
        {
            deleteLastXLinesInFile(FilePath, 3);

            sw = new StreamWriter(FilePath, true);
            sw.WriteLine("      <div class='panel-group'>");
            sw.WriteLine("        <div class='panel panel-default " + TestParametersClass.StepStatus + "'>");
            sw.WriteLine("          <div class='panel-heading " + TestParametersClass.StepStatus + "'>");
            sw.WriteLine("            <h3 class='panel-title'>");
            sw.WriteLine("              <a data-toggle='collapse' id='Toggle' href='#collapse" + Properties["Step ID"] + "'>Step ID: " + Properties["Step ID"] + "</a>");
            sw.WriteLine("              <p class='" + TestParametersClass.StepStatus + "' id='StepResult'>Step " + TestParametersClass.StepStatus + "</p>");
            sw.WriteLine("            </h3>");
            sw.WriteLine("          </div>");
            sw.WriteLine("          <div id='collapse" + Properties["Step ID"] + "' class='panel-collapse collapse in'>");
            sw.WriteLine("            <div class='panel-body'>");
            sw.WriteLine("              <div class='table'>");
            sw.WriteLine("                <div class='PropertiesTableCell'>");
            sw.WriteLine("                  <TABLE class='PropertiesTable'>");

            foreach (KeyValuePair<string, string> entry in Properties.Skip(2))
            {
                string Name = entry.Key;
                string Value = entry.Value;

                if (Value != "")
                {
                    sw.WriteLine("                    <TR>");
                    sw.WriteLine("                      <TD class='PropertyName" + Properties["Step Status"] + "'>" + Name + "</TD>");
                    sw.WriteLine("                      <TD class='PropertyValue" + Properties["Step Status"] + "'>" + Value + "</TD>");
                    sw.WriteLine("                    </TR>");
                }
            }

            sw.WriteLine("                  </TABLE>");
            sw.WriteLine("                </div>");
            sw.WriteLine("                <div class='ScreenshotCell'>");

            if (TestParametersClass.BrowserOpen)
            {
                sw.WriteLine("                  <a href='" + TestParametersClass.LocalScreenshotFilePathForResultFile + "'>");
                sw.WriteLine("                    <img class='ScreenshotMini' src='" + TestParametersClass.LocalScreenshotFilePathForResultFile + "'>");
                sw.WriteLine("                  </a>");
            }

            sw.WriteLine("                </div>");
            sw.WriteLine("              </div>");
            sw.WriteLine("            </div>");
            sw.WriteLine("          </div>");
            sw.WriteLine("        </div>");
            sw.WriteLine("      </div>");
            sw.WriteLine("    </div>");
            sw.WriteLine("  </BODY>");
            sw.WriteLine("</HTML>");
            sw.Dispose();
        }

        public static void appendException(string ExceptionType, string ExceptionMessage)
        {
            deleteLastXLinesInFile(FilePath, 2);

            sw = new StreamWriter(FilePath, true);
            sw.WriteLine("    <TABLE>");
            sw.WriteLine("      <TR>");
            sw.WriteLine("        <TD class='ExceptionHeader'>EXCEPTION</TD>");
            sw.WriteLine("      </TR>");
            sw.WriteLine("    </TABLE>");
            sw.WriteLine("    <TABLE>");
            sw.WriteLine("      <TR class='ExceptionDetails'>");
            sw.WriteLine("        <TD class='ExceptionType'>Error Type</TD>");
            sw.WriteLine("        <TD class='ExceptionMessage'>Error Message</TD>");
            sw.WriteLine("      </TR>");
            sw.WriteLine("      <TR class='ExceptionDetails'>");
            sw.WriteLine("        <TD class='ExceptionType'>" + ExceptionType + "</TD>");
            sw.WriteLine("        <TD class='ExceptionMessage'>" + ExceptionMessage + "</TD>");
            sw.WriteLine("      </TR>");
            sw.WriteLine("    </TABLE>");
            sw.WriteLine("    <TABLE>");
            sw.WriteLine("      <TR>");
            sw.WriteLine("        <TD class='ExceptionHeader'></TD>");
            sw.WriteLine("      </TR>");
            sw.WriteLine("    </TABLE>");
            sw.WriteLine("  </BODY>");
            sw.WriteLine("</HTML>");
            sw.Dispose();
        }

        public static void appendTerminateMessage()
        {
            deleteLastXLinesInFile(FilePath, 2);

            sw = new StreamWriter(FilePath, true);
            sw.WriteLine("    <TABLE>");
            sw.WriteLine("      <TR>");
            sw.WriteLine("        <TD class='ExceptionHeader'>Catastrophic Failure</TD>");
            sw.WriteLine("      </TR>");
            sw.WriteLine("    </TABLE>");
            sw.WriteLine("  </BODY>");
            sw.WriteLine("</HTML>");
            sw.Dispose();
        }

        public static void appendScripts()
        {
            deleteLastXLinesInFile(FilePath, 2);

            sw = new StreamWriter(FilePath, true);
            sw.WriteLine("  <script>");
            sw.WriteLine("  ");
            sw.WriteLine("  ");
            sw.WriteLine("  ");
            sw.WriteLine("  ");
            sw.WriteLine("  ");
            sw.WriteLine("  ");

            sw.WriteLine("  </BODY>");
            sw.WriteLine("</HTML>");
            sw.Dispose();
        }
    }
}
