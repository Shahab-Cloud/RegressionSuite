using JM.MainUtils;
using JMAutomation.MainUtils;
using JMAutomation.Test.Steps;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace JMAutomation.Test.TestsJM
{
    [TestFixture, Category("ValidateCreditReportPage"), Category("AllRegressionTCs"),Category("Non_Registration_TCs")]
    public class ValidateCreditReportPage
    {
        public static string classname = "ValidateCreditReportPage";
        private static IEnumerable<object[]> custDetails => GenericUtils.ReadExcelDataAsPerColumnsInput(classname, 2);

        [OneTimeSetUp]
        public void ReportSetup()
        {

            Report.extentReport = Report.ExtentInitialize(classname);
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            WebdriverSession.Driver = new ChromeDriver();
            WebdriverSession.Driver.Manage().Window.Maximize(); 

        }


        [Test, TestCaseSource("custDetails")]
        public static void CreditReportPage(string IdNumber, string Password)
        {

            /**************************************************************
             * 
             * Test:- Check For Can You Afford  Your Debt Field Behaviour
             * 
             * ************************************************************/

            Report.log = Report.ExtentTest(classname + " for ID - " + IdNumber);
            Report.childlog = Report.ExtentTestGroup("Login with ID");
            DBCreditCoach.deleteCreditHistory(IdNumber);
            loginPageSteps.loginT0App();
            loginPageSteps.loginWithID(IdNumber, Password);
            Report.printAndClearStep(Report.childlog);


            Report.childlog = Report.ExtentTestGroup("Verify Credit Report");
            CreditReportPageSteps.verifyCreditReport(IdNumber);
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
