using AventStack.ExtentReports;
using JM;
using JM.MainUtils;
using JM.Test.Pages;
using JMAutomation;
using JMAutomation.MainUtils;
using JMAutomation.Test.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMAutomation.Test.Steps
{
    public class SettingPageSteps
    {
        public static void verifySettingPage(string IdNumber, bool uncheckemail, bool unchecksms, bool unchecktel) {
            SettingPage SettingPage = new SettingPage();
            DashboardPageSteps.isdashboardPageDispalyed();
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibility(SettingPage.profileicon);
            BaseStep.click(SettingPage.ProfileIcon);
            BaseStep.wait.waitForElementVisibility(SettingPage.settingbtn);
            BaseStep.click(SettingPage.SettingBtn);

            BaseStep.wait.waitForElementExistsLongWait(SettingPage.emailtext,10);
            uncheckEmail(IdNumber, uncheckemail);
            uncheckSMS(IdNumber, unchecksms);
            uncheckTel(IdNumber, unchecktel);

        }

        private static void uncheckEmail(string IdNumber, bool uncheckEmail) {
            SettingPage SettingPage = new SettingPage();

            if (uncheckEmail)
            {
                bool first = SettingPage.EmailCheckBox.Selected;
                if (SettingPage.EmailCheckBox.Selected)
                {
                    BaseStep.wait.waitTillPageLoad();
                    BaseStep.click(SettingPage.EmailText);
                    Report.childlog.Log(Status.Info, "Email checkBox is Deselected");
                    Validate.takestepFullScreenShot("Email checkBox is Deselected", Status.Info);
                    BaseStep.wait.genericWait(2000);
                    BaseStep.wait.waitTillPageLoad();
                    Assert.IsTrue(!SettingPage.EmailCheckBox.Selected);
                    BaseStep.wait.waitForElementCliackableLongWait(SettingPage.emailtext,60);
                    BaseStep.wait.genericWait(2000);
                    bool Email = DBCreditCoach.verifiedEmail_SMS_Tel(IdNumber,"Email");
                    Assert.IsTrue(!Email);
                }
                else
                {
                    Validate.takestepFullScreenShot("Email checkBox is Deselected", Status.Info);
                    BaseStep.wait.genericWait(2000);
                    Assert.IsTrue(!SettingPage.EmailCheckBox.Selected);
                    BaseStep.wait.waitForElementCliackableLongWait(SettingPage.emailtext, 60);
                    BaseStep.wait.genericWait(2000);
                    bool Email = DBCreditCoach.verifiedEmail_SMS_Tel(IdNumber, "Email");
                    Assert.IsTrue(!Email);
                }
            } 

        }
        private static void uncheckSMS(string IdNumber, bool uncheckSMS)
        {
            SettingPage SettingPage = new SettingPage();

            if (uncheckSMS)
            {
                bool first = SettingPage.SmsCheckBox.Selected;
                if (SettingPage.SmsCheckBox.Selected)
                {
                    BaseStep.click(SettingPage.SmsText);
                    Report.childlog.Log(Status.Info, "SMS checkBox is Deselected");
                    Validate.takestepFullScreenShot("SMS checkBox is Deselected", Status.Info);
                    BaseStep.wait.genericWait(2000);
                    Assert.IsTrue(!SettingPage.SmsCheckBox.Selected);
                    BaseStep.wait.waitForElementCliackableLongWait(SettingPage.smstext, 60);
                    BaseStep.wait.genericWait(2000);
                    bool Sms = DBCreditCoach.verifiedEmail_SMS_Tel(IdNumber, "SMS");
                    Assert.IsTrue(!Sms);
                }
                else
                {
                    Validate.takestepFullScreenShot("Sms checkBox is Deselected", Status.Info);
                    BaseStep.wait.genericWait(2000);
                    Assert.IsTrue(!SettingPage.SmsCheckBox.Selected);
                    BaseStep.wait.waitForElementCliackableLongWait(SettingPage.smstext, 60);
                    BaseStep.wait.genericWait(2000);
                    bool Sms = DBCreditCoach.verifiedEmail_SMS_Tel(IdNumber, "SMS");
                    Assert.IsTrue(!Sms);
                }
            }

        }
        private static void uncheckTel(string IdNumber, bool uncheckTel)
        {
            SettingPage SettingPage = new SettingPage();

            if (uncheckTel)
            {
                bool first = SettingPage.TelCheckBox.Selected;
                if (SettingPage.TelCheckBox.Selected)
                {
                    BaseStep.click(SettingPage.TelText);
                    Report.childlog.Log(Status.Info, "Tel checkBox is Deselected");
                    Validate.takestepFullScreenShot("Tel checkBox is Deselected", Status.Info);
                    BaseStep.wait.genericWait(2000);
                    Assert.IsTrue(!SettingPage.TelCheckBox.Selected);
                    BaseStep.wait.waitForElementCliackableLongWait(SettingPage.teltext, 60);
                    BaseStep.wait.genericWait(2000);
                    bool Tel = DBCreditCoach.verifiedEmail_SMS_Tel(IdNumber, "Tel");
                    Assert.IsTrue(!Tel);
                }
                else
                {
                    Validate.takestepFullScreenShot("Tel checkBox is Deselected", Status.Info);
                    BaseStep.wait.genericWait(2000);
                    Assert.IsTrue(!SettingPage.TelCheckBox.Selected);
                    BaseStep.wait.waitForElementCliackableLongWait(SettingPage.teltext, 60);
                    BaseStep.wait.genericWait(2000);
                    bool Tel = DBCreditCoach.verifiedEmail_SMS_Tel(IdNumber, "Tel");
                    Assert.IsTrue(!Tel);
                }
            }

        }

    }
}
