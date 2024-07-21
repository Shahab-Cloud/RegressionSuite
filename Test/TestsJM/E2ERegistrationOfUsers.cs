using JM.MainUtils;
using JMAutomation.MainUtils;
using OpenQA.Selenium.Chrome;
using SanlamAutomation.TestResources;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace JMAutomation.Test.TestsJM
{
    [TestFixture, Category("E2ERegistrationOfUsers"),Category("AllRegressionTCs"),Category("Registration_TCs")]
    public class E2ERegistrationOfUsers
    {

        private static IEnumerable<object[]> custDetails => GenericUtils.ReadExcelDataAsPerColumnsInput("E2ERegistrationOfUsers", 6);



        [OneTimeSetUp]
        public void ReportSetup()
        {
            string getClassName = this.GetType().Name + DateTime.Now.ToString("_MMddyyyy_hhmmtt");
           Report.extentReport = Report.ExtentInitialize(getClassName);

        }
        [SetUp]
        public void InitializeTest() {
            new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
            WebdriverSession.Driver = new ChromeDriver();
            WebdriverSession.Driver.Manage().Window.Maximize();
        }
        
       


        
        [Test, TestCaseSource("custDetails")]
        public static async Task UserRegistrtionAsync(string IdNumber, string fname, string surname, string pnumber, string websource, string isSecondSetRequired) {

            /***
             * 
             * Test:- E2E registration for Valid and Invalid User
             * 
             * **/

            Report.log=Report.ExtentTest("User Registration with ID - "+ IdNumber);    
            Report.childlog= Report.ExtentTestGroup("ID Number");
            loginPageSteps.loginT0App();                 
            loginPageSteps.enterIDNumber(IdNumber);
            Report.printAndClearStep(Report.childlog);


            Report.childlog = Report.ExtentTestGroup("Customer Details");
            RegistrationPageSteps.enterCustDetails(fname, surname, pnumber);
            Report.printAndClearStep(Report.childlog);

            Report.childlog = Report.ExtentTestGroup("OTP PopUp");
            await RegistrationPageSteps.enterOTPAsync(DBQueries.OTPquery, pnumber);
            Report.printAndClearStep(Report.childlog);

            Report.childlog = Report.ExtentTestGroup("Basic Verification");
            RegistrationPageSteps.getBasicVerificationFromDB(IdNumber);
            Report.printAndClearStep(Report.childlog);

            Report.childlog = Report.ExtentTestGroup("Security Questions");
            bool isSecondSetRequire = bool.Parse(isSecondSetRequired);
            RegistrationPageSteps.handleSecurityQuestions(DBQueries.questionQuery(IdNumber), isSecondSetRequire);
            Report.printAndClearStep(Report.childlog);

            Report.childlog = Report.ExtentTestGroup("Login Page");
            loginPageSteps.loginUserAfterValidation(IdNumber, properties.password);
            Report.printAndClearStep(Report.childlog);

            Report.childlog = Report.ExtentTestGroup("DB Post Validation");
            RegistrationPageSteps.getPostValidationAfterReg(IdNumber);
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
