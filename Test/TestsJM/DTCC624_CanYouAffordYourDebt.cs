using JM.MainUtils;
using JM.Test.Pages;
using JMAutomation;
using JMAutomation.MainUtils;
using OpenQA.Selenium.Chrome;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace JMAutomation.Test.TestsJM
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class DTCC624_CanYouAffordYourDebt
    {
        public static string classname = "DTCC624_CanYouAffordYourDebt";
        private static IEnumerable<object[]> custDetails => GenericUtils.ReadExcelDataAsPerColumnsInput(classname,3);

        [OneTimeSetUp]
        public void ReportSetup()
        {
            
            Report.extentReport = Report.ExtentInitialize(classname);
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            WebdriverSession.Driver = new ChromeDriver();
            WebdriverSession.Driver.Manage().Window.Maximize();
        }
    

        [Test, TestCaseSource("custDetails")]
        public static void CanYouAffordYourDebt(string IdNumber, string Password, string Salary)
        {

            /**************************************************************
             * 
             * Test:- Check For Can You Afford  Your Debt Field Behaviour
             * 
             * ************************************************************/

            Report.log = Report.ExtentTest(classname + " for ID - " + IdNumber);
            DashboardPage HomePage = new DashboardPage();            
            bool DashboardPage = HomePage.isHomePageDisplayed();
            if (!DashboardPage)
            {
                Report.childlog = Report.ExtentTestGroup("Login with ID");
                loginPageSteps.loginT0App();
                loginPageSteps.loginWithID(IdNumber, Password);
                Report.printAndClearStep(Report.childlog);
            }

            Report.childlog = Report.ExtentTestGroup("Verify Debt Eligiblity");            
            DashboardPageSteps.enterSalaryAndRepaymentValueAfterLogin(IdNumber, Salary);            
            DashboardPageSteps.checkDebt(Salary);
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
