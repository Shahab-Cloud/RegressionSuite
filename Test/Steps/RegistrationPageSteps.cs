using AventStack.ExtentReports;
using JM.MainUtils;
using JM.Test.Pages;
using JMAutomation.MainResources;
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
            OTPStorageAccount.deleteOTPTableAsync("0" + phonenumber);

            Validate.takestepFullScreenShot("OTP Popup is Visible", Status.Info);
            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.getotppopupbtn, 60);
            BaseStep.click(RegistrationPage.GetOtpPopUpBtn);
            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.enterotp, 60);
            BaseStep.wait.genericWait(2000);

            //  string otp = DBCreditCoachOTP.getOTP(OTPquery);           

           var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync("0" + phonenumber);          
            var pins = sortedEntities.Select(entity => entity.Pin).ToArray();

            string otp = pins[1].ToString();

            BaseStep.sendkeys(RegistrationPage.EnterOTP, otp);
            Validate.takestepFullScreenShot("OTP Entered Successfully", Status.Info);
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

        public static void sendOTP(string phonenumber)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();
            BaseStep.wait.waitTillPageLoad();
            OTPStorageAccount.deleteOTPTableAsync("0"+phonenumber);
            BaseStep.wait.waitForElementVisibilityLongWait(RegistrationPage.otppopuptext, 60);
            BaseStep.wait.genericWait(5000);

            Validate.takestepFullScreenShot("OTP Popup is Visible", Status.Info);
            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.getotppopupbtn, 60);
            BaseStep.click(RegistrationPage.GetOtpPopUpBtn);
            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.enterotp, 60);

        }


        public static async Task verifytwoOTPgetsgeneratedintheOTPtable(string phoneNumber)
        {
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);

            int hitCount = 2;

            var hitCounts = sortedEntities.Select(entity => entity.HitCount).ToArray();
            Assert.IsTrue(hitCount == hitCounts.Length);

        }
        public static async Task verify1stOtpisActive(string phoneNumber)
        {
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var hitCounts = sortedEntities.Select(entity => entity.HitCount).ToArray();
            Assert.IsTrue(hitCounts[1] == 1);
            Assert.IsTrue(hitCounts[0] == 0);
            Report.childlog.Log(Status.Info, "HitCount for 1st otp is " + hitCounts[1]);
            Report.childlog.Log(Status.Info, "HitCount for 2nd otp is " + hitCounts[0]);
        }

        public static async Task countdowntimerisgettingdisplayed()
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(RegistrationPage.resendotptimer, 20);
            Validate.takestepFullScreenShot("Countdown timer appeared", Status.Info);
            BaseStep.wait.waitForElementInvisibilityLongWait(RegistrationPage.resendotptimer, 30);

            bool ResendOtpTimer = RegistrationPage.ResendOtpTimer.Displayed;
            if (!ResendOtpTimer)
            {
                Console.WriteLine("Countdown timer appeared and disappeared within 30 seconds.");
                Report.childlog.Log(Status.Info, "Countdown timer appeared and disappeared within 30 seconds.");
            }
            else
            {
                Console.WriteLine("Countdown timer did not disappear within 30 seconds.");
                Report.childlog.Log(Status.Info, "Countdown timer did not disappear within 30 seconds.");
            }
        }
        public static async Task clickResendOTPthentheResendOTPbuttonGetsDisable(string phonenumber)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            BaseStep.click(RegistrationPage.ResendOtpsBtn);
            BaseStep.wait.waitTillPageLoad();
            int hitcount = await OTPStorageAccount.FetchthedatafromthetableAsyncofIntType(phonenumber, 2, "hitcount");
            Assert.AreEqual(1, hitcount);
            Assert.IsTrue(!RegistrationPage.ResendOtpsBtn.Displayed);
            Report.childlog.Log(Status.Info, "Resend OTP button gets disable after clicking on it");

            string ResendOtpMsg = BaseStep.getText.text(RegistrationPage.ResendOtpsMsg);
            Report.childlog.Log(Status.Info, "Otp is resend succesfully and message visible is " + ResendOtpMsg);

        }

        public static async Task incorrectOtpIsNotAcceptedWithin5MinandAttemptcountIncreaseAsync(string phoneNumber, string password)
        {

            RegistrationPage RegistrationPage = new RegistrationPage();
            loginPage loginPage = new loginPage();

            
            //Check and verify if incorrect OTP is entered within first 5 minutes then attempt count
            //gets incremented for the 1st OTP and it should not get accepted

           
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var pins = sortedEntities.Select(entity => entity.Pin).ToArray();


            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.enterotp, 60);
            BaseStep.sendkeys(RegistrationPage.EnterOTP, pins[0].ToString());
            Report.childlog.Log(Status.Info, "Incorrect otp is entered 1st time is " + pins[0]);
           
            

            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(RegistrationPage.SubmitOTP);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibility(RegistrationPage.incorrectotpmsg);
            Assert.IsTrue(RegistrationPage.IncorrectOtpMsg.Displayed);


            var sortedEntities2 = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var AttemptCounts = sortedEntities2.Select(entity => entity.AttemptCount).ToArray();

            Validate.assertEquals(1, AttemptCounts[0], "AttemptCount is not increased", false);
            Report.childlog.Log(Status.Info, "AttemptCount for 1st otp is " + AttemptCounts[0] + "and incorect otp is not accepted");
        }

        public static async Task verify1stActiveOtpisAccepted(string phoneNumber, string password)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();
            loginPage loginPage = new loginPage();

            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var hitCounts = sortedEntities.Select(entity => entity.HitCount).ToArray();
            Assert.IsTrue(hitCounts[0] == 1);
            Report.childlog.Log(Status.Info, "HitCount for 1st otp is " + hitCounts[0]);

            var pins = sortedEntities.Select(entity => entity.Pin).ToArray();

            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.enterotp, 60);
            BaseStep.clearAndSendkeys(RegistrationPage.EnterOTP, pins[0].ToString());
            Report.childlog.Log(Status.Info, "1st otp is " + pins[0]);
         

            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(RegistrationPage.SubmitOTP);

            BaseStep.wait.waitTillPageLoad();

            var sortedEntities2 = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var AttemptCounts = sortedEntities2.Select(entity => entity.AttemptCount).ToArray();
            Validate.assertEquals(1, AttemptCounts[0], "AttemptCount is not increased", false);
            Report.childlog.Log(Status.Info, "AttemptCount for 1st otp is " + AttemptCounts[0] + "and otp is accepted");

            var SessionCloseReasons = sortedEntities2.Select(entity => entity.SessionCloseReason).ToArray();
            Validate.assertEquals("SuccessClosure", SessionCloseReasons[0], "SuccessClosure is not mention for 1st otp", true);
            Validate.assertEquals("SuccessClosure", SessionCloseReasons[1], "SuccessClosure is not mention for 2nd otp", true);
            Report.childlog.Log(Status.Info, "log of SuccessClosure for both OTP is visible");
        }

        public static async Task incorrectOtpIsNotAcceptedandAttemptcountIncreaseAsync(string phoneNumber, string password)
        {

            RegistrationPage RegistrationPage = new RegistrationPage();          

           

            //Check and verify if incorrect OTP is entered after first 5 minutes then attempt count gets recorded
            //for the 2nd OTP and it should not get accepted

            OTPStorageAccount.updateOtpExpiryTimeAsync(phoneNumber, 0);
            BaseStep.wait.genericWait(60000);
            Report.childlog.Log(Status.Info, "After 5 mins, Again enter incorrect Otp and it will increase second Otp attempt count");
            string randomOtp = GenericUtils.randomInteger(100000, 999999).ToString();

            BaseStep.clearAndSendkeys(RegistrationPage.EnterOTP, randomOtp);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp);
            BaseStep.wait.genericWait(1000);

            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(RegistrationPage.SubmitOTP);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibility(RegistrationPage.incorrectotpmsg);
            Assert.IsTrue(RegistrationPage.IncorrectOtpMsg.Displayed);

            BaseStep.wait.genericWait(2000);
            var sortedEntities3 = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var AttemptCounts2 = sortedEntities3.Select(entity => entity.AttemptCount).ToArray();
            var pins2 = sortedEntities3.Select(entity => entity.Pin).ToArray();

            Validate.assertEquals(1, AttemptCounts2[0], "AttemptCount is not increased", false);
            Report.childlog.Log(Status.Info, "AttemptCount for Random otp is increased after 5 mins is - " + AttemptCounts2[0] + "and incorect otp is not accepted");


            //Check and verify if 2nd correct OTP is entered after first 5 minutes then attempt count gets recorded
            //for the 2nd OTP and it should not get accepted

            BaseStep.clearAndSendkeys(RegistrationPage.EnterOTP, pins2[0].ToString());
            BaseStep.wait.genericWait(1000);
            Report.childlog.Log(Status.Info, " otp entered is " + pins2[0]);
            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(RegistrationPage.SubmitOTP);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibility(RegistrationPage.incorrectotpmsg);
            Assert.IsTrue(RegistrationPage.IncorrectOtpMsg.Displayed);

            BaseStep.wait.genericWait(2000);
            var sortedEntities4 = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var AttemptCounts3 = sortedEntities4.Select(entity => entity.AttemptCount).ToArray();
            Validate.assertEquals(2, AttemptCounts3[0], "AttemptCount is not increased", false);
            Report.childlog.Log(Status.Info, "AttemptCount for 2nd otp is increased after 5 mins is - " + AttemptCounts3[0] + "and otp is not accepted");

            

            /*Check and verify if the 1st OTP is entered after
            5 minutes then if its getting accepted or not*/

            BaseStep.clearAndSendkeys(RegistrationPage.EnterOTP, pins2[1].ToString());
            BaseStep.wait.genericWait(1000);
            Report.childlog.Log(Status.Info, " otp entered is " + pins2[1]);
            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(RegistrationPage.SubmitOTP);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibility(RegistrationPage.incorrectotpmsg);
            Assert.IsTrue(RegistrationPage.IncorrectOtpMsg.Displayed);

            BaseStep.wait.genericWait(2000);
            var sortedEntities5 = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var AttemptCounts4 = sortedEntities5.Select(entity => entity.AttemptCount).ToArray();
            Validate.assertEquals(3, AttemptCounts4[0], "AttemptCount is not increased", false);
            Report.childlog.Log(Status.Info, "AttemptCount for 2nd otp is increased after 5 mins when enter 1st otp is - " + AttemptCounts4[0] + "and otp is not accepted");
            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);

           

        }

        public static async Task secondOTPgetAcceptedAfter5minsResendBtnClickAsync(string phonenumber, string password)
        {
           
            BaseStep.wait.waitTillPageLoad();
            OtpEnterAfterClickingonResendOtpAsync(phonenumber, password, 0);

            BaseStep.wait.genericWait(2000);
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var SessionCloseReasons = sortedEntities.Select(entity => entity.SessionCloseReason).ToArray();
            Validate.assertEquals("SuccessClosure", SessionCloseReasons[0], "SuccessClosure is not mention for 1st otp", true);
            Validate.assertEquals("SuccessClosure", SessionCloseReasons[1], "SuccessClosure is not mention for 2nd otp", true);
            Report.childlog.Log(Status.Info, "log of SuccessClosure for both OTP is visible");
        }
        public static async Task OtpEnterAfterClickingonResendOtpAsync(string phonenumber, string password, int otpIndex)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            BaseStep.wait.genericWait(2000);
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var pins = sortedEntities.Select(entity => entity.Pin).ToArray();

            BaseStep.wait.waitTillPageLoad();
            BaseStep.clearAndSendkeys(RegistrationPage.EnterOTP, pins[otpIndex].ToString());
            BaseStep.wait.genericWait(1000);
            Report.childlog.Log(Status.Info, "otp enter  is " + pins[otpIndex]);
            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(RegistrationPage.SubmitOTP);
            BaseStep.wait.waitTillPageLoad();

        }

        public static async Task FirstOTPnotAcceptedAfter5minsResendBtnClickAsync(string phonenumber, string password)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            BaseStep.wait.waitTillPageLoad();

            BaseStep.click(RegistrationPage.ResendOtpsBtn);
            BaseStep.wait.waitTillPageLoad();

            OtpEnterAfterClickingonResendOtpAsync(phonenumber, password, 1);

            BaseStep.wait.genericWait(2000);
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var AttemptCounts4 = sortedEntities.Select(entity => entity.AttemptCount).ToArray();
            Validate.assertEquals(4, AttemptCounts4[0], "AttemptCount is not increased", false);
            Report.childlog.Log(Status.Info, "AttemptCount for 2nd otp is increased after 5 mins when enter 1st otp is - " + AttemptCounts4[0] + "and otp is not accepted");
        }

        public static async Task secondOTPgetAcceptedWithin5minssync(string phoneNumber, string password)
        {

            RegistrationPage RegistrationPage = new RegistrationPage();
         
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.genericWait(2000);
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var pins = sortedEntities.Select(entity => entity.Pin).ToArray();


            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.enterotp, 60);
            BaseStep.sendkeys(RegistrationPage.EnterOTP, pins[0].ToString());
            Report.childlog.Log(Status.Info, "2nd otp is entered 1st time is " + pins[0]);         

            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(RegistrationPage.SubmitOTP);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibility(RegistrationPage.incorrectotpmsg);
            Assert.IsTrue(RegistrationPage.IncorrectOtpMsg.Displayed);

            BaseStep.wait.genericWait(2000);
            var sortedEntities2 = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var AttemptCounts = sortedEntities2.Select(entity => entity.AttemptCount).ToArray();

            Validate.assertEquals(1, AttemptCounts[0], "AttemptCount is not increased", false);
            Report.childlog.Log(Status.Info, "AttemptCount for 2nd otp is " + AttemptCounts[0] );
        }
        public static async Task secondOtpEnterAfterClickingonResendOtpAsync(string phonenumber, string password)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            BaseStep.wait.waitForElementInvisibilityLongWait(RegistrationPage.resendotptimer, 30);
            BaseStep.click(RegistrationPage.ResendOtpsBtn);
            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.waitForElementVisibility(RegistrationPage.resendotpsmsg);
            Assert.IsTrue(RegistrationPage.ResendOtpsMsg.Displayed);

            BaseStep.wait.genericWait(2000);
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var pins = sortedEntities.Select(entity => entity.Pin).ToArray();

            BaseStep.clearAndSendkeys(RegistrationPage.EnterOTP, pins[0].ToString());
            Report.childlog.Log(Status.Info, "2nd otp is " + pins[0]);
            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(RegistrationPage.SubmitOTP);
            BaseStep.wait.waitTillPageLoad();
           
            var sortedEntities2 = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var SessionCloseReasons = sortedEntities2.Select(entity => entity.SessionCloseReason).ToArray();
            Validate.assertEquals("SuccessClosure", SessionCloseReasons[0], "SuccessClosure is not mention for 1st otp", true);
            Validate.assertEquals("SuccessClosure", SessionCloseReasons[1], "SuccessClosure is not mention for 2nd otp", true);
            Report.childlog.Log(Status.Info, "log of SuccessClosure for both OTP is visible");
        }

        public static async Task sessionExpireafter10minsifOtpNotEnteredEvenAfterClickingOnResendButton(string phonenumber)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            BaseStep.wait.genericWait(300000);
            Report.childlog.Log(Status.Info, "After 5 mins, Now Resend OTP button gets disable after clicking on it. Still no Otp is entered");
            Validate.takestepFullScreenShot("No otp is enetered", Status.Info);
            clickResendOTPthentheResendOTPbuttonGetsDisable(phonenumber);
            BaseStep.wait.genericWait(300000);
            string SessionExpiryMsg = BaseStep.getText.text(RegistrationPage.SessionExpiryMsg);
            Assert.IsTrue(RegistrationPage.SessionExpiryMsg.Displayed);
            Report.childlog.Log(Status.Info, "After 10 mins, Session got expired and error message visible is " + SessionExpiryMsg);


            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var SessionCloseReasons = sortedEntities.Select(entity => entity.SessionCloseReason).ToArray();
            Validate.assertEquals("TimeoutClosure", SessionCloseReasons[0], "TimeoutClosure is not mention for 1st otp", true);
            Validate.assertEquals("TimeoutClosure", SessionCloseReasons[1], "TimeoutClosure is not mention for 2nd otp", true);
            Report.childlog.Log(Status.Info, "log of TimeoutClosure for both OTP is visible");
        }

        public static async Task incorrectOTPisentered6timeswithinfirst5minutes(string phonenumber, string password, int index)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            BaseStep.wait.waitTillPageLoad();

            string randomOtp1 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.enterotp, 60);
            BaseStep.sendkeys(RegistrationPage.EnterOTP, randomOtp1);
            Report.childlog.Log(Status.Info, "1st otp is " + randomOtp1);
            
            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);            
            BaseStep.click(RegistrationPage.SubmitOTP);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.genericWait(2000);
            Assert.IsTrue(RegistrationPage.IncorrectOtpMsg.Displayed);
            BaseStep.wait.waitForElementInvisibilityLongWait(RegistrationPage.incorrectotpmsg, 10);



            string randomOtp2 = GenericUtils.randomInteger(100000, 999999).ToString();            
            BaseStep.clearAndSendkeys(RegistrationPage.EnterOTP, randomOtp2);           
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp2);
            Validate.takestepFullScreenShot("otp", Status.Info);            
            BaseStep.click(RegistrationPage.SubmitOTP);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.genericWait(2000);
            Assert.IsTrue(RegistrationPage.IncorrectOtpMsg.Displayed);
            BaseStep.wait.waitForElementInvisibilityLongWait(RegistrationPage.incorrectotpmsg, 10);

            string randomOtp3 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.wait.genericWait(2000);
            BaseStep.clearAndSendkeys(RegistrationPage.EnterOTP, randomOtp3);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp3);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.wait.genericWait(1000);
            BaseStep.click(RegistrationPage.SubmitOTP);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.genericWait(2000);
            Assert.IsTrue(RegistrationPage.IncorrectOtpMsg.Displayed);
            BaseStep.wait.waitForElementInvisibilityLongWait(RegistrationPage.incorrectotpmsg, 10);

            string randomOtp4 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.wait.genericWait(1000);
            BaseStep.clearAndSendkeys(RegistrationPage.EnterOTP, randomOtp4);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp4);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.wait.genericWait(1000);
            BaseStep.click(RegistrationPage.SubmitOTP);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.genericWait(2000);
            Assert.IsTrue(RegistrationPage.IncorrectOtpMsg.Displayed);
            BaseStep.wait.waitForElementInvisibilityLongWait(RegistrationPage.incorrectotpmsg, 10);

            string randomOtp5 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.wait.genericWait(1000);
            BaseStep.clearAndSendkeys(RegistrationPage.EnterOTP, randomOtp5);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp5);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.wait.genericWait(1000);
            BaseStep.click(RegistrationPage.SubmitOTP);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.genericWait(2000);
            Assert.IsTrue(RegistrationPage.IncorrectOtpMsg.Displayed);
            BaseStep.wait.waitForElementInvisibilityLongWait(RegistrationPage.incorrectotpmsg, 10);

            string randomOtp6 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.wait.genericWait(1000);
            BaseStep.clearAndSendkeys(RegistrationPage.EnterOTP, randomOtp6);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp6);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.wait.genericWait(1000);
            BaseStep.click(RegistrationPage.SubmitOTP);
            BaseStep.wait.waitTillPageLoad();
            Validate.takestepFullScreenShot("OTP Session Expired", Status.Info);

            BaseStep.wait.genericWait(3000);
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var AttemptCounts = sortedEntities.Select(entity => entity.AttemptCount).ToArray();
            Validate.assertEquals(6, AttemptCounts[index], "AttemptCount is not increased", true);
            Report.childlog.Log(Status.Info, "AttemptCount for otp increased  is - " + AttemptCounts[index]);


            var SessionCloseReasons = sortedEntities.Select(entity => entity.SessionCloseReason).ToArray();
            Validate.assertEquals("FailClosure", SessionCloseReasons[0], "FailureClosure is not mention for 1st otp", true);
            Validate.assertEquals("FailClosure", SessionCloseReasons[1], "FailureClosure is not mention for 2nd otp", true);
            Report.childlog.Log(Status.Info, "log of FailureClosure for both OTP is visible");

        }

        public static async Task incorrectOTPisentered6timesAfter5minutes(string phonenumber, string password)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            BaseStep.wait.waitTillPageLoad();
            OTPStorageAccount.updateOtpExpiryTimeAsync(phonenumber, 0);
            BaseStep.wait.genericWait(60000);

            incorrectOTPisentered6timeswithinfirst5minutes(phonenumber, password, 1);
            Report.childlog.Log(Status.Info, "incorrect OTP is entered 6 times After 5 minutes");

        }

        public static async Task incorrectOTPisentered3timesBeforeAndAfter5minutes(string phonenumber, string password)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            
            BaseStep.wait.waitTillPageLoad();
            incorrectOTPisentered3times(phonenumber, password, 0);

            OTPStorageAccount.updateOtpExpiryTimeAsync(phonenumber, 0);
            BaseStep.wait.genericWait(60000);

            incorrectOTPisentered3times(phonenumber, password, 1);

            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var SessionCloseReasons = sortedEntities.Select(entity => entity.SessionCloseReason).ToArray();
            Validate.assertEquals("FailClosure", SessionCloseReasons[0], "FailureClosure is not mention for 1st otp", true);
            Validate.assertEquals("FailClosure", SessionCloseReasons[1], "FailureClosure is not mention for 2nd otp", true);
            Report.childlog.Log(Status.Info, "log of FailureClosure for both OTP is visible");
            Report.childlog.Log(Status.Info, "incorrect OTP is entered 6 times After 5 minutes");

        }

        public static async Task incorrectOTPisentered3times(string phonenumber, string password, int index)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            BaseStep.wait.waitTillPageLoad();

            string randomOtp1 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.enterotp, 60);
            BaseStep.sendkeys(RegistrationPage.EnterOTP, randomOtp1);
            Report.childlog.Log(Status.Info, "1st otp is " + randomOtp1);
            BaseStep.sendkeys(RegistrationPage.Password, password);
            BaseStep.sendkeys(RegistrationPage.ConfirmPassword, password);
            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(RegistrationPage.SubmitOTP);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(RegistrationPage.IncorrectOtpMsg.Displayed);
            BaseStep.wait.waitForElementInvisibilityLongWait(RegistrationPage.incorrectotpmsg, 10);

            string randomOtp2 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(RegistrationPage.EnterOTP, randomOtp2);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp2);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(RegistrationPage.SubmitOTP);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(RegistrationPage.IncorrectOtpMsg.Displayed);
            BaseStep.wait.waitForElementInvisibilityLongWait(RegistrationPage.incorrectotpmsg, 10);

            string randomOtp3 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(RegistrationPage.EnterOTP, randomOtp3);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp3);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(RegistrationPage.SubmitOTP);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(RegistrationPage.IncorrectOtpMsg.Displayed);
            BaseStep.wait.waitForElementInvisibilityLongWait(RegistrationPage.incorrectotpmsg, 10);


            BaseStep.wait.genericWait(3000);
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var AttemptCounts = sortedEntities.Select(entity => entity.AttemptCount).ToArray();
            Validate.assertEquals(3, AttemptCounts[index], "AttemptCount is not increased", true);
            Report.childlog.Log(Status.Info, "AttemptCount for otp increased  is - " + AttemptCounts[index]);
        }

        public static async Task userRequestedAndResendOTPClickedThanOtpPopupClose(string phonenumber, string password, int index)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            BaseStep.wait.waitTillPageLoad();
            

            BaseStep.wait.genericWait(2000);
            var sortedEntities1 = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var pinset1 = sortedEntities1.Select(entity => entity.Pin).ToArray();
            string OtpSet1 = pinset1[index].ToString();
            Report.childlog.Log(Status.Info, "Otps is " + pinset1.ToString());

            BaseStep.wait.waitForElementInvisibilityLongWait(RegistrationPage.resendotptimer, 30);
            BaseStep.click(RegistrationPage.ResendOtpsBtn);
            BaseStep.wait.waitTillPageLoad();

            BaseStep.click(RegistrationPage.OtpPopupCutBtn);
            

            BaseStep.click(RegistrationPage.RegisterBtn);
            BaseStep.wait.waitTillPageLoad();

            
            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.getotppopupbtn, 60);
            BaseStep.click(RegistrationPage.GetOtpPopUpBtn);
            BaseStep.wait.waitTillPageLoad();

            Assert.IsTrue(RegistrationPage.SessionExpiryTimer.Displayed);
            Validate.takestepFullScreenShot("Session Expiry countdown start", Status.Info);

            
            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.enterotp, 60);
            BaseStep.sendkeys(RegistrationPage.EnterOTP, OtpSet1.ToString());
            Report.childlog.Log(Status.Info, "2nd otp from set2 is " + OtpSet1);            
            BaseStep.click(RegistrationPage.SubmitOTP);
            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.genericWait(2000);
            var sortedEntities1a = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var AttemptCount = sortedEntities1a.Select(entity => entity.AttemptCount).ToArray();
            var IsVerified = sortedEntities1a.Select(entity => entity.IsVerified).ToArray();
            var SessionCloseReason = sortedEntities1a.Select(entity => entity.SessionCloseReason).ToArray();
            Report.childlog.Log(Status.Info, "AttemptCount of Set 2_Otp 2" + AttemptCount[0] + ", IsVerified is " + IsVerified[0] + " and SessionCloseReason is " + SessionCloseReason[0]);
            Validate.assertEquals(1, AttemptCount[1], "AttemptCount is not expected", true);
            Validate.assertEquals(true, IsVerified[1], "IsVerified is not expected", true);
            Validate.assertEquals("SuccessClosure", SessionCloseReason[1], "SuccessClosure is not expected", true);

        }

        public static async Task userRequestedAndResendOTPClickedThanBrowserCloseAndSixtimesIncorrect(string phonenumber, string password)
        {
           
            RegistrationPage RegistrationPage = new RegistrationPage();

            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.genericWait(2000);
            var sortedEntities1 = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var pinset1 = sortedEntities1.Select(entity => entity.Pin).ToArray();
            Report.childlog.Log(Status.Info, "Otps is " + pinset1.ToString());

            BaseStep.wait.waitForElementInvisibilityLongWait(RegistrationPage.resendotptimer, 30);
            BaseStep.click(RegistrationPage.ResendOtpsBtn);
            BaseStep.wait.waitTillPageLoad();

            BaseStep.click(RegistrationPage.OtpPopupCutBtn);


            BaseStep.click(RegistrationPage.RegisterBtn);
            BaseStep.wait.waitTillPageLoad();


            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.getotppopupbtn, 60);
            BaseStep.click(RegistrationPage.GetOtpPopUpBtn);
            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.sessionexpirytimer, 60);
            Assert.IsTrue(RegistrationPage.SessionExpiryTimer.Displayed);
            Validate.takestepFullScreenShot("Session Expiry countdown start", Status.Info);


            incorrectOTPisentered6timeswithinfirst5minutes(phonenumber, password, 1);

        }

        public static async Task userRequestedAndResendOTPClickedThanBrowserCloseAndExpireIn10Mins(string phonenumber, string password)
        {
            RegistrationPage RegistrationPage = new RegistrationPage();

            BaseStep.wait.waitTillPageLoad();

            
            BaseStep.wait.genericWait(2000);
            var sortedEntities1 = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var pinset1 = sortedEntities1.Select(entity => entity.Pin).ToArray();
            Report.childlog.Log(Status.Info, "Otps is " + pinset1.ToString());

            BaseStep.wait.waitForElementInvisibilityLongWait(RegistrationPage.resendotptimer, 30);
            BaseStep.click(RegistrationPage.ResendOtpsBtn);
            BaseStep.wait.waitTillPageLoad();

            BaseStep.click(RegistrationPage.OtpPopupCutBtn);


            BaseStep.click(RegistrationPage.RegisterBtn);
            BaseStep.wait.waitTillPageLoad();


            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.getotppopupbtn, 60);
            BaseStep.click(RegistrationPage.GetOtpPopUpBtn);
            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.waitForElementCliackableLongWait(RegistrationPage.sessionexpirytimer, 60);
            Assert.IsTrue(RegistrationPage.SessionExpiryTimer.Displayed);
            Validate.takestepFullScreenShot("Session Expiry countdown start", Status.Info);
            
            BaseStep.wait.waitForElementInvisibilityLongWait(RegistrationPage.sessionexpirytimer, 6000000);
            Validate.takestepFullScreenShot("Session Expired", Status.Info);
           
        }
    }
}
