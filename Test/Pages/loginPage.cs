
using JM.MainUtils;
using OpenQA.Selenium;


namespace JMAutomation
{
    public class loginPage:WebdriverSession
    {
      

        public By idnumber = By.XPath("//*[@id=\"IdNumber\"]");
        public IWebElement IDNumber => Driver.FindElement(idnumber);

        public By registerbtn = By.XPath("//*[@id=\"id-number-form\"]/div[2]/div/button");
        public IWebElement RegisterBtn => Driver.FindElement(registerbtn);

        
        public By loginicononlandingpage = By.XPath("/html/body/app-root/app-register/header/nav/div/div/div/ul/li/a");
        public IWebElement LoginIconOnLandingPage => Driver.FindElement(loginicononlandingpage);

        public By loginicononforgotpass = By.XPath("/html/body/app-root/app-forgot/app-header/header/nav/div/div/div/ul/li[2]/a");
        public IWebElement LoginIconOnForgotPass => Driver.FindElement(loginicononforgotpass);


        public By loginidnumber = By.XPath("//*[@id=\"logonIdentifier\"]");
        public IWebElement LoginIdNumber => Driver.FindElement(loginidnumber);

        public By loginidpassword = By.XPath("//*[@id=\"password\"]");
        public IWebElement LoginIDPassword => Driver.FindElement(loginidpassword);

        public By loginbtn = By.XPath("//*[@id=\"next\"]");
        public IWebElement LoginBtn => Driver.FindElement(loginbtn);


        public By forgotpassword = By.XPath("//*[@id=\"forgotPassword\"]");
        public IWebElement ForgotPassword => Driver.FindElement(forgotpassword);


        public bool isLoginPageDisplayedAfterRegis => LoginIdNumber.Displayed;

        



    }
}
