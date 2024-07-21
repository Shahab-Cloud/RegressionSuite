using AventStack.ExtentReports;
using JM.MainUtils;
using JM.Test.Pages;
using JMAutomation.MainUtils;

namespace JMAutomation
{
    public class RegistrationPageSteps
    {


        public static void enterCustDetails(string firstName, string surname, string pnumber)
        {


            RegistrationPage RegistrationPage = new RegistrationPage();

            BaseStep.wait.waitForElementVisibilityLongWait(RegistrationPage.firstname, 60);

            Assert.IsTrue(RegistrationPage.isPageDisplayed());

            BaseStep.sendkeys(RegistrationPage.FirstNumber, firstName);
            Report.childlog.Log(Status.Info, "FirstNumber Entered By User is :- " + firstName);
            BaseStep.sendkeys(RegistrationPage.SurName, surname);
            Report.childlog.Log(Status.Info, "SurName Entered By User is :- " + surname);
            BaseStep.sendkeys(RegistrationPage.CellPhoneNumber, "0" + pnumber);
            Report.childlog.Log(Status.Info, "CellPhoneNumber Entered By User is :- " + "0" + pnumber);
            string emailAddress = "Test123" + GenericUtils.getRandomString(4) + "@test.com";
            BaseStep.sendkeys(RegistrationPage.EmailAddress, emailAddress);
            Report.childlog.Log(Status.Info, "EmailAddress Entered By User is :- " + emailAddress);
            BaseStep.sendkeys(RegistrationPage.Password, properties.password);
            Report.childlog.Log(Status.Info, "Password Entered By User is :- " + properties.password);
            BaseStep.sendkeys(RegistrationPage.ConfirmPassword, properties.password);
            Report.childlog.Log(Status.Info, "ConfirmPassword Entered By User is :- " + properties.password);
            BaseStep.click(RegistrationPage.TandCSlider);
            BaseStep.wait.genericWait(2000);
            Validate.takestepFullScreenShot("Customer Details", Status.Info);
            BaseStep.click(RegistrationPage.RegisterBtn);
            BaseStep.wait.waitTillPageLoad();


        }

        public static void enterCustDetailsForAutoReg()
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            BaseStep.wait.waitForElementVisibilityLongWait(RegistrationPage.arpassword, 60);

            BaseStep.sendkeys(RegistrationPage.AutoRegPassword, properties.password);
            Report.childlog.Log(Status.Info, "Password Entered By User is :- " + properties.password);
            BaseStep.sendkeys(RegistrationPage.AutoRegConfirmPassword, properties.password);
            Report.childlog.Log(Status.Info, "ConfirmPassword Entered By User is :- " + properties.password);
            BaseStep.click(RegistrationPage.AutoRegTandCSlider);
            BaseStep.wait.genericWait(2000);
            Validate.takestepFullScreenShot("Customer Details", Status.Info);
            BaseStep.click(RegistrationPage.AutoRegRegisterBtn);



        }


        public static async Task enterOTPAsync(String OTPquery, string phonenumber)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(RegistrationPage.otppopuptext, 60);
            BaseStep.wait.genericWait(5000);

            Validate.takestepFullScreenShot("OTP Popup is Visible", Status.Info);
            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.getotppopupbtn, 60);
            BaseStep.click(RegistrationPage.GetOtpPopUpBtn);
            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.enterotp, 60);
            BaseStep.wait.genericWait(2000);

            //  string otp = DBCreditCoachOTP.getOTP(OTPquery);          

            BaseStep.click(RegistrationPage.SubmitOTP);
            BaseStep.wait.waitTillPageLoad();

        }

        public static void handleSecurityQuestions(string query, bool isSecondSetRequired)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            if (isSecondSetRequired == true)
            {
                // first Set
                UserFailedAtSecurityQuestions();

                // second Set
                try
                {
                    BaseStep.wait.waitForElementVisibilityLongWait(RegistrationPage.securityquestion, 10);
                    Validate.takestepFullScreenShot("Second Set of Security Question are visible", Status.Info);
                    DBCreditCoach.getQuestionClick(query);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Validate.takestepFullScreenShot("No 2nd Set of Security Questions for this User is visible", Status.Info);

                }

            }
            else
            {

                try
                {
                    BaseStep.wait.waitForElementVisibilityLongWait(RegistrationPage.securityquestion, 30);
                    Validate.takestepFullScreenShot("Security Question are visible", Status.Info);
                    DBCreditCoach.getQuestionClick(query);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Validate.takestepFullScreenShot("No Security Questions for this User", Status.Info);

                }

            }

        }

        public static void getBasicVerificationFromDB(string idnumber)
        {

            BaseStep.wait.genericWait(2000);
            DBCreditCoach.verifyDBBasicVerification(idnumber);

        }

        public static void getPostValidationAfterReg(string idnumber)
        {

            BaseStep.wait.genericWait(2000);
            Validate.takestepFullScreenShot("Registration Succesful", Status.Info);
            DBCreditCoach.getPostValidationAfterReg(idnumber);

        }
        public static void UserFailedAtSecurityQuestions()
        {
            RegistrationPage RegistrationPage = new RegistrationPage();
            loginPage loginPage = new loginPage();

            try
            {
                BaseStep.wait.genericWait(2000);
                BaseStep.wait.waitForElementVisibilityLongWait(RegistrationPage.securityquestion, 10);
                Validate.takestepFullScreenShot("Security Question are visible", Status.Info);
                bool questionVisible = RegistrationPage.SecurityQuestion.Displayed;
                int i = 1;
                while (questionVisible && i <= 5)
                {

                    BaseStep.click(RegistrationPage.OptionSelect("0"));
                    i++;
                }
                BaseStep.wait.genericWait(2000);
                Validate.takestepFullScreenShot("5 out of 5 Questions are Selected", Status.Info);
                BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.aftersecurityquestionsubmitbtn, 15);
                BaseStep.click(RegistrationPage.AfterSecurityQuestionSubmitBtn);
                BaseStep.wait.waitTillPageLoad();

                bool SecurityQuestionFailedMsgVisible = RegistrationPage.SecurityQuestionFailedMsg.Displayed;
                if (SecurityQuestionFailedMsgVisible)
                {

                    string SecurityQuestionFailedMsg = BaseStep.getText.text(RegistrationPage.SecurityQuestionFailedMsg);
                    Report.childlog.Log(Status.Info, "Error Message is visible with text is :- " + SecurityQuestionFailedMsg);
                }
            }
            catch
            {
                Console.WriteLine("Set 1 Security Question");
            }



        }


      
    }
}
