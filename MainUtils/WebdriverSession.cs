using DocumentFormat.OpenXml.Bibliography;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V114.Runtime;
using OpenQA.Selenium.Remote;
using WebDriverManager.DriverConfigs.Impl;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM.MainUtils
{
    public class WebdriverSession
    {
        public static IWebDriver Driver { get; set; }
        public static string folderName { get; set; }
        public static string screenshotfolder { get; set; }
        public static IReadOnlyCollection<string> windowHandles { get; set; }
        public static DateTime currentDateTime { get; set; }

        public static void refreshPage () { 
        Driver.Navigate().Refresh();
            
        }

        
        public static void GetRemoteChromeDriver()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments(
                "start-maximized",
                "enable-automation",
                "--no-sandbox", //this is the relevant other arguments came from solving other issues
                "--disable-infobars",
                "--disable-dev-shm-usage",
                "--disable-browser-side-navigation",
                "--disable-gpu",
                "--ignore-certificate-errors");
            var capability = chromeOptions.ToCapabilities();

            Driver = new RemoteWebDriver(new Uri("http://localhost:59655"), capability, TimeSpan.FromMinutes(3));
            Driver.Manage().Timeouts().PageLoad.Add(TimeSpan.FromSeconds(20));
                    
        }


        public static void RemoteChromeDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.EnableMobileEmulation("iPhone SE");
            options.AddArgument("no-sandbox");

            Driver = new ChromeDriver(ChromeDriverService.CreateDefaultService(), options, TimeSpan.FromMinutes(3));
            Driver.Manage().Timeouts().PageLoad.Add(System.TimeSpan.FromSeconds(30));

        }

    }
}
