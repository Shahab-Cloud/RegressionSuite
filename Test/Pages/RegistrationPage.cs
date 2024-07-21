
using JM.MainUtils;
using OpenQA.Selenium;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM.Test.Pages
{
    public class RegistrationPage : WebdriverSession
    {
        

        public By firstname = By.XPath("//*[@id=\"FirstName\"]");
        public IWebElement FirstNumber => Driver.FindElement(firstname);

        public By surname = By.XPath("//*[@id=\"Surname\"]");
        public IWebElement SurName => Driver.FindElement(surname);

        public By cellphonenumber = By.XPath("//*[@id=\"PhoneNumber\"]");
        public IWebElement CellPhoneNumber => Driver.FindElement(cellphonenumber);

        public By emailaddress = By.XPath("//*[@id=\"Email\"]");
        public IWebElement EmailAddress => Driver.FindElement(emailaddress);

        public By password = By.XPath("//*[@id=\"Email\"]/parent::div/parent::div/following-sibling::div/div/div/input");
        public IWebElement Password => Driver.FindElement(password);


        public By confirmpassword = By.XPath("//*[@id=\"Email\"]/parent::div/parent::div/following-sibling::div/div[2]/div/input");
        public IWebElement ConfirmPassword => Driver.FindElement(confirmpassword);

        public By tAndcslider = By.XPath("//*[@id=\"registerForm\"]/div[4]/div[1]/label/span");
        public IWebElement TandCSlider => Driver.FindElement(tAndcslider);

        public By registerbtn = By.XPath("//*[@id=\"registerForm\"]/div[6]/div/button");
        public IWebElement RegisterBtn => Driver.FindElement(registerbtn);

        public By securityquestion = By.XPath("/html/body/app-root/app-security-question/section/div[1]/div/div/div[2]/h4");
        public IWebElement SecurityQuestion => Driver.FindElement(securityquestion);

        
        public By aftersecurityquestionsubmitbtn = By.XPath("/html/body/app-root/app-security-question/section/div[3]/div/div/button");
        public IWebElement AfterSecurityQuestionSubmitBtn => Driver.FindElement(aftersecurityquestionsubmitbtn);

        // Auto Reg

        public By arpassword = By.XPath("//*[@id='Password']");
        public IWebElement AutoRegPassword => Driver.FindElement(arpassword);


        public By arconfirmpassword = By.XPath("//*[@id='ConfirmPassword']");
        public IWebElement AutoRegConfirmPassword => Driver.FindElement(arconfirmpassword);

        public By artAndcslider = By.XPath("//*[@id=\"auto-register-form\"]/div[2]/div[1]/label/span");
        public IWebElement AutoRegTandCSlider => Driver.FindElement(artAndcslider);

        public By arregisterbtn = By.XPath("//*[@id=\"auto-register-form\"]/div[4]/div/button");
        public IWebElement AutoRegRegisterBtn => Driver.FindElement(arregisterbtn);

        //OTP popup

        public By otppopuptext = By.XPath("//*[text()=' Get your One-Time Pin (OTP) via SMS ']");
        public IWebElement OtpPopUpText => Driver.FindElement(otppopuptext);

        public By getotppopupbtn = By.XPath("//button[text()='Send SMS OTP']");
        public IWebElement GetOtpPopUpBtn => Driver.FindElement(getotppopupbtn);


        public By enterotp = By.XPath("//*[contains(text(),'Please Enter OTP')]/following-sibling::input");
        public IWebElement EnterOTP => Driver.FindElement(enterotp);

        public By submitotp = By.XPath("/html/body/ngb-modal-window/div/div/form/div[2]/div[2]/button");
        public IWebElement SubmitOTP => Driver.FindElement(submitotp);        

        public By resendotpbtn = By.XPath("/html/body/app-root/app-forgot/section/div/div/div/form/div/div[2]/div[1]/div[2]/button");
        public IWebElement ResendOtpBtn => Driver.FindElement(resendotpbtn);

        public By resendotpmsg = By.XPath("/html/body/app-root/app-forgot/section/div/div/div/form/div/div[3]/button");
        public IWebElement ResendOtpMsg => Driver.FindElement(resendotpmsg);


        //otps
        public By resendotptimer = By.XPath("//button[@name='btnResend SMS OTPOTPRegisterPage']/span");
        public IWebElement ResendOtpTimer => Driver.FindElement(resendotptimer);
        
        public By resendotpsbtn = By.XPath("//button[@name='btnResend SMS OTPOTPRegisterPage']");
        public IWebElement ResendOtpsBtn => Driver.FindElement(resendotpsbtn);

        public By resendotpsmsg = By.XPath("//div[@class='alert alert-success']");
        public IWebElement ResendOtpsMsg => Driver.FindElement(resendotpsmsg);

        public By sessionexpirymsg = By.XPath("/html/body/ngb-modal-window/div/div/form/div[1]/div[1]");
        public IWebElement SessionExpiryMsg => Driver.FindElement(sessionexpirymsg);

        public By incorrectotpmsg = By.XPath("//div[@class='alert alert-danger']");
        public IWebElement IncorrectOtpMsg => Driver.FindElement(incorrectotpmsg);

        public By otppopupcutbtn = By.XPath("/html/body/ngb-modal-window/div/div/div/button");
        public IWebElement OtpPopupCutBtn => Driver.FindElement(otppopupcutbtn);

        // resend otp timer after browser close         

        public By sessionexpirytimer = By.XPath("/html/body/ngb-modal-window/div/div/form/div[1]/div[1]");
        public IWebElement SessionExpiryTimer => Driver.FindElement(sessionexpirytimer);



        //securityquestionfailedmsg

        public By securityquestionfailedmsg = By.XPath("/html/body/app-root/app-security-question/section/div[2]/div[1]/div");
        public IWebElement SecurityQuestionFailedMsg => Driver.FindElement(securityquestionfailedmsg);


        public IWebElement OptionSelect(String optionID) {

         BaseStep.wait.waitForElementVisibility(securityquestion);
         return   WebdriverSession.Driver.FindElement(By.XPath("//*[@id='option-"+optionID+"']"));

        }

        




        public bool isPageDisplayed()
        {
            return FirstNumber.Displayed;
        }

        public bool isSecurityQuestionDisplayed()
        {
            return SecurityQuestion.Displayed;
        }

        public String SecurityQuestionTest()
        {
            return SecurityQuestion.Text;
        }
    }
}
