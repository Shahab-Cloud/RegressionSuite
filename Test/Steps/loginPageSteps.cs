using AventStack.ExtentReports;
using JM.MainUtils;
using JMAutomation.MainUtils;
using Newtonsoft.Json.Linq;

namespace JMAutomation
{
    public class loginPageSteps : BaseStep
    {

        public static void loginT0App()
        {
            string path = GenericUtils.getDataPath("TestResources");
            JObject json = GenericUtils.GetJson(path + "\\Url.json");
            string url = json[properties.environment].ToString();
            try
            {
                WebdriverSession.Driver.Navigate().GoToUrl(url);
                Validate.takestepFullScreenShot("Landing Page is Visible", Status.Info);
            }
            catch
            {
                loginPage loginPage = new loginPage();
                BaseStep.wait.waitTillPageLoad();
                BaseStep.wait.waitForElementVisibilityLongWait(loginPage.idnumber, 300);
                Validate.takestepFullScreenShot("Landing Page is Visible", Status.Info);
            }

        }

        public static void loginT0App(string url)
        {

            try {
                WebdriverSession.Driver.Navigate().GoToUrl(url);
                Validate.takestepFullScreenShot("Landing Page is Visible", Status.Info);
            } catch {
                loginPage loginPage = new loginPage();
                BaseStep.wait.waitTillPageLoad();
                BaseStep.wait.waitForElementVisibilityLongWait(loginPage.idnumber, 300);
                Validate.takestepFullScreenShot("Landing Page is Visible", Status.Info);
            }
            

        }

        public  static void enterIDNumber(string IdNumber) {
            loginPage loginPage = new loginPage();            
            BaseStep.wait.waitForElementCliackableLongWait(loginPage.idnumber, 60);
            BaseStep.wait.genericWait(5000);            
            BaseStep.sendkeys(loginPage.IDNumber, IdNumber);
            Report.childlog.Log(Status.Info,"ID Number Entered By User is :- "+ IdNumber);
            Validate.takestepFullScreenShot("IDNumber", Status.Pass);
            BaseStep.click(loginPage.RegisterBtn);
            BaseStep.wait.waitTillPageLoad();

        }

        public static void loginUserAfterValidation(String IDNumber, String Password)
        {
            loginPage loginPage = new loginPage();
            
           BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber,60);            
            BaseStep.wait.genericWait(5000);

            Assert.IsTrue(loginPage.isLoginPageDisplayedAfterRegis);
            Validate.takestepFullScreenShot("Login Page is displayed after registration", Status.Info);
            BaseStep.sendkeys(loginPage.LoginIdNumber, IDNumber);
            BaseStep.sendkeys(loginPage.LoginIDPassword, Password);
            Validate.takestepFullScreenShot("Credentials Entered", Status.Info);
            BaseStep.click(loginPage.LoginBtn);
            BaseStep.wait.waitTillPageLoad();
            
            DashboardPageSteps.waitTillSalaryPopUpIsDisplayed();
            DashboardPageSteps.isdashboardPageDispalyed();
            

        }

        public static void loginWithID(String IDNumber, String Password) {

            loginPage loginPage = new loginPage();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.idnumber, 60);
            BaseStep.wait.genericWait(5000);

            BaseStep.click(loginPage.LoginIconOnLandingPage);
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);
            BaseStep.wait.genericWait(5000);

            Assert.IsTrue(loginPage.isLoginPageDisplayedAfterRegis);
            Validate.takestepFullScreenShot("Login Page is displayed after registration", Status.Info);
            BaseStep.sendkeys(loginPage.LoginIdNumber, IDNumber);
            BaseStep.sendkeys(loginPage.LoginIDPassword, Password);
            Validate.takestepFullScreenShot("Credentials Entered", Status.Info);
            BaseStep.click(loginPage.LoginBtn);

        }



    }
}
