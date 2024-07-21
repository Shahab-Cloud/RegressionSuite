using DocumentFormat.OpenXml.Bibliography;
using JM.MainUtils;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM.Test.Pages
{
    public  class ForgotPasswordPage:WebdriverSession
    {

        public By idnumber = By.XPath("//*[@id=\"IdNumber\"]");
        public IWebElement IDNumber => Driver.FindElement(idnumber);

        
        public By searchtext = By.XPath("//*[@id=\"SearchText\"]");
        public IWebElement SearchText => Driver.FindElement(searchtext);

        public By sendotpbtn = By.XPath("//*[text()='Send OTP']");
        public IWebElement SendOtpBtn => Driver.FindElement(sendotpbtn);
        
        public By resendotptimer = By.XPath("//button[@name='btnResend OTPForgotPassword']/span");
        public IWebElement ResendOtpTimer => Driver.FindElement(resendotptimer);

        public By resendotpbtn = By.XPath("//button[@name='btnResend OTPForgotPassword']");
        public IWebElement ResendOtpBtn => Driver.FindElement(resendotpbtn);

        public By enterotp = By.XPath("/html/body/app-root/app-forgot/section/div/div/div/form/div/div[2]/div[1]/div[1]/div/div[1]/div/input");
        public IWebElement EnterOtp => Driver.FindElement(enterotp);

        public By password = By.XPath("//*[@id=\"Password\"]");
        public IWebElement Password => Driver.FindElement(password);

        public By confirmpassword = By.XPath("//*[@id=\"ConfirmPassword\"]");
        public IWebElement ConfirmPassword => Driver.FindElement(confirmpassword);

        public By submitbtn = By.XPath("/html/body/app-root/app-forgot/section/div/div/div/form/div/div[3]/button");
        public IWebElement SubmitBtn => Driver.FindElement(submitbtn);

        public By resendotpmsg = By.XPath("/html/body/app-root/app-forgot/section/div/div/div/form/div/div[3]/button");
        public IWebElement ResendOtpMsg => Driver.FindElement(resendotpmsg);

        public By sessionexpirymsg = By.XPath("/html/body/app-root/app-forgot/section/div/div/div/form/div/div[3]/button");
        public IWebElement SessionExpiryMsg => Driver.FindElement(sessionexpirymsg);

        public By incorrectotpmsg = By.XPath("/html/body/app-root/app-forgot/section/div/div/div/form/div/div[1]/div");
        public IWebElement IncorrectOtpMsg => Driver.FindElement(incorrectotpmsg);

        // resend otp timer after browser close         

         public By sessionexpirytimer = By.XPath("/html/body/app-root/app-forgot/section/div/div/div/form/div/div[1]/div");
        public IWebElement SessionExpiryTimer => Driver.FindElement(sessionexpirytimer);


        public static bool isResendOtpTimerDisplayed() 
        { 
        bool isResendOtpTimerDisplayed = false;
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            try
            {
                isResendOtpTimerDisplayed = ForgotPasswordPage.ResendOtpTimer.Displayed;
            }catch 
            {
                return isResendOtpTimerDisplayed;
            }

            return isResendOtpTimerDisplayed;
        }

        public static bool isSessionExpiryMsgDisplayed()
        {
            bool isSessionExpiryMsgDisplayed = false;
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            try
            {
                isSessionExpiryMsgDisplayed = ForgotPasswordPage.SessionExpiryMsg.Displayed;
            }
            catch
            {
                return isSessionExpiryMsgDisplayed;
            }

            return isSessionExpiryMsgDisplayed;
        }
        public static bool isResendOtpBtnEnabled()
        {
            bool isResendOtpBtnEnabled = false;
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            try
            {
                isResendOtpBtnEnabled = ForgotPasswordPage.ResendOtpBtn.Enabled;
            }
            catch
            {
                return isResendOtpBtnEnabled;
            }

            return isResendOtpBtnEnabled;
        }

    }
}
    