using JM.MainUtils;
using JMAutomation.MainUtils;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace JMAutomation.Test.TestsJM
{
    [TestFixture, Category("DTCC633_UnderstandingMyScore"), Category("AllRegressionTCs"),Category("Non_Registration_TCs")]
    public class DTCC633_UnderstandingMyScore
    {
        private static string classname = "DTCC633_UnderstandingMyScore";
        private static IEnumerable<object[]> custDetails => GenericUtils.ReadExcelDataAsPerColumnsInput(classname,4);

        [OneTimeSetUp]
        public void ReportSetup()
        {

            Report.extentReport = Report.ExtentInitialize(classname);

        }
        [SetUp]
        public void InitializeTest()
        {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            WebdriverSession.Driver = new ChromeDriver();
            WebdriverSession.Driver.Manage().Window.Maximize();

        }

        [Test, TestCaseSource("custDetails")]
        public static void UnderstandingMyScore(string IdNumber, string Password, string Salary, string RepaymentValue)
        {

            /**************************************************************
             * 
             * Test:- Check For Understanding My Score Field Behaviour
             * 
             * ************************************************************/

            Report.log = Report.ExtentTest(classname + "for ID - " + IdNumber);
            Report.childlog = Report.ExtentTestGroup("Login with ID");            
            loginPageSteps.loginT0App();
            loginPageSteps.loginWithID(IdNumber, Password);
            Report.printAndClearStep(Report.childlog);

            Report.childlog = Report.ExtentTestGroup("Understanding My Score");
            DashboardPageSteps.checkNoOfOpenAccounts();
            DashboardPageSteps.yourDebtIsMadeUpOf(IdNumber);
            DashboardPageSteps.howMuchHaveYouPaidOff(IdNumber);

            Report.printAndClearStep(Report.childlog);



        }
        [TearDown]
        public void TearDownTest()
        {
            Validate.GetResult();
            WebdriverSession.Driver.Quit();
            Report.ExtentClose();

        }
    }
}
