using JM.MainUtils;
using JMAutomation.MainUtils;
using JMAutomation.Test.Steps;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace JMAutomation.Test.TestsJM
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class DTCC_ValidateSettingPageFeild
    {
        public static string classname = "DTCC_ValidateSettingPageFeild";
        private static IEnumerable<object[]> custDetails => GenericUtils.ReadExcelDataAsPerColumnsInput(classname, 2);

        [OneTimeSetUp]
        public void ReportSetup()
        {

            Report.extentReport = Report.ExtentInitialize(classname);
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            WebdriverSession.Driver = new ChromeDriver(chromeOptions);
            WebdriverSession.Driver.Manage().Window.Maximize();

        }


        [Test, TestCaseSource("custDetails")]
        public static void ValidateSettingPageFeild(string IdNumber, string Password)
        {

            /**************************************************************
             * 
             * Test:- Check For ValidateSettingPageFeild
             * 
             * ************************************************************/

            Report.log = Report.ExtentTest(classname + " for ID - " + IdNumber);
            Report.childlog = Report.ExtentTestGroup("Login with ID");            
            loginPageSteps.loginT0App();
            loginPageSteps.loginWithID(IdNumber, Password);
            Report.printAndClearStep(Report.childlog);


            Report.childlog = Report.ExtentTestGroup("Verify User Settings");           
            SettingPageSteps.verifySettingPage(IdNumber,true,false,false);
            Report.printAndClearStep(Report.childlog);



        }
        [TearDown]
        public void TearDown()
        {
            Validate.GetResult();


        }
        [OneTimeTearDown]
        public void TearDownTest()
        {

            WebdriverSession.Driver.Quit();
            Report.ExtentClose();

        }
    }
}
