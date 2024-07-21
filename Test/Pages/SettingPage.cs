using JM.MainUtils;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMAutomation.Test.Pages
{
    public class SettingPage:WebdriverSession
    {
        public By profileicon = By.XPath("//*[@id=\"navbarDropdown\"]/h5");
        public IWebElement ProfileIcon => Driver.FindElement(profileicon);

        public By settingbtn = By.XPath("//*[@id=\"navbarDropdown\"]/following-sibling::ul/li[2]/a");
        public IWebElement SettingBtn => Driver.FindElement(settingbtn);

        public By emailcheckbox = By.XPath("//*[@id=\"allowEmail\"]");
        public IWebElement EmailCheckBox => Driver.FindElement(emailcheckbox);

        public By emailtext = By.XPath("//label[@for='allowEmail']");
        public IWebElement EmailText => Driver.FindElement(emailtext);

        public By smscheckbox = By.XPath("//*[@id=\"allowEmail\"]");
        public IWebElement SmsCheckBox => Driver.FindElement(smscheckbox);

        public By smstext = By.XPath("/html/body/app-root/app-layout/app-setting/section/section/div/div/div[2]/form/div/ul[1]/li/div[2]/div[2]/div/label");
        public IWebElement SmsText => Driver.FindElement(smstext);

        public By telcheckbox = By.XPath("//*[@id=\"allowEmail\"]");
        public IWebElement TelCheckBox => Driver.FindElement(telcheckbox);

        public By teltext = By.XPath("/html/body/app-root/app-layout/app-setting/section/section/div/div/div[2]/form/div/ul[1]/li/div[2]/div[2]/div/label");
        public IWebElement TelText => Driver.FindElement(teltext);


    }
}
