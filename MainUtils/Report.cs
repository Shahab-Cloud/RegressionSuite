using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using JMAutomation;

namespace JM.MainUtils
{
    public class Report : WebdriverSession
    {

        public static ExtentTest log { get; set; }
        public static ExtentTest childlog { get; set; }
        public static ExtentReports extentReport { get; set; }

        private static ExtentReports extent;
        public static ExtentReports ExtentInitialize(String classname)
        {
            // This is used for pipelines
            /*    // Construct the relative path for the folder
                string relativeFolderPath = Path.Combine("ExtentReport", classname);
                 folderName = Path.Combine(@"D:\a\1\s\", relativeFolderPath);
    */
            if (properties.IsReportRequiredInProject)
            {
                string relativeFolderPath = Path.Combine("ExtentReport", classname);
                folderName = Path.Combine(@"C:\Users\mohds\source\repos\JMRegressionSuite\JMAutomation", relativeFolderPath);
                // Create the directory if it doesn't exist
                Directory.CreateDirectory(folderName);

            }
            else
            {
                folderName = properties.folderName + classname + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss").Trim();
                Directory.CreateDirectory(folderName);

            }
            string reportFilePath = folderName + @"\\ExtentReport.html";
            var htmlreporter = new ExtentHtmlReporter(reportFilePath);
            /* htmlreporter.Config.Theme = Theme.Dark;*/
            htmlreporter.Config.DocumentTitle = classname;

            extent = new ExtentReports();
            extent.AttachReporter(htmlreporter);
            screenshotfolder = folderName + "\\PassedScreenshots";
            return extent;

        }
        public static void ExtentClose()
        {
            extent.Flush();
        }

        public static ExtentTest ExtentTest(String testName)
        {

            return extentReport.CreateTest(testName);
        }

        public static ExtentTest ExtentTestGroup(String nodename)
        {
            return Report.log.CreateNode(nodename);
        }

        public static void printAndClearStep(ExtentTest test)
        {
            Report.log.RemoveNode(test);
        }

    }
}

