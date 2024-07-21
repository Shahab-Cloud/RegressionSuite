using JM;
using JM.MainUtils;
using JMAutomation;
using JMAutomation.MainUtils;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace JMAutomation.Test.TestsJM
{
    [TestFixture, Category("DTCC634_yourCreditReport"), Category("AllRegressionTCs"), Category("Non_Registration_TCs")]
    public class DTCC634_yourCreditReport
    {
        public static string classname = "DTCC634_yourCreditReport";
        private static IEnumerable<object[]> custDetails => GenericUtils.ReadExcelDataForDTCC634(classname);

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
        public static void yourCreditReport(string IdNumber, string Password, string CreditScore)
        {

            /**************************************************************
             * 
             * Test:- Check For Credit Score Field Behaviour On DashBoard Page
             * 
             * ************************************************************/

            Report.log = Report.ExtentTest(classname + " for ID - " + IdNumber);

            Report.childlog = Report.ExtentTestGroup("Update Score From DB");
            DBCreditCoach.updateScorefromDB(IdNumber, CreditScore);
            Report.printAndClearStep(Report.childlog);

            Report.childlog = Report.ExtentTestGroup("Login with ID");
            loginPageSteps.loginT0App();
            loginPageSteps.loginWithID(IdNumber, Password);
            Report.printAndClearStep(Report.childlog);

            Report.childlog = Report.ExtentTestGroup("Verify Debt Eligiblity");
            DashboardPageSteps.yourCreditScore(IdNumber, CreditScore);
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
