using AventStack.ExtentReports;
using JM.MainUtils;
using JM.Test.Pages;
using JMAutomation.MainResources;
using JMAutomation.MainUtils;
using OpenQA.Selenium.Chrome;

namespace JMAutomation
{
    public class ForgotPasswordPageSteps
    {
        public static void forgotPasswordatLoginPage(string phoneNumber)
        {

            loginPage loginPage = new loginPage();
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.idnumber, 60);
            Validate.takestepFullScreenShot("landingPage", Status.Info);

            BaseStep.click(loginPage.LoginIconOnLandingPage);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);
            Validate.takestepFullScreenShot("LoginPage", Status.Info);

            BaseStep.wait.genericWait(5000);
            BaseStep.click(loginPage.ForgotPassword);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);
            Validate.takestepFullScreenShot("ForgotPassword Page", Status.Info);

            Report.childlog.Log(Status.Info, "Delete all contents from the Otp Table");
            OTPStorageAccount.deleteOTPTableAsync(phoneNumber);

            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phoneNumber);
            Validate.takestepFullScreenShot("Phone Number is entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();
        }

        public static async Task verifytwoOTPgetsgeneratedintheOTPtable(string phoneNumber)
        {
            BaseStep.wait.genericWait(2000);
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
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.resendotptimer, 20);
            Validate.takestepFullScreenShot("Countdown timer appeared", Status.Info);
            BaseStep.wait.waitForElementInvisibilityLongWait(ForgotPasswordPage.resendotptimer, 30);

            bool ResendOtpTimer = ForgotPasswordPage.isResendOtpTimerDisplayed();
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

        //clicking resend OTP then the Resend OTP button gets disable

        public static async Task clickResendOTPthentheResendOTPbuttonGetsDisable(string phonenumber)
        {
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();

            BaseStep.click(ForgotPasswordPage.ResendOtpBtn);
            BaseStep.wait.waitTillPageLoad();
            int hitcount = await OTPStorageAccount.FetchthedatafromthetableAsyncofIntType(phonenumber, 2, "hitcount");
            Assert.AreEqual(1, hitcount);
            Assert.IsTrue(!ForgotPasswordPage.isResendOtpBtnEnabled()) ;
            Report.childlog.Log(Status.Info, "Resend OTP button gets disable after clicking on it");

            string ResendOtpMsg = BaseStep.getText.text(ForgotPasswordPage.ResendOtpMsg);
            Report.childlog.Log(Status.Info, "Otp is resend succesfully and message visible is " + ResendOtpMsg);



        }
        //Check and verify if the resend button is enter within 5 minutes then 1st OTP get accepted or not
        public static async Task firstOtpEnterAfterClickingonResendOtpAsync(string phonenumber, string password)
        {

            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            BaseStep.wait.genericWait(2000);        

            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var pins = sortedEntities.Select(entity => entity.Pin).ToArray();

            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.enterotp, 10);
            BaseStep.sendkeys(ForgotPasswordPage.EnterOtp, pins[1].ToString());
            Report.childlog.Log(Status.Info, "1st otp is " + pins[1]);

            BaseStep.sendkeys(ForgotPasswordPage.Password, password);
            BaseStep.sendkeys(ForgotPasswordPage.ConfirmPassword, password);

            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();

            Report.childlog.Log(Status.Info, "Resend button is enter within 5 minutes then 1st OTP is  accepted ");

            var sortedEntities2 = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var AttemptCounts = sortedEntities2.Select(entity => entity.AttemptCount).ToArray();
            Validate.assertEquals(1, AttemptCounts[1], "AttemptCount is not increased", false);
            Report.childlog.Log(Status.Info, "AttemptCount for 1st otp is " + AttemptCounts[1] + "and otp is accepted");

            var SessionCloseReasons = sortedEntities2.Select(entity => entity.SessionCloseReason).ToArray();
            Validate.assertEquals("SuccessClosure", SessionCloseReasons[0], "SuccessClosure is not mention for 1st otp", true);
            Validate.assertEquals("SuccessClosure", SessionCloseReasons[1], "SuccessClosure is not mention for 2nd otp", true);
            Report.childlog.Log(Status.Info, "log of SuccessClosure for both OTP is visible");
        }

        //Check and verify if the resend button is enter within 5 minutes then 2nd OTP get accepted or not
        public static async Task secondOtpEnterAfterClickingonResendOtpAsync(string phonenumber, string password)
        {
            loginPage loginPage = new loginPage();
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);

            BaseStep.click(loginPage.ForgotPassword);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);


            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.genericWait(3000);
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var pins = sortedEntities.Select(entity => entity.Pin).ToArray();

            BaseStep.wait.waitTillPageLoad();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, pins[0].ToString());
            Report.childlog.Log(Status.Info, "2nd otp is " + pins[0]);
            BaseStep.sendkeys(ForgotPasswordPage.Password, password);
            BaseStep.sendkeys(ForgotPasswordPage.ConfirmPassword, password);

            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementCliackableLongWait(loginPage.forgotpassword, 10);

        }


        //Check and verify if OTP is not entered within 10 minutes and user click on resend OTP after 5 minutes then the session gets expires or not

        // Check and verify when the 10 minutes session gets timeout then SessionCloseReason column should have "TimeoutClosure" for both the OTP's
        public static async Task sessionExpireafter10minsifOtpNotEnteredEvenAfterClickingOnResendButton(string phonenumber)
        {
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            
            BaseStep.wait.genericWait(300000);
            Report.childlog.Log(Status.Info, "After 5 mins, Now Resend OTP button gets disable after clicking on it. Still no Otp is entered");
            Validate.takestepFullScreenShot("No otp is enetered", Status.Info);
            await clickResendOTPthentheResendOTPbuttonGetsDisable(phonenumber);

          
            BaseStep.wait.genericWait(255000);
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sessionexpirymsg,7000);
            string SessionExpiryMsg = BaseStep.getText.text(ForgotPasswordPage.SessionExpiryMsg);
            Assert.IsTrue(ForgotPasswordPage.isSessionExpiryMsgDisplayed());
            Report.childlog.Log(Status.Info, "After 10 mins, Session got expired and error message visible is " + SessionExpiryMsg);


            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var SessionCloseReasons = sortedEntities.Select(entity => entity.SessionCloseReason).ToArray();
            Validate.assertEquals("TimeoutClosure", SessionCloseReasons[0], "TimeoutClosure is not mention for 1st otp", true);
            Validate.assertEquals("TimeoutClosure", SessionCloseReasons[1], "TimeoutClosure is not mention for 2nd otp", true);
            Report.childlog.Log(Status.Info, "log of TimeoutClosure for both OTP is visible");
        }

        //Check and verify if First OTP which is activated is getting accepted or not
        //Check and verify when user successfully enter the first OTP then SessionCloseReason column
        //should have a log of SuccessClosure for both OTP
        public static async Task verify1stActiveOtpisAccepted(string phoneNumber, string password)
        {
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            loginPage loginPage = new loginPage();

            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var hitCounts = sortedEntities.Select(entity => entity.HitCount).ToArray();
            Assert.IsTrue(hitCounts[1] == 1);
            Report.childlog.Log(Status.Info, "HitCount for 1st otp is " + hitCounts[1]);

            var pins = sortedEntities.Select(entity => entity.Pin).ToArray();

            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.enterotp, 10);
            BaseStep.sendkeys(ForgotPasswordPage.EnterOtp, pins[1].ToString());
            Report.childlog.Log(Status.Info, "1st otp is " + pins[1]);
            BaseStep.sendkeys(ForgotPasswordPage.Password, password);
            BaseStep.sendkeys(ForgotPasswordPage.ConfirmPassword, password);

            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementCliackableLongWait(loginPage.forgotpassword, 10);

            var sortedEntities2 = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var AttemptCounts = sortedEntities2.Select(entity => entity.AttemptCount).ToArray();
            Validate.assertEquals(1, AttemptCounts[1], "AttemptCount is not increased", false);
            Report.childlog.Log(Status.Info, "AttemptCount for 1st otp is " + AttemptCounts[1] + "and otp is accepted");

            var SessionCloseReasons = sortedEntities2.Select(entity => entity.SessionCloseReason).ToArray();
            Validate.assertEquals("SuccessClosure", SessionCloseReasons[0], "SuccessClosure is not mention for 1st otp", true);
            Validate.assertEquals("SuccessClosure", SessionCloseReasons[1], "SuccessClosure is not mention for 2nd otp", true);
            Report.childlog.Log(Status.Info, "log of SuccessClosure for both OTP is visible");
        }

        public static async Task incorrectOtpIsNotAcceptedandAttemptcountIncreaseAsync(string phoneNumber, string password)
        {

            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            loginPage loginPage = new loginPage();

            OTPStorageAccount.deleteOTPTableAsync(phoneNumber);
            //Check and verify if incorrect OTP is entered within first 5 minutes then attempt count
            //gets incremented for the 1st OTP and it should not get accepted

            BaseStep.click(loginPage.ForgotPassword);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phoneNumber);
            Validate.takestepFullScreenShot("Phone Number is entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);

            BaseStep.wait.waitTillPageLoad();
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var pins = sortedEntities.Select(entity => entity.Pin).ToArray();


            BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.enterotp, 60);
            BaseStep.sendkeys(ForgotPasswordPage.EnterOtp, pins[0].ToString());
            Report.childlog.Log(Status.Info, "Incorrect otp is entered 1st time is " + pins[0]);
            BaseStep.sendkeys(ForgotPasswordPage.Password, password);
            BaseStep.sendkeys(ForgotPasswordPage.ConfirmPassword, password);

            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibility(ForgotPasswordPage.incorrectotpmsg);
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);


            var sortedEntities2 = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var AttemptCounts = sortedEntities2.Select(entity => entity.AttemptCount).ToArray();

            Validate.assertEquals(1, AttemptCounts[0], "AttemptCount is not increased", false);
            Report.childlog.Log(Status.Info, "AttemptCount for 1st otp is " + AttemptCounts[1] + "and incorect otp is not accepted");

            //Check and verify if incorrect OTP is entered after first 5 minutes then attempt count gets recorded
            //for the 2nd OTP and it should not get accepted

            OTPStorageAccount.updateOtpExpiryTimeAsync(phoneNumber, 0);
            BaseStep.wait.genericWait(60000);
            Report.childlog.Log(Status.Info, "After 5 mins, Again enter incorrect Otp and it will increase second Otp attempt count");
            string randomOtp = GenericUtils.randomInteger(100000, 999999).ToString();

            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp);

            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibility(ForgotPasswordPage.incorrectotpmsg);
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);


            var sortedEntities3 = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var AttemptCounts2 = sortedEntities3.Select(entity => entity.AttemptCount).ToArray();
            var pins2 = sortedEntities2.Select(entity => entity.Pin).ToArray();

            Validate.assertEquals(1, AttemptCounts2[0], "AttemptCount is not increased", false);
            Report.childlog.Log(Status.Info, "AttemptCount for Random otp is increased after 5 mins is - " + AttemptCounts2[0] + "and incorect otp is not accepted");


            //Check and verify if 2nd correct OTP is entered after first 5 minutes then attempt count gets recorded
            //for the 2nd OTP and it should not get accepted

            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, pins2[1].ToString());
            Report.childlog.Log(Status.Info, " otp entered is " + pins2[1]);
            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);

            BaseStep.wait.waitTillPageLoad();
            var sortedEntities4 = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var AttemptCounts3 = sortedEntities4.Select(entity => entity.AttemptCount).ToArray();
            Validate.assertEquals(2, AttemptCounts3[0], "AttemptCount is not increased", false);
            Report.childlog.Log(Status.Info, "AttemptCount for 2nd otp is increased after 5 mins is - " + AttemptCounts3[0] + "and otp is not accepted");

            BaseStep.wait.waitForElementVisibility(ForgotPasswordPage.incorrectotpmsg);
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            /*Check and verify if the 1st OTP is entered after
            5 minutes then if its getting accepted or not*/

            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, pins2[0].ToString());
            Report.childlog.Log(Status.Info, " otp entered is " + pins2[0]);
            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);

            BaseStep.wait.genericWait(3000);
            var sortedEntities5 = await OTPStorageAccount.connectWithStorageAccountAsync(phoneNumber);
            var AttemptCounts4 = sortedEntities5.Select(entity => entity.AttemptCount).ToArray();
            Validate.assertEquals(3, AttemptCounts4[0], "AttemptCount is not increased", false);
            Report.childlog.Log(Status.Info, "AttemptCount for 2nd otp is increased after 5 mins when enter 1st otp is - " + AttemptCounts4[0] + "and otp is not accepted");
            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);

            BaseStep.wait.waitForElementVisibility(ForgotPasswordPage.incorrectotpmsg);
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

        }

        //Check and verify if the resend button is enter after 5 minutes then 2nd OTP get accepted or not

        public static async Task secondOTPgetAcceptedAfter5minsResendBtnClickAsync(string phonenumber, string password)
        {
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();

            BaseStep.click(ForgotPasswordPage.ResendOtpBtn);
            BaseStep.wait.waitTillPageLoad();
            secondOtpEnterAfterClickingonResendOtpAsync(phonenumber, password);
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var SessionCloseReasons = sortedEntities.Select(entity => entity.SessionCloseReason).ToArray();
            Validate.assertEquals("SuccessClosure", SessionCloseReasons[0], "SuccessClosure is not mention for 1st otp", true);
            Validate.assertEquals("SuccessClosure", SessionCloseReasons[1], "SuccessClosure is not mention for 2nd otp", true);
            Report.childlog.Log(Status.Info, "log of SuccessClosure for both OTP is visible");
        }

        //Check and verify if incorrect OTP is entered 6 times within first 5 minutes then OTP session gets expired and attempt
        //count gets incremented for the 1st OTP

        //Check and verify when user fails to enter the OTP 6 times within first 5 minutes then SessionCloseReason column should
        //have a log of FailureClosure for both OTP
        public static async Task incorrectOTPisentered6timeswithinfirst5minutes(string phonenumber, string password, int index)
        {
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();

            BaseStep.wait.waitTillPageLoad();

            string randomOtp1 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.enterotp, 60);
            BaseStep.sendkeys(ForgotPasswordPage.EnterOtp, randomOtp1);
            Report.childlog.Log(Status.Info, "1st otp is " + randomOtp1);
            BaseStep.sendkeys(ForgotPasswordPage.Password, password);
            BaseStep.sendkeys(ForgotPasswordPage.ConfirmPassword, password);
            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp2 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp2);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp2);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp3 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp3);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp3);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp4 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp4);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp4);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp5 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp5);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp5);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp6 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp6);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp6);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Validate.takestepFullScreenShot("Session Expired", Status.Info);

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
            loginPage loginPage = new loginPage();

            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            Report.childlog.Log(Status.Info, "Delete all contents from the Otp Table");
            OTPStorageAccount.deleteOTPTableAsync(phonenumber);

            BaseStep.click(loginPage.LoginIconOnForgotPass);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);
            Validate.takestepFullScreenShot("LoginPage", Status.Info);


            BaseStep.click(loginPage.ForgotPassword);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);
            Validate.takestepFullScreenShot("ForgotPassword Page", Status.Info);
            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
            Validate.takestepFullScreenShot("Phone Number is entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();
            await OTPStorageAccount.updateOtpExpiryTimeAsync(phonenumber, 1);
            BaseStep.wait.genericWait(120000);

            incorrectOTPisentered6timeswithinfirst5minutes(phonenumber, password, 1);
            Report.childlog.Log(Status.Info, "incorrect OTP is entered 6 times After 5 minutes");

        }

        //Check and verify after resending OTP after first 5 minutes and
        //entering incorrect OTP for 6 times
        public static async Task incorrectOTPisEntered6timesAfterResending5minutes(string phonenumber, string password)
        {
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            loginPage loginPage = new loginPage();

            Report.childlog.Log(Status.Info, "Delete all contents from the Otp Table");
            await OTPStorageAccount.deleteOTPTableAsync(phonenumber);


            BaseStep.click(loginPage.LoginIconOnForgotPass);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);
            Validate.takestepFullScreenShot("LoginPage", Status.Info);


            BaseStep.click(loginPage.ForgotPassword);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);

            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
            Validate.takestepFullScreenShot("Phone Number is entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();

            await OTPStorageAccount.updateOtpExpiryTimeAsync(phonenumber, 1);
            BaseStep.wait.genericWait(120000);
            BaseStep.click(ForgotPasswordPage.ResendOtpBtn);
            incorrectOTPisentered6timeswithinfirst5minutes(phonenumber, password, 1);
            BaseStep.wait.waitTillPageLoad();

        }

        public static async Task incorrectOTPisentered5timesAnd6timeCorrectwithinfirst5minutes(string phonenumber, string password, int index)
        {
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();

            BaseStep.wait.waitTillPageLoad();
            try
            {
                if (ForgotPasswordPage.SendOtpBtn.Displayed)
                {

                    BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.sendotpbtn, 60);
                    BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
                    Validate.takestepFullScreenShot("Phone Number is entered", Status.Info);
                    BaseStep.click(ForgotPasswordPage.SendOtpBtn);
                    BaseStep.wait.waitTillPageLoad();
                }
            }
            catch
            {
                Validate.takestepFullScreenShot("Enter Otp", Status.Info);
            }


            string randomOtp1 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.enterotp, 60);
            BaseStep.sendkeys(ForgotPasswordPage.EnterOtp, randomOtp1);
            Report.childlog.Log(Status.Info, "1st otp is " + randomOtp1);
            BaseStep.sendkeys(ForgotPasswordPage.Password, password);
            BaseStep.sendkeys(ForgotPasswordPage.ConfirmPassword, password);
            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp2 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp2);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp2);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp3 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp3);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp3);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp4 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp4);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp4);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp5 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp5);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp5);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            BaseStep.wait.genericWait(3000);
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var pins = sortedEntities.Select(entity => entity.Pin).ToArray();

            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, pins[index].ToString());
            Report.childlog.Log(Status.Info, "Correct Otp entered is " + pins[index].ToString());
            Validate.takestepFullScreenShot("Correct otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Validate.takestepFullScreenShot("Otp is entered", Status.Info);

        }
        /*Check and verify entering incorrect OTP after first 5 minutes for first, second, third, fourth, fifth attempt 
            * and then entering 2nd OTP correctly in the 6th attempt*/
        public static async Task incorrectOTPisentered5timesAnd6time2ndCorrectOtpAfter5minutes(string phonenumber, string password)
        {
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            loginPage loginPage = new loginPage();

            Report.childlog.Log(Status.Info, "Delete all contents from the Otp Table");
            await OTPStorageAccount.deleteOTPTableAsync(phonenumber);

            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);
            Validate.takestepFullScreenShot("LoginPage", Status.Info);


            BaseStep.click(loginPage.ForgotPassword);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.sendotpbtn, 60);
            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
            Validate.takestepFullScreenShot("Phone Number is entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();

            await OTPStorageAccount.updateOtpExpiryTimeAsync(phonenumber, 1);
            BaseStep.wait.genericWait(120000);
            incorrectOTPisentered5timesAnd6timeCorrectwithinfirst5minutes(phonenumber, password, 0);
            Assert.IsTrue(ForgotPasswordPage.SessionExpiryMsg.Displayed);
            Report.childlog.Log(Status.Info, "Second otp is enterd 6th time but not valid");
        }

        //Check and verify after resending OTP after first 5 minutes and entering incorrect OTP for first, second, third,
        //fourth, fifth attempt and then entering 2nd OTP correctly in the 6th attempt

        public static async Task incorrectOTPisentered5timesAnd6time2ndCorrectAfterResending5minutes(string phonenumber, string password)
        {
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();

            Report.childlog.Log(Status.Info, "Delete all contents from the Otp Table");
            await OTPStorageAccount.deleteOTPTableAsync(phonenumber);

            BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.sendotpbtn, 60);
            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
            Validate.takestepFullScreenShot("Phone Number is entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();

            await OTPStorageAccount.updateOtpExpiryTimeAsync(phonenumber, 1);
            BaseStep.wait.genericWait(120000);
            BaseStep.click(ForgotPasswordPage.ResendOtpBtn);
            BaseStep.wait.waitTillPageLoad();
            incorrectOTPisentered5timesAnd6timeCorrectwithinfirst5minutes(phonenumber, password, 0);
            Report.childlog.Log(Status.Info, "Second otp is enterd 6th time and is valid");

        }
        //Check and verify after resending OTP within first 5 minutes and entering incorrect OTP for first, second, third, fourth, fifth attempt
        //and then entering 2nd OTP correctly in the 6th attempt

        public static async Task incorrectOTPisentered5timesAnd6time2ndCorrectAfterResendingwithin5minutes(string phonenumber, string password)
        {
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            loginPage loginPage = new loginPage();

            Report.childlog.Log(Status.Info, "Delete all contents from the Otp Table");
            OTPStorageAccount.deleteOTPTableAsync(phonenumber);

            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);
            Validate.takestepFullScreenShot("LoginPage", Status.Info);


            BaseStep.click(loginPage.ForgotPassword);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);
            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
            Validate.takestepFullScreenShot("Phone Number is entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.click(ForgotPasswordPage.ResendOtpBtn);
            incorrectOTPisentered5timesAnd6timeCorrectwithinfirst5minutes(phonenumber, password, 0);
            Report.childlog.Log(Status.Info, "Second otp is enterd 6th time within 5 mins and is not valid");
        }

        //Check and verify entering incorrect OTP for first, second, third, fourth, fifth attempt after 5 minutes
        //and then resend OTP and then entering 2nd OTP correctly in the 6th attempt

        //Check and verify entering incorrect OTP for first, second, third, fourth, fifth attempt after 5 minutes
        //and then resend OTP and then entering 6th OTP incorrectly
        public static async Task incorrectOTPisentered5timesAndAfterResending6timeOtpafter5minutes(string phonenumber, string password, int index)
        {
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            loginPage loginPage = new loginPage();

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.genericWait(5000);

            try
            {
                if (ForgotPasswordPage.SendOtpBtn.Displayed)
                {

                    OTPStorageAccount.deleteOTPTableAsync(phonenumber);
                    BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.sendotpbtn, 60);
                    BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
                    Validate.takestepFullScreenShot("Phone Number is entered", Status.Info);
                    BaseStep.click(ForgotPasswordPage.SendOtpBtn);
                    BaseStep.wait.waitTillPageLoad();
                }
            }
            catch
            {
                Validate.takestepFullScreenShot("Enter Otp", Status.Info);
            }


            string randomOtp1 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.enterotp, 60);
            BaseStep.sendkeys(ForgotPasswordPage.EnterOtp, randomOtp1);
            Report.childlog.Log(Status.Info, "1st otp is " + randomOtp1);
            BaseStep.sendkeys(ForgotPasswordPage.Password, password);
            BaseStep.sendkeys(ForgotPasswordPage.ConfirmPassword, password);
            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp2 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp2);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp2);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp3 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp3);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp3);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp4 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp4);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp4);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp5 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp5);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp5);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            BaseStep.wait.genericWait(3000);
            await OTPStorageAccount.updateOtpExpiryTimeAsync(phonenumber, 1);
            BaseStep.wait.genericWait(120000);
            BaseStep.click(ForgotPasswordPage.ResendOtpBtn);
            Assert.IsTrue(ForgotPasswordPage.ResendOtpMsg.Displayed);
            BaseStep.wait.genericWait(3000);
            var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var pins = sortedEntities.Select(entity => entity.Pin).ToArray();

            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, pins[index].ToString());
            Report.childlog.Log(Status.Info, "Correct Otp entered is " + pins[index].ToString());
            Validate.takestepFullScreenShot("Correct otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            Validate.takestepFullScreenShot("Otp is entered", Status.Info);
            BaseStep.wait.waitTillPageLoad();
        }


        public static async Task incorrectOTPisentered5timesAndAfterResending6time2ndCorrectWithin5minutes(string phonenumber, string password, int index)
        {
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            loginPage loginPage = new loginPage();

            BaseStep.wait.waitTillPageLoad();
            OTPStorageAccount.deleteOTPTableAsync(phonenumber);


            try
            {
                if (ForgotPasswordPage.SendOtpBtn.Displayed)
                {

                    BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.sendotpbtn, 60);
                    BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
                    Validate.takestepFullScreenShot("Phone Number is entered", Status.Info);
                    BaseStep.click(ForgotPasswordPage.SendOtpBtn);
                    BaseStep.wait.waitTillPageLoad();
                }
            }
            catch
            {
                Validate.takestepFullScreenShot("Enter otp", Status.Info);
            }


            string randomOtp1 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.enterotp, 60);
            BaseStep.sendkeys(ForgotPasswordPage.EnterOtp, randomOtp1);
            Report.childlog.Log(Status.Info, "1st otp is " + randomOtp1);
            BaseStep.sendkeys(ForgotPasswordPage.Password, password);
            BaseStep.sendkeys(ForgotPasswordPage.ConfirmPassword, password);
            Validate.takestepFullScreenShot("All Fields are entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp2 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp2);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp2);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp3 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp3);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp3);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp4 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp4);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp4);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);

            string randomOtp5 = GenericUtils.randomInteger(100000, 999999).ToString();
            BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp5);
            Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp5);
            Validate.takestepFullScreenShot("otp", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);


            BaseStep.wait.waitForElementInvisibilityLongWait(ForgotPasswordPage.resendotptimer, 10);
            BaseStep.click(ForgotPasswordPage.ResendOtpBtn);
            Assert.IsTrue(ForgotPasswordPage.ResendOtpMsg.Displayed);
            BaseStep.wait.waitTillPageLoad();

            if (index == 0)
            {
                BaseStep.wait.genericWait(3000);
                var sortedEntities = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
                var pins = sortedEntities.Select(entity => entity.Pin).ToArray();

                BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, pins[index].ToString());
                Report.childlog.Log(Status.Info, "Correct Otp entered is " + pins[index].ToString());
                Validate.takestepFullScreenShot("Correct otp", Status.Info);
            }
            else
            {
                string randomOtp6 = GenericUtils.randomInteger(100000, 999999).ToString();
                BaseStep.clearAndSendkeys(ForgotPasswordPage.EnterOtp, randomOtp6);
                Report.childlog.Log(Status.Info, "Random Otp entered is " + randomOtp6);
                Validate.takestepFullScreenShot("otp", Status.Info);
                BaseStep.click(ForgotPasswordPage.SubmitBtn);
                BaseStep.wait.waitTillPageLoad();
                Assert.IsTrue(ForgotPasswordPage.IncorrectOtpMsg.Displayed);
            }

            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();
            Validate.takestepFullScreenShot("Otp is entered", Status.Info);
        }

        public static async Task incorrectOTPisentered5timesAndAfterResending6timeIncorrectWithin5minutes(string phonenumber, string password, int index)
        {
            loginPage loginPage = new loginPage();
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);

            BaseStep.click(loginPage.ForgotPassword);
            Validate.takestepFullScreenShot("LoginPage", Status.Info);

            incorrectOTPisentered5timesAndAfterResending6time2ndCorrectWithin5minutes(phonenumber, password, index);
            Assert.IsTrue(ForgotPasswordPage.SessionExpiryMsg.Displayed);
            Report.childlog.Log(Status.Info, "incorrect OTP is entered 5 times And After Resending 6 time Incorrect Within 5 minutes");

        }
        /*Check and verify once the user successfully enter the first OTP within 5 minutes then he should not wait for 10 minutes for 
         * requesting new set of OTP*/

        public static async Task<int> userRequested2SetAndEnter1stOtpfromSet1(string phonenumber, string password, int index)
        {
            loginPage loginPage = new loginPage();
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();


            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.enterotp, 20);

            var sortedEntities1 = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var pinset1 = sortedEntities1.Select(entity => entity.Pin).ToArray();
            string firstOtpSet1 = pinset1[1].ToString();
            Report.childlog.Log(Status.Info, "Otps of Set 1" + pinset1.ToString());

            BaseStep.sendkeys(ForgotPasswordPage.EnterOtp, firstOtpSet1);
            Report.childlog.Log(Status.Info, "1st otp from set1 is " + firstOtpSet1);
            BaseStep.sendkeys(ForgotPasswordPage.Password, password);
            BaseStep.sendkeys(ForgotPasswordPage.ConfirmPassword, password);
            Validate.takestepFullScreenShot("1st otp from set1 is entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            Report.childlog.Log(Status.Info, "1st OTP accepted");
            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.genericWait(3000);
            var sortedEntities1a = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var AttemptCount = sortedEntities1a.Select(entity => entity.AttemptCount).ToArray();
            var IsVerified = sortedEntities1a.Select(entity => entity.IsVerified).ToArray();
            var SessionCloseReason = sortedEntities1a.Select(entity => entity.SessionCloseReason).ToArray();
            Report.childlog.Log(Status.Info, "AttemptCount of Set 1_otp 1" + AttemptCount[0] + ", IsVerified is " + IsVerified[0] + " and SessionCloseReason is " + SessionCloseReason[0]);
            Validate.assertEquals(1, AttemptCount[1], "AttemptCount is not expected", true);
            Validate.assertEquals(true, IsVerified[1], "IsVerified is not expected", true);
            Validate.assertEquals("SuccessClosure", SessionCloseReason[1], "SuccessClosure is not expected", true);

            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);

            BaseStep.click(loginPage.ForgotPassword);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);


            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.genericWait(3000);
            var sortedEntities2 = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var pinset2 = sortedEntities2.Select(entity => entity.Pin).ToArray();
            Report.childlog.Log(Status.Info, "Otps of Set 2" + pinset2);

            return pinset2[index];
        }


        /*Check and verify once the user have requested a new set of OTP within 10 minutes
         * then he can able to enter the first correct OTP in the new set*/
        public static async Task userRequested2SetAndEnter1stOtpfromSet2(string phonenumber, string password)
        {
            loginPage loginPage = new loginPage();
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();


            int pinset2 = await userRequested2SetAndEnter1stOtpfromSet1(phonenumber, password, 1);
            Report.childlog.Log(Status.Info, "1st Otp of Set 2" + pinset2.ToString());

            BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.enterotp, 60);
            BaseStep.sendkeys(ForgotPasswordPage.EnterOtp, pinset2.ToString());
            Report.childlog.Log(Status.Info, "1st otp from set2 is " + pinset2);
            BaseStep.sendkeys(ForgotPasswordPage.Password, password);
            BaseStep.sendkeys(ForgotPasswordPage.ConfirmPassword, password);
            Validate.takestepFullScreenShot("1st otp from set2 is entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();

            var sortedEntities1a = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var AttemptCount = sortedEntities1a.Select(entity => entity.Pin).ToArray();
            var IsVerified = sortedEntities1a.Select(entity => entity.IsVerified).ToArray();
            var SessionCloseReason = sortedEntities1a.Select(entity => entity.SessionCloseReason).ToArray();
            Report.childlog.Log(Status.Info, "AttemptCount of Set 2_Otp 1" + AttemptCount[0] + ", IsVerified is " + IsVerified[0] + " and SessionCloseReason is " + SessionCloseReason[0]);
            Validate.assertEquals(1, AttemptCount[0], "AttemptCount is not expected", true);
            Validate.assertEquals(true, IsVerified[0], "IsVerified is not expected", true);
            Validate.assertEquals("SuccessClosure", SessionCloseReason[0], "SuccessClosure is not expected", true);
        }

        /*Check and verify once the user have requested a new set of OTP then he can able 
         * to enter the second correct OTP after clicking on Resend OTP within 5 minutes*/

        public static async Task userRequested2SetAndEnter2ndOtpfromSet1AfterClickingResend(string phonenumber, string password)
        {
            loginPage loginPage = new loginPage();
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();
            OTPStorageAccount.deleteOTPTableAsync(phonenumber);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);

            BaseStep.click(loginPage.ForgotPassword);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);


            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();

            int secondpinset2 = await userRequested2SetAndEnter1stOtpfromSet1(phonenumber, password, 1);
            Report.childlog.Log(Status.Info, "2nd Otp of Set 2" + secondpinset2);


            BaseStep.wait.waitForElementInvisibilityLongWait(ForgotPasswordPage.resendotptimer, 60);
            BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.sendotpbtn, 60);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            Assert.IsTrue(ForgotPasswordPage.ResendOtpMsg.Displayed);
            BaseStep.wait.waitTillPageLoad();

            BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.enterotp, 60);
            BaseStep.sendkeys(ForgotPasswordPage.EnterOtp, secondpinset2.ToString());
            Report.childlog.Log(Status.Info, "2nd otp from set2 is " + secondpinset2);
            BaseStep.sendkeys(ForgotPasswordPage.Password, password);
            BaseStep.sendkeys(ForgotPasswordPage.ConfirmPassword, password);
            Validate.takestepFullScreenShot("2nd otp from set2 is entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
            BaseStep.wait.waitTillPageLoad();

            var sortedEntities1a = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var AttemptCount = sortedEntities1a.Select(entity => entity.Pin).ToArray();
            var IsVerified = sortedEntities1a.Select(entity => entity.IsVerified).ToArray();
            var SessionCloseReason = sortedEntities1a.Select(entity => entity.SessionCloseReason).ToArray();
            Report.childlog.Log(Status.Info, "AttemptCount of Set 2_Otp 2" + AttemptCount[0] + ", IsVerified is " + IsVerified[0] + " and SessionCloseReason is " + SessionCloseReason[0]);
            Validate.assertEquals(1, AttemptCount[0], "AttemptCount is not expected", true);
            Validate.assertEquals(true, IsVerified[0], "IsVerified is not expected", true);
            Validate.assertEquals("SuccessClosure", SessionCloseReason[0], "SuccessClosure is not expected", true);

        }

        /*Check and verify once the user have requested a new set of OTP and then he fails to enter the
         * OTP within 5 minutes*/
        public static async Task userRequested2SetAndFails2Set(string phonenumber, string password)
        {
            loginPage loginPage = new loginPage();
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();

            OTPStorageAccount.deleteOTPTableAsync(phonenumber);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);

            BaseStep.click(loginPage.ForgotPassword);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);


            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();

            await userRequested2SetAndEnter1stOtpfromSet1(phonenumber, password, 1);

            BaseStep.click(ForgotPasswordPage.SendOtpBtn);

            incorrectOTPisentered6timeswithinfirst5minutes(phonenumber, password, 1);

        }

        /*Check and verify once the user have requested a new set of OTP
         * and then if he doesn't enter the OTP then session gets timeout*/
        public static async Task userRequested2SetAndFails2SetwithoutEnterOtp(string phonenumber, string password)
        {

            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();

            await OTPStorageAccount.deleteOTPTableAsync(phonenumber);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);


            BaseStep.clearAndSendkeys(ForgotPasswordPage.SearchText, phonenumber);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();

            await userRequested2SetAndEnter1stOtpfromSet1(phonenumber, password, 1);

            await OTPStorageAccount.updateOtpExpiryTimeAsync(phonenumber, 1);
            BaseStep.wait.genericWait(120000);

            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sessionexpirymsg, 60);
            Assert.IsTrue(ForgotPasswordPage.SessionExpiryMsg.Displayed);
            Validate.takestepFullScreenShot("No Otp is enter", Status.Info);
            Report.childlog.Log(Status.Info, "Session expired for Second set");

        }

        /*Check and verify if user have requested an OTP and then clicked on Resend OTP
         * and again if he request an OTP within the OTP session then session should not
         * expire and could able to enter the 1st OTP correctly*/

        /*Check and verify if user have requested an OTP and then clicked on Resend OTP 
        * and again if he request an OTP within the OTP session then session should not
        * expire and could able to enter the 2nd OTP correctly before 5 minutes*/
        public static async Task userRequestedAndResendOTPClickedThanBrowserClose(string phonenumber, string password, int index)
        {
            loginPage loginPage = new loginPage();
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();

            BaseStep.wait.waitTillPageLoad();
            try
            {
                BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.enterotp, 5);
            }
            catch
            {

                BaseStep.click(loginPage.ForgotPassword);
                BaseStep.wait.waitTillPageLoad();
                BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);

                Report.childlog.Log(Status.Info, "Delete all contents from the Otp Table");
                OTPStorageAccount.deleteOTPTableAsync(phonenumber);

                BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
                BaseStep.click(ForgotPasswordPage.SendOtpBtn);
                BaseStep.wait.waitTillPageLoad();
            }

            BaseStep.wait.genericWait(2000);
            var sortedEntities1 = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var pinset1 = sortedEntities1.Select(entity => entity.Pin).ToArray();
            string OtpSet1 = pinset1[index].ToString();
            Report.childlog.Log(Status.Info, "Otps is " + pinset1.ToString());

            BaseStep.wait.waitForElementInvisibilityLongWait(ForgotPasswordPage.resendotptimer, 30);
            BaseStep.click(ForgotPasswordPage.ResendOtpBtn);
            BaseStep.wait.waitTillPageLoad();

            WebdriverSession.Driver.Close();
            Report.childlog.Log(Status.Info, "Close the browser and create a new visit and then access with same id and cellphone number within 10 minutes ");

            WebdriverSession.Driver = new ChromeDriver();
            WebdriverSession.Driver.Manage().Window.Maximize();
            loginPageSteps.loginT0App();
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.idnumber, 60);
            Validate.takestepFullScreenShot("landingPage", Status.Info);
            BaseStep.click(loginPage.LoginIconOnLandingPage);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);
            Validate.takestepFullScreenShot("LoginPage", Status.Info);

            BaseStep.wait.genericWait(2000);
            BaseStep.click(loginPage.ForgotPassword);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);
            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();

            try
            {
                BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sessionexpirytimer, 10);
                Assert.IsTrue(ForgotPasswordPage.SessionExpiryTimer.Displayed);
            }
            catch
            {
                WebdriverSession.refreshPage();
                BaseStep.wait.waitTillPageLoad();
                BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);
                BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
                BaseStep.click(ForgotPasswordPage.SendOtpBtn);
                BaseStep.wait.waitTillPageLoad();

            }


            BaseStep.sendkeys(ForgotPasswordPage.EnterOtp, OtpSet1.ToString());
            Report.childlog.Log(Status.Info, "2nd otp from set2 is " + OtpSet1);
            BaseStep.sendkeys(ForgotPasswordPage.Password, password);
            BaseStep.sendkeys(ForgotPasswordPage.ConfirmPassword, password);
            Validate.takestepFullScreenShot("2nd otp from set2 is entered", Status.Info);
            BaseStep.click(ForgotPasswordPage.SubmitBtn);
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

        /*Check and verify if user have requested an OTP and then clicked on Resend OTP
         * and again if he request an OTP within the OTP session then session should not
         * expire and could able to enter the incorrect OTP 6 times before 5 minutes*/

        public static async Task userRequestedAndResendOTPClickedThanBrowserCloseAndSixtimesIncorrect(string phonenumber, string password)
        {
            loginPage loginPage = new loginPage();
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();

            BaseStep.wait.waitTillPageLoad();

            BaseStep.click(loginPage.ForgotPassword);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);

            Report.childlog.Log(Status.Info, "Delete all contents from the Otp Table");
            OTPStorageAccount.deleteOTPTableAsync(phonenumber);

            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();


            BaseStep.wait.genericWait(2000);
            var sortedEntities1 = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var pinset1 = sortedEntities1.Select(entity => entity.Pin).ToArray();           
            Report.childlog.Log(Status.Info, "Otps is " + pinset1.ToString());

            BaseStep.wait.waitForElementInvisibilityLongWait(ForgotPasswordPage.resendotptimer, 30);
            BaseStep.click(ForgotPasswordPage.ResendOtpBtn);
            BaseStep.wait.waitTillPageLoad();

            WebdriverSession.Driver.Close();
            Report.childlog.Log(Status.Info, "Close the browser and create a new visit and then access with same id and cellphone number within 10 minutes ");

            WebdriverSession.Driver = new ChromeDriver();
            WebdriverSession.Driver.Manage().Window.Maximize();
            loginPageSteps.loginT0App();
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.idnumber, 60);
            Validate.takestepFullScreenShot("landingPage", Status.Info);
            BaseStep.click(loginPage.LoginIconOnLandingPage);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);
            Validate.takestepFullScreenShot("LoginPage", Status.Info);

            BaseStep.wait.genericWait(2000);
            BaseStep.click(loginPage.ForgotPassword);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);
            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();

            try
            {
                BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sessionexpirytimer, 10);
                Assert.IsTrue(ForgotPasswordPage.SessionExpiryTimer.Displayed);
            }
            catch
            {
                WebdriverSession.refreshPage();
                BaseStep.wait.waitTillPageLoad();
                BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);
                BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
                BaseStep.click(ForgotPasswordPage.SendOtpBtn);
                BaseStep.wait.waitTillPageLoad();

            }

            incorrectOTPisentered6timeswithinfirst5minutes(phonenumber, password, 1);

        }

        /*Check and verify if user have requested an OTP and then clicked on Resend OTP
         * and again if he request an OTP within the OTP session then session should not
         * expire and if user wait for 10 minutes then session should get expire*/

        public static async Task userRequestedAndResendOTPClickedThanBrowserCloseAndExpireIn10Mins(string phonenumber, string password)
        {
            loginPage loginPage = new loginPage();
            ForgotPasswordPage ForgotPasswordPage = new ForgotPasswordPage();

            BaseStep.wait.waitTillPageLoad();
            
            BaseStep.wait.waitForElementCliackableLongWait(ForgotPasswordPage.sendotpbtn, 60);
            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();


            BaseStep.wait.genericWait(2000);
            var sortedEntities1 = await OTPStorageAccount.connectWithStorageAccountAsync(phonenumber);
            var pinset1 = sortedEntities1.Select(entity => entity.Pin).ToArray();
            Report.childlog.Log(Status.Info, "Otps is " + pinset1.ToString());

            BaseStep.wait.waitForElementInvisibilityLongWait(ForgotPasswordPage.resendotptimer, 30);
            BaseStep.click(ForgotPasswordPage.ResendOtpBtn);
            BaseStep.wait.waitTillPageLoad();

            WebdriverSession.Driver.Close();
            Report.childlog.Log(Status.Info, "Close the browser and create a new visit and then access with same id and cellphone number within 10 minutes ");

            WebdriverSession.Driver = new ChromeDriver();
            WebdriverSession.Driver.Manage().Window.Maximize();
            loginPageSteps.loginT0App();
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.idnumber, 60);
            Validate.takestepFullScreenShot("landingPage", Status.Info);
            BaseStep.click(loginPage.LoginIconOnLandingPage);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);
            Validate.takestepFullScreenShot("LoginPage", Status.Info);

            BaseStep.wait.genericWait(2000);
            BaseStep.click(loginPage.ForgotPassword);

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);
            BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
            BaseStep.click(ForgotPasswordPage.SendOtpBtn);
            BaseStep.wait.waitTillPageLoad();

            try
            {
               BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sessionexpirytimer, 10);
                Assert.IsTrue(ForgotPasswordPage.SessionExpiryTimer.Displayed);
            }
            catch
            {
                WebdriverSession.refreshPage();
                BaseStep.wait.waitTillPageLoad();
                BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sendotpbtn, 60);
                BaseStep.sendkeys(ForgotPasswordPage.SearchText, phonenumber);
                BaseStep.click(ForgotPasswordPage.SendOtpBtn);
                BaseStep.wait.waitTillPageLoad();

            }

            BaseStep.wait.waitForElementInvisibilityLongWait(ForgotPasswordPage.sessionexpirytimer, 6000000);
            BaseStep.wait.waitForElementVisibilityLongWait(ForgotPasswordPage.sessionexpirymsg, 60);
            Assert.IsTrue(ForgotPasswordPage.SessionExpiryMsg.Displayed);
            Validate.takestepFullScreenShot("Session Expired", Status.Info);
        }

    }
}
