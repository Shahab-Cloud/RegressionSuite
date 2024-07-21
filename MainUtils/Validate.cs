using AventStack.ExtentReports;
using JM.MainUtils;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Chrome;
using RazorEngine.Compilation.ImpromptuInterface.Optimization;
using OpenQA.Selenium.Support.Extensions;

namespace JMAutomation.MainUtils
{
    public class Validate : WebdriverSession
    {
        public static void takeFullPageScreenShot(string message, Status status)
        {
            string screenShotPath = screenshot(message);
            Report.childlog.AddScreenCaptureFromPath(screenShotPath);
            IJavaScriptExecutor js = (IJavaScriptExecutor)Driver;
            js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            BaseStep.wait.genericWait(1000);
            Report.childlog.Log(Status.Pass, message + ":-" + Report.childlog.AddScreenCaptureFromPath(screenShotPath));

        }
        public static void takestepFullScreenShot(string message, Status status)
        {

            switch (status)
            {
                case Status.Pass:
                    sendPassFull(message);
                    break;
                case Status.Fail:
                    sendFailFull(message);
                    break;
                default:
                    sendStatusFull(message);
                    break;

            }

        }

        public static string screenshot(string message)
        {
            Screenshot ss = ((ITakesScreenshot)Driver).GetScreenshot();
            string screenshot = ((ITakesScreenshot)Driver).GetScreenshot().AsBase64EncodedString;
            Directory.CreateDirectory(screenshotfolder);
            string filepath = screenshotfolder + "\\" + message + DateTime.Now.ToString("HH-mm-ss") + ".png";
            string localpath = new Uri(filepath).LocalPath;
            ss.SaveAsFile(localpath, ScreenshotImageFormat.Png);
            Report.childlog.Log(Status.Info, message, MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot).Build());

            return localpath;
        }
        public static string screenshotFailed(string message)
        {
            Screenshot ss = ((ITakesScreenshot)Driver).GetScreenshot();
            string screenshot = ((ITakesScreenshot)Driver).GetScreenshot().AsBase64EncodedString;
            string screenshotfolder = Path.Combine(folderName, "FailedScreenshot");
            Directory.CreateDirectory(screenshotfolder);
            string filename = $"{message} {DateTime.Now.ToString("HH-mm-ss")}.png";
            string filepath = Path.Combine(screenshotfolder, filename);
            ss.SaveAsFile(filepath, ScreenshotImageFormat.Png);
            Report.childlog.Log(Status.Info, message, MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot).Build());
            return filepath;
        }
        public static void sendPassFull(string message)
        {
            string screenShotPath = screenshot(message);
            Report.childlog.AddScreenCaptureFromPath(screenShotPath);

        }
        public static void sendFailFull(string message)
        {
            string screenShotPath = screenshot(message);
            Report.childlog.Log(Status.Fail, message + ":-" + Report.childlog.AddScreenCaptureFromPath(screenShotPath));
        }

        public static void sendStatusFull(string message)
        {
            string screenShotPath = screenshot(message);
            Report.childlog.AddScreenCaptureFromPath(screenShotPath);

        }



        public static void assertEquals(object expected, object actual, string message, bool surpassScreenshot)
        {

            string screenShotPath;
            if (!surpassScreenshot)
            {
                string  ssText = expected.ToString();
                screenShotPath = screenshot("ScreenShot with text _ "+ssText);
                Report.childlog.Log(Status.Info, "ScreenShot:-" + Report.childlog.AddScreenCaptureFromPath(screenShotPath));
            }
            if (!(expected.Equals(actual)))
            {
                Report.childlog.Log(Status.Fail, message);
            }
            Assert.AreEqual(expected, actual);


        }

        public static void assertNotEquals(object expected, object actual, string message, bool surpassScreenshot)
        {

            string screenShotPath;
            if (!surpassScreenshot)
            {
                screenShotPath = screenshot(message);
                Report.childlog.Log(Status.Info, message + ":-" + Report.childlog.AddScreenCaptureFromPath(screenShotPath));
            }
            if (!(expected == actual))
            {
                Report.childlog.Log(Status.Fail, message);
            }
            Assert.AreNotEqual(expected, actual);

        }

        public static void GetResult()
        {

            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = "<pre>" + TestContext.CurrentContext.Result.StackTrace + "</pre>";
            var errorMsg = TestContext.CurrentContext.Result.Message;



            if (status == TestStatus.Failed)
            {
                string screenShotPath = screenshotFailed("Failed Step Screenshot");
                Report.childlog.Log(Status.Fail, stackTrace + errorMsg);
                Report.childlog.Log(Status.Fail, "Snapshot below :- " + Report.childlog.AddScreenCaptureFromPath(screenShotPath));

            }
            else
            {
                Report.childlog.Log(Status.Pass, stackTrace + errorMsg);

            }
        }

        public static void devLogs()
        {
            var logs = Driver.Manage().Logs.GetLog(LogType.Browser);
            foreach (var log in logs)
            {
                Report.childlog.Log(Status.Info, $"[{log.Timestamp}] {log.Level}: {log.Message}");
            }
        }

    }
}
