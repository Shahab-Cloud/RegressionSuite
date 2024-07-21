using AventStack.ExtentReports;
using JM.MainUtils;
using JM.Test.Pages;
using JMAutomation.MainUtils;
using OpenQA.Selenium;

namespace JMAutomation
{
    public class DashboardPageSteps : WebdriverSession
    {

        public static void isdashboardPageDispalyed()
        {

            DashboardPage dashboardPage = new DashboardPage();
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(dashboardPage.homepage, 60);
            BaseStep.wait.genericWait(2000);
            Assert.IsTrue(dashboardPage.isHomePageDisplayed());
        }

        public static void waitTillSalaryPopUpIsDisplayed()
        {
            DashboardPage dashboardPage = new DashboardPage();
            BaseStep.wait.genericWait(2000);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(dashboardPage.salarypopup, 60);
            string salaryPopUpText = BaseStep.getText.text(dashboardPage.SalaryPopUp);
            Report.childlog.Log(Status.Info, "Salary Pop Text - " + salaryPopUpText);            
            //Validate.takestepFullScreenShot("Take Home Salary Entered ", Status.Pass);
            foreach (IWebElement ele in dashboardPage.TakeHomeSalary)
            {
                try
                {
                    BaseStep.clearAndSendkeys(ele, "0");
                }
                catch (Exception e)
                {
                    Report.childlog.Log(Status.Info, "Error - " + e);
                }
            }
            Validate.takestepFullScreenShot("Take Home Salary Entered ", Status.Pass);

            foreach (IWebElement ele in dashboardPage.SalaryPopUpCntnBtn)
            {
                try
                {
                    BaseStep.click(ele);
                }
                catch (Exception e)
                {
                    Report.childlog.Log(Status.Info, "Error - " + e);
                }
            }
           
        }

        public static void enterSalaryAndRepaymentValueAfterLogin(String IdNumber, String salary)
        {
            DashboardPage dashboardPage = new DashboardPage();
            isdashboardPageDispalyed();
            BaseStep.wait.waitTillPageLoad();
            try
            {
                BaseStep.wait.waitForElementVisibilityLongWait(dashboardPage.salarypopup, 10);

                /*
                 * As per Dev team Repayment value can only be calculated from bureau end
                 */
                //  enterRepaymentValue(IdNumber, expectedRepaymentValue);

                string salaryPopUpText = BaseStep.getText.text(dashboardPage.SalaryPopUp);
                Report.childlog.Log(Status.Info, "Salary Pop Text - " + salaryPopUpText);
                foreach (IWebElement ele in dashboardPage.TakeHomeSalary)
                {
                    try
                    {
                        BaseStep.sendkeys(ele, "0");
                    }
                    catch (Exception e)
                    {
                        Report.childlog.Log(Status.Info, "Error - " + e);
                    }
                }
                Validate.takestepFullScreenShot("Take Home Salary Entered ", Status.Pass);
                foreach (IWebElement ele in dashboardPage.SalaryPopUpCntnBtn)
                {
                    try
                    {
                        BaseStep.click(ele);
                    }
                    catch (Exception e)
                    {
                        Report.childlog.Log(Status.Info, "Error - " + e);
                    }
                }
                BaseStep.wait.waitTillPageLoad();

            }
            catch (Exception e)
            {
                Report.childlog.Log(Status.Info, "No Salary Popup is visible due to " + e);
                BaseStep.wait.waitForElementVisibilityLongWait(dashboardPage.homepage, 60);
                /*
                 * As per Dev team Repayment value can only be calculated from bureau end
                 */

                // enterRepaymentValue(IdNumber,expectedRepaymentValue);

                BaseStep.wait.genericWait(5000);
                BaseStep.click(dashboardPage.ProfileIcon);
                BaseStep.wait.waitForElementVisibility(dashboardPage.profileoption);
                BaseStep.click(dashboardPage.ProfileOption);

                BaseStep.wait.waitTillPageLoad();
                BaseStep.wait.waitForElementVisibilityLongWait(dashboardPage.profilecurrencyfield, 60);
                BaseStep.clearAndSendkeys(dashboardPage.ProfileCurrencyField, salary);
                BaseStep.scrollToElement(dashboardPage.ProfileUpdateBtn);

                if (dashboardPage.ProfileUpdateBtn.Enabled)
                {
                    BaseStep.click(dashboardPage.ProfileUpdateBtn);
                    BaseStep.wait.waitTillPageLoad();
                    BaseStep.wait.waitForElementVisibilityLongWait(dashboardPage.profileupdatemsg, 60);
                    BaseStep.wait.genericWait(5000);
                }
                else
                {
                    BaseStep.clearAndSendkeys(dashboardPage.ProfileCurrencyField, salary);
                    BaseStep.click(dashboardPage.ProfileUpdateBtn);
                    BaseStep.wait.waitTillPageLoad();
                    BaseStep.wait.waitForElementVisibilityLongWait(dashboardPage.profileupdatemsg, 60);
                    BaseStep.wait.genericWait(5000);
                }

                Validate.takestepFullScreenShot("Take Home Salary Entered is " + salary, Status.Pass);

                BaseStep.click(dashboardPage.DashboardIcon);
                BaseStep.wait.waitTillPageLoad();
            }
        }

        private static void enterRepaymentValue(String IdNumber)
        {
            DashboardPage dashboardPage = new DashboardPage();
            string actualRepaymentValue = BaseStep.getText.text(dashboardPage.RepaymentsCircle);
            string actualRV = GenericUtils.splitString(actualRepaymentValue, " ", 1);
            /*
             *  As per Dev team Repayment value can only be calculated from bureau end
             * 
             * if (actualRV != expectedRepaymentValue) {
                DBCreditCoach.updateRepaymentFromDB(IdNumber, expectedRepaymentValue);
                }   */

        }

        public static void checkDebt(string expectedIncomeValue)
        {
            DashboardPage dashboardPage = new DashboardPage();
            string expectedDebtToIncomeRatioText;
            BaseStep.scrollToElement(dashboardPage.RepaymentsCircle);
            BaseStep.wait.genericWait(3000);
            BaseStep.wait.waitTillPageLoad();
            string actualRepaymentValue = BaseStep.getText.text(dashboardPage.RepaymentsCircle);
            string actualRV = GenericUtils.splitString(actualRepaymentValue, " ", 1);
            string aRV;
            if (actualRV.Contains(","))
            {
                aRV = actualRV.Replace(",", "");
            }
            else
            {
                aRV = actualRV;
            }

            Report.childlog.Log(Status.Info, "RepaymentValue value is equal to " + aRV);

            BaseStep.scrollToElement(dashboardPage.IncomeCircle);
            string actualIncomeValue = BaseStep.getText.text(dashboardPage.IncomeCircle);
            string actualIV = GenericUtils.splitString(actualIncomeValue, " ", 1);
            Validate.assertEquals(expectedIncomeValue, actualIV, "Income Value is not matched", true);
            Report.childlog.Log(Status.Info, "actualIncomeValue value is equal to expectedIncomeValue");

            int ERV = int.Parse(aRV);
            int EIV = int.Parse(expectedIncomeValue);


            decimal per = ((decimal)ERV / (decimal)EIV) * 100;
            decimal debtPer = Math.Round(per);
            string DebtToIncomeRatioColor = null;



            if (debtPer <= 20)
            {
                string DebtToIncomeRatio = BaseStep.getText.text(dashboardPage.DebtToIncomeRatio);
                int DTIR = int.Parse(DebtToIncomeRatio.Replace("%", ""));
                Assert.IsTrue(DTIR <= 20);
                expectedDebtToIncomeRatioText = "Very High Chance";
                string actualDebtToIncomeRatioText = BaseStep.getText.text(dashboardPage.DebtToIncomeRatioText);
                string color = dashboardPage.DebtToIncomeRatioText.GetCssValue("color");
                if (color.Contains("(48, 149, 95, 1)"))
                {
                    DebtToIncomeRatioColor = "Green";
                }
                Assert.IsTrue(DebtToIncomeRatioColor == "Green");
                Validate.assertEquals(expectedDebtToIncomeRatioText, actualDebtToIncomeRatioText, "Debt-To-Income Ratio Text is not matched", false);
                Report.childlog.Log(Status.Pass, "User have DebtToIncomeRatio is below 20% and having " + actualDebtToIncomeRatioText + " and Text color is " + DebtToIncomeRatioColor);

            }
            else if (debtPer >= 21 && debtPer <= 40)
            {
                string DebtToIncomeRatio = BaseStep.getText.text(dashboardPage.DebtToIncomeRatio);
                int DTIR = int.Parse(DebtToIncomeRatio.Replace("%", ""));
                Assert.IsTrue(DTIR >= 21 && DTIR <= 40);
                expectedDebtToIncomeRatioText = "High Chance";
                string actualDebtToIncomeRatioText = BaseStep.getText.text(dashboardPage.DebtToIncomeRatioText);
                string color = dashboardPage.DebtToIncomeRatioText.GetCssValue("color");
                if (color.Contains("(165, 193, 94, 1)"))
                {
                    DebtToIncomeRatioColor = "Light Green";
                }
                Validate.assertEquals(expectedDebtToIncomeRatioText, actualDebtToIncomeRatioText, "Debt-To-Income Ratio Text is not matched", false);
                Report.childlog.Log(Status.Pass, "User have DebtToIncomeRatio is above 20% and below 40% and having " + actualDebtToIncomeRatioText + " and Text color is " + DebtToIncomeRatioColor);
            }
            else if (debtPer >= 41 && debtPer <= 60)
            {
                string DebtToIncomeRatio = BaseStep.getText.text(dashboardPage.DebtToIncomeRatio);
                int DTIR = int.Parse(DebtToIncomeRatio.Replace("%", ""));
                Assert.IsTrue(DTIR >= 41 && DTIR <= 60);
                expectedDebtToIncomeRatioText = "Moderate Chance";
                string actualDebtToIncomeRatioText = BaseStep.getText.text(dashboardPage.DebtToIncomeRatioText);
                string color = dashboardPage.DebtToIncomeRatioText.GetCssValue("color");
                if (color.Contains("(248, 217, 38, 1)"))
                {
                    DebtToIncomeRatioColor = "Yellow";
                }
                Validate.assertEquals(expectedDebtToIncomeRatioText, actualDebtToIncomeRatioText, "Debt-To-Income Ratio Text is not matched", false);
                Report.childlog.Log(Status.Pass, "User have DebtToIncomeRatio is above 40% and below 60% and having " + actualDebtToIncomeRatioText + " and Text color is " + DebtToIncomeRatioColor);
            }
            else if (debtPer >= 61 && debtPer <= 80)
            {
                string DebtToIncomeRatio = BaseStep.getText.text(dashboardPage.DebtToIncomeRatio);
                int DTIR = int.Parse(DebtToIncomeRatio.Replace("%", ""));
                Assert.IsTrue(DTIR >= 61 && DTIR <= 80);
                expectedDebtToIncomeRatioText = "Low Chance";
                string actualDebtToIncomeRatioText = BaseStep.getText.text(dashboardPage.DebtToIncomeRatioText);
                string color = dashboardPage.DebtToIncomeRatioText.GetCssValue("color");
                if (color.Contains("(241, 144, 68, 1)"))
                {
                    DebtToIncomeRatioColor = "Orange";
                }
                Validate.assertEquals(expectedDebtToIncomeRatioText, actualDebtToIncomeRatioText, "Debt-To-Income Ratio Text is not matched", false);
                Report.childlog.Log(Status.Pass, "User have DebtToIncomeRatio is above 60% and below 80% and having " + actualDebtToIncomeRatioText + " and Text color is " + DebtToIncomeRatioColor);
            }
            else if (debtPer >= 81)
            {
                string DebtToIncomeRatio = BaseStep.getText.text(dashboardPage.DebtToIncomeRatio);
                int DTIR = int.Parse(DebtToIncomeRatio.Replace("%", ""));
                Assert.IsTrue(DTIR >= 81);
                expectedDebtToIncomeRatioText = "Very Low Chance";
                string actualDebtToIncomeRatioText = BaseStep.getText.text(dashboardPage.DebtToIncomeRatioText);
                string color = dashboardPage.DebtToIncomeRatioText.GetCssValue("color");
                if (color.Contains("(246, 52, 52, 1)"))
                {
                    DebtToIncomeRatioColor = "Red";
                }
                Validate.assertEquals(expectedDebtToIncomeRatioText, actualDebtToIncomeRatioText, "Debt-To-Income Ratio Text is not matched", false);
                Report.childlog.Log(Status.Pass, "User have DebtToIncomeRatio is above 80% and having " + actualDebtToIncomeRatioText + " and Text color is " + DebtToIncomeRatioColor);
            }


        }

        public static void checkNoOfOpenAccounts()
        {
            DashboardPage dashboardPage = new DashboardPage();

            BaseStep.wait.waitTillPageLoad();
            isdashboardPageDispalyed();
            BaseStep.wait.waitForElementVisibilityLongWait(dashboardPage.noofopenaccountsforhome, 60);
            BaseStep.scrollToElement(dashboardPage.NoOfOpenAccountsForHome);
            BaseStep.wait.genericWait(2000);

            // Home Account

            string NoOfOpenAccountsForHome = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForHome);
            Report.childlog.Log(Status.Info, "No. Of Open Accounts for Home are " + NoOfOpenAccountsForHome);


            // Car Account

            string NoOfOpenAccountsForCar = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForHome);
            Report.childlog.Log(Status.Info, "No. Of Open Accounts for Car are " + NoOfOpenAccountsForCar);

            // Clothing Account

            string NoOfOpenAccountsForClothing = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForClothing);
            Report.childlog.Log(Status.Info, "No. Of Open Accounts for Clothing are " + NoOfOpenAccountsForClothing);

            // Credit Card

            string NoOfOpenAccountsForCreditCard = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForCreditCard);
            Report.childlog.Log(Status.Info, "No. Of Open Accounts for CreditCard are " + NoOfOpenAccountsForCreditCard);


            // Loans

            string NoOfOpenAccountsForLoans = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForLoans);
            Report.childlog.Log(Status.Info, "No. Of Open Accounts for Loans are " + NoOfOpenAccountsForLoans);



        }
        private static int TotalOpeningAndCurrentBalanceAsPerLoanType(string Idnumber, string loanType, string sumtype)
        {

            int sumForOpenStatus = DBCreditCoach.sumOfAccountFromDB(Idnumber, loanType, "Open", sumtype);
            int sumForTermsExtended = DBCreditCoach.sumOfAccountFromDB(Idnumber, loanType, "Terms Extended", sumtype);
            int sumForFacilityRevoked = DBCreditCoach.sumOfAccountFromDB(Idnumber, loanType, "Facility Revoked", sumtype);
            int sumForPaidUp = DBCreditCoach.sumOfAccountFromDB(Idnumber, loanType, "Paid Up", sumtype);
            int sumForHandedOverStatus = DBCreditCoach.sumOfAccountFromDB(Idnumber, loanType, "Handed Over", sumtype);
            int sumForSettlementOfAdverseArrears = DBCreditCoach.sumOfAccountFromDB(Idnumber, loanType, "Settlement of Adverse Arrears", sumtype);
            int sumForPrescriptionInterruptedIndicator = DBCreditCoach.sumOfAccountFromDB(Idnumber, loanType, "Prescription Interrupted Indicator", sumtype);


            int TotalOpeningBalance = sumForOpenStatus + sumForTermsExtended + sumForFacilityRevoked +
                sumForPaidUp + sumForSettlementOfAdverseArrears + sumForPrescriptionInterruptedIndicator +
                sumForHandedOverStatus;

            return TotalOpeningBalance;
        }

        public static void yourDebtIsMadeUpOf(string Idnumber)
        {

            DashboardPage dashboardPage = new DashboardPage();

            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(dashboardPage.noofopenaccountsforhome, 60);
            BaseStep.wait.genericWait(2000);

            // Home 
            string NoOfOpenAccountsForHome = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForHome);

            int HomeLoanNos = int.Parse(NoOfOpenAccountsForHome);
            if (HomeLoanNos >= 1)
            {
                int TotalBondCurrentBalanceForHome = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Home Loan", "Current_Balance");
                string totalcurrentbalanceforHome = BaseStep.getText.text(dashboardPage.TotalCurrentBalance);
                string TotalCurrentBalanceForHome = GenericUtils.splitString(totalcurrentbalanceforHome, " ", 1);
                int TCBH;
                if (TotalCurrentBalanceForHome.Contains(","))
                {
                    TCBH = int.Parse(TotalCurrentBalanceForHome.Replace(",", ""));
                }
                else
                {
                    TCBH = int.Parse(TotalCurrentBalanceForHome);
                }

                decimal Homeper = ((decimal)TotalBondCurrentBalanceForHome / (decimal)TCBH) * 100;
                string YourDebtPerForHome = Math.Round(Homeper).ToString() + "%";
                string YourDeptIsMadeUpOfHome = BaseStep.getText.text(dashboardPage.YourDeptIsMadeUpOfHome);
                Validate.assertEquals(YourDeptIsMadeUpOfHome, YourDebtPerForHome, " YourDeptIsMadeUpOfHome is not matched ", false);
                Report.childlog.Log(Status.Info, "Expected YourDeptIsMadeUpOfHome is equal to Actual i.e. " + YourDeptIsMadeUpOfHome);
            }
            // Car
            string NoOfOpenAccountsForCar = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForCar);

            int CarLoanNos = int.Parse(NoOfOpenAccountsForCar);
            if (CarLoanNos >= 1)
            {
                int TotalBondCurrentBalanceForCar = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Vehicle Loan", "Current_Balance");
                string totalcurrentbalanceforCar = BaseStep.getText.text(dashboardPage.TotalCurrentBalance);
                string TotalCurrentBalanceForCar = GenericUtils.splitString(totalcurrentbalanceforCar, " ", 1);
                int TCBCar;
                if (TotalCurrentBalanceForCar.Contains(","))
                {
                    TCBCar = int.Parse(TotalCurrentBalanceForCar.Replace(",", ""));
                }
                else
                {
                    TCBCar = int.Parse(TotalCurrentBalanceForCar);
                }

                decimal Carper = ((decimal)TotalBondCurrentBalanceForCar / (decimal)TCBCar) * 100;
                string YourDebtPerForCar = Math.Round(Carper).ToString() + "%";
                string YourDeptIsMadeUpOfCar = BaseStep.getText.text(dashboardPage.YourDeptIsMadeUpOfCar);
                Validate.assertEquals(YourDeptIsMadeUpOfCar, YourDebtPerForCar, " YourDeptIsMadeUpOfCar is not matched ", false);
                Report.childlog.Log(Status.Info, "Expected YourDeptIsMadeUpOfCar is equal to Actual i.e. " + YourDeptIsMadeUpOfCar);
            }
            // Clothing
            string NoOfOpenAccountsForClothing = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForClothing);

            int CLoanNos = int.Parse(NoOfOpenAccountsForClothing);
            if (CLoanNos >= 1)
            {
                int TotalBondCurrentBalanceForClothing = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Retail", "Current_Balance");
                string totalcurrentbalanceforclothing = BaseStep.getText.text(dashboardPage.TotalCurrentBalance);
                string TotalCurrentBalanceForClothing = GenericUtils.splitString(totalcurrentbalanceforclothing, " ", 1);
                int TCBC;
                if (TotalCurrentBalanceForClothing.Contains(","))
                {
                    TCBC = int.Parse(TotalCurrentBalanceForClothing.Replace(",", ""));
                }
                else
                {
                    TCBC = int.Parse(TotalCurrentBalanceForClothing);
                }

                decimal Cper = ((decimal)TotalBondCurrentBalanceForClothing / (decimal)TCBC) * 100;
                string YourDebtPerForClothing = Math.Round(Cper).ToString() + "%";
                string YourDeptIsMadeUpOfClothing = BaseStep.getText.text(dashboardPage.YourDeptIsMadeUpOfClothing);
                Validate.assertEquals(YourDeptIsMadeUpOfClothing, YourDebtPerForClothing, " YourDeptIsMadeUpOfClothing is not matched ", false);
                Report.childlog.Log(Status.Info, "Expected YourDeptIsMadeUpOfClothing is equal to Actual i.e. " + YourDeptIsMadeUpOfClothing);

            }


            // Credit Card
            string NoOfOpenAccountsForCreditCard = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForCreditCard);

            int CCLoanNos = int.Parse(NoOfOpenAccountsForCreditCard);
            if (CCLoanNos >= 1)
            {
                int TotalBondCurrentBalanceForCC = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Credit Card", "Current_Balance");
                string totalcurrentbalanceforCC = BaseStep.getText.text(dashboardPage.TotalCurrentBalance);
                string TotalCurrentBalance = GenericUtils.splitString(totalcurrentbalanceforCC, " ", 1);
                int TCBCC;
                if (TotalCurrentBalance.Contains(","))
                {
                    TCBCC = int.Parse(TotalCurrentBalance.Replace(",", ""));
                }
                else
                {
                    TCBCC = int.Parse(TotalCurrentBalance);
                }

                decimal CCper = ((decimal)TotalBondCurrentBalanceForCC / (decimal)TCBCC) * 100;
                string YourDebtPerForCC = Math.Round(CCper).ToString() + "%";
                string YourDeptIsMadeUpOfCreditCard = BaseStep.getText.text(dashboardPage.YourDeptIsMadeUpOfCreditCard);
                Validate.assertEquals(YourDeptIsMadeUpOfCreditCard, YourDebtPerForCC, " YourDeptIsMadeUpOfCreditCard is not matched ", false);
                Report.childlog.Log(Status.Info, "Expected YourDeptIsMadeUpOfCreditCard is equal to Actual i.e. " + YourDeptIsMadeUpOfCreditCard);

            }

            //Loans
            string NoOfOpenAccountsForLoans = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForLoans);

            int LoanNos = int.Parse(NoOfOpenAccountsForLoans);
            if (LoanNos >= 1)
            {
                int TotalBondCurrentBalanceForLoans = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Personal Loan", "Current_Balance");
                string totalcurrentbalanceforLoans = BaseStep.getText.text(dashboardPage.TotalCurrentBalance);
                string TotalCurrentBalanceForLoans = GenericUtils.splitString(totalcurrentbalanceforLoans, " ", 1);
                int TCBL;
                if (TotalCurrentBalanceForLoans.Contains(","))
                {
                    TCBL = int.Parse(TotalCurrentBalanceForLoans.Replace(",", ""));
                }
                else
                {
                    TCBL = int.Parse(TotalCurrentBalanceForLoans);
                }
                decimal Loanper = ((decimal)TotalBondCurrentBalanceForLoans / (decimal)TCBL) * 100;
                string YourDebtPerForLoan = Math.Round(Loanper).ToString() + "%";
                string YourDeptIsMadeUpOfLoans = BaseStep.getText.text(dashboardPage.YourDeptIsMadeUpOfLoans);
                Validate.assertEquals(YourDeptIsMadeUpOfLoans, YourDebtPerForLoan, " YourDeptIsMadeUpOfLoan is not matched ", false);
                Report.childlog.Log(Status.Info, "Expected YourDeptIsMadeUpOfLoans is equal to Actual i.e. " + YourDeptIsMadeUpOfLoans);



            }



        }

        public static void howMuchHaveYouPaidOff(string Idnumber)
        {
            DashboardPage dashboardPage = new DashboardPage();

            // Home 
            string NoOfOpenAccountsForHome = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForHome);

            int HomeLoanNos = int.Parse(NoOfOpenAccountsForHome);
            if (HomeLoanNos >= 1)
            {
                int TotalBondCurrentBalanceForCC = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Home Loan", "Current_Balance");
                int TotalBondOpeningBalanceForCC = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Home Loan", "Opening_Balance_Credit_Limit");
                decimal Homeper = 100 - ((decimal)TotalBondCurrentBalanceForCC / (decimal)TotalBondOpeningBalanceForCC) * 100;
                string YourDebtPerForHome = Math.Round(Homeper).ToString() + "%";
                string HowMuchHaveYouPaidOffHome = BaseStep.getText.text(dashboardPage.HowMuchHaveYouPaidOffHome);
                Validate.assertEquals(HowMuchHaveYouPaidOffHome, YourDebtPerForHome, " HowMuchHaveYouPaidOffHome is not matched ", false);
                Report.childlog.Log(Status.Info, "Expected HowMuchHaveYouPaidOffHome is equal to Actual i.e. " + HowMuchHaveYouPaidOffHome);
            }
            // Car
            string NoOfOpenAccountsForCar = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForCar);

            int CarLoanNos = int.Parse(NoOfOpenAccountsForCar);
            if (CarLoanNos >= 1)
            {
                int TotalBondCurrentBalanceForCC = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Vehicle Loan", "Current_Balance");
                int TotalBondOpeningBalanceForCC = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Vehicle Loan", "Opening_Balance_Credit_Limit");
                decimal Carper = 100 - ((decimal)TotalBondCurrentBalanceForCC / (decimal)TotalBondOpeningBalanceForCC) * 100;
                string YourDebtPerForCar = Math.Round(Carper).ToString() + "%";
                string HowMuchHaveYouPaidOffCar = BaseStep.getText.text(dashboardPage.HowMuchHaveYouPaidOffCar);
                Validate.assertEquals(HowMuchHaveYouPaidOffCar, YourDebtPerForCar, " HowMuchHaveYouPaidOffCar is not matched ", false);
                Report.childlog.Log(Status.Info, "Expected HowMuchHaveYouPaidOffCar is equal to Actual i.e. " + HowMuchHaveYouPaidOffCar);
            }
            // Clothing
            string NoOfOpenAccountsForClothing = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForClothing);

            int CLoanNos = int.Parse(NoOfOpenAccountsForClothing);
            if (CLoanNos >= 1)
            {
                int TotalBondCurrentBalanceForCC = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Retail", "Current_Balance");
                int TotalBondOpeningBalanceForCC = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Retail", "Opening_Balance_Credit_Limit");
                decimal Cper = 100 - ((decimal)TotalBondCurrentBalanceForCC / (decimal)TotalBondOpeningBalanceForCC) * 100;
                string YourDebtPerForClothing = Math.Round(Cper).ToString() + "%";
                string HowMuchHaveYouPaidOffClothing = BaseStep.getText.text(dashboardPage.HowMuchHaveYouPaidOffClothing);
                Validate.assertEquals(HowMuchHaveYouPaidOffClothing, YourDebtPerForClothing, " HowMuchHaveYouPaidOffClothing is not matched ", false);
                Report.childlog.Log(Status.Info, "Expected HowMuchHaveYouPaidOffClothing is equal to Actual i.e. " + HowMuchHaveYouPaidOffClothing);

            }


            // Credit Card
            string NoOfOpenAccountsForCreditCard = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForCreditCard);

            int CCLoanNos = int.Parse(NoOfOpenAccountsForCreditCard);
            if (CCLoanNos >= 1)
            {
                int TotalBondCurrentBalanceForCC = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Credit Card", "Current_Balance");
                int TotalBondOpeningBalanceForCC = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Credit Card", "Opening_Balance_Credit_Limit");
                decimal CCper = 100 - ((decimal)TotalBondCurrentBalanceForCC / (decimal)TotalBondOpeningBalanceForCC) * 100;
                string YourDebtPerForCC = Math.Round(CCper).ToString() + "%";
                string HowMuchHaveYouPaidOffCreditCard = BaseStep.getText.text(dashboardPage.HowMuchHaveYouPaidOffCreditCard);
                Validate.assertEquals(HowMuchHaveYouPaidOffCreditCard, YourDebtPerForCC, " HowMuchHaveYouPaidOffCreditCard is not matched ", false);
                Report.childlog.Log(Status.Info, "Expected HowMuchHaveYouPaidOffCreditCard is equal to Actual i.e. " + HowMuchHaveYouPaidOffCreditCard);

            }

            //Loans
            string NoOfOpenAccountsForLoans = BaseStep.getText.text(dashboardPage.NoOfOpenAccountsForLoans);

            int LoanNos = int.Parse(NoOfOpenAccountsForLoans);
            if (LoanNos >= 1)
            {
                int TotalBondCurrentBalanceForCC = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Personal Loan", "Current_Balance");
                int TotalBondOpeningBalanceForCC = TotalOpeningAndCurrentBalanceAsPerLoanType(Idnumber, "Personal Loan", "Opening_Balance_Credit_Limit");
                decimal Loanper = 100 - ((decimal)TotalBondCurrentBalanceForCC / (decimal)TotalBondOpeningBalanceForCC) * 100;
                string HowMuchPaidPerForLoan = Math.Round(Loanper).ToString() + "%";
                string HowMuchHaveYouPaidOffLoans = BaseStep.getText.text(dashboardPage.HowMuchHaveYouPaidOffLoans);
                Validate.assertEquals(HowMuchHaveYouPaidOffLoans, HowMuchPaidPerForLoan, " HowMuchHaveYouPaidOffLoans is not matched ", false);
                Report.childlog.Log(Status.Info, "Expected HowMuchHaveYouPaidOffLoans is equal to Actual i.e. " + HowMuchHaveYouPaidOffLoans);



            }
        }

        public static void yourCreditScore(string IdNumber, string CreditScore)
        {
            DashboardPage dashboardPage = new DashboardPage();
            isdashboardPageDispalyed();

            //Credit Score
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(dashboardPage.creditscore, 10);
            
            string expectedCreditScore = DBCreditCoach.getCreditScoreFromDB(IdNumber);

            BaseStep.wait.genericWait(2000);
            string actualCreditScoreAfter = BaseStep.getText.text(dashboardPage.CreditScore);


            Validate.assertEquals(expectedCreditScore, actualCreditScoreAfter, "expectedCreditScore is not equal to Actual", false);
            Report.childlog.Log(Status.Info, "actualCreditScore i.e " + actualCreditScoreAfter + " is eqaul to expectedCreditScore i.e." + expectedCreditScore);

            // Credit Score Status

            int CS = int.Parse(actualCreditScoreAfter);
            if (CS > 0 && CS <= 2)
            {

                string expectedCreditScoreStatus = "no info";
                string actualCreditScoreStatus = BaseStep.getText.text(dashboardPage.CreditScoreStatus);
                Validate.assertEquals(expectedCreditScoreStatus.ToLower(), actualCreditScoreStatus.ToLower(), "expectedCreditScoreStatus is not equal to Actual", false);
                Report.childlog.Log(Status.Info, "actualCreditScoreStatus i.e " + actualCreditScoreStatus + " is eqaul to expectedCreditScoreStatus i.e." + expectedCreditScoreStatus);
            }
            else if (CS > 2 && CS <= 600)
            {
                string expectedCreditScoreStatus = "not good";
                string actualCreditScoreStatus = BaseStep.getText.text(dashboardPage.CreditScoreStatus);
                Validate.assertEquals(expectedCreditScoreStatus.ToLower(), actualCreditScoreStatus.ToLower(), "expectedCreditScoreStatus is not equal to Actual", false);
                Report.childlog.Log(Status.Info, "actualCreditScoreStatus i.e " + actualCreditScoreStatus + " is eqaul to expectedCreditScoreStatus i.e." + expectedCreditScoreStatus);

            }
            else if (CS > 600 && CS <= 800)
            {
                string expectedCreditScoreStatus = "needs work";
                string actualCreditScoreStatus = BaseStep.getText.text(dashboardPage.CreditScoreStatus);
                Validate.assertEquals(expectedCreditScoreStatus.ToLower(), actualCreditScoreStatus.ToLower(), "expectedCreditScoreStatus is not equal to Actual", false);
                Report.childlog.Log(Status.Info, "actualCreditScoreStatus i.e " + actualCreditScoreStatus + " is eqaul to expectedCreditScoreStatus i.e." + expectedCreditScoreStatus);

            }
            else if (CS > 800 && CS <= 850)
            {
                string expectedCreditScoreStatus = "okay";
                string actualCreditScoreStatus = BaseStep.getText.text(dashboardPage.CreditScoreStatus);
                Validate.assertEquals(expectedCreditScoreStatus.ToLower(), actualCreditScoreStatus.ToLower(), "expectedCreditScoreStatus is not equal to Actual", false);
                Report.childlog.Log(Status.Info, "actualCreditScoreStatus i.e " + actualCreditScoreStatus + " is eqaul to expectedCreditScoreStatus i.e." + expectedCreditScoreStatus);

            }
            else if (CS > 851 && CS <= 900)
            {
                string expectedCreditScoreStatus = "good";
                string actualCreditScoreStatus = BaseStep.getText.text(dashboardPage.CreditScoreStatus);
                Validate.assertEquals(expectedCreditScoreStatus.ToLower(), actualCreditScoreStatus.ToLower(), "expectedCreditScoreStatus is not equal to Actual", false);
                Report.childlog.Log(Status.Info, "actualCreditScoreStatus i.e " + actualCreditScoreStatus + " is eqaul to expectedCreditScoreStatus i.e." + expectedCreditScoreStatus);

            }
            else if (CS > 901 && CS <= 1000)
            {
                string expectedCreditScoreStatus = "excellent";
                string actualCreditScoreStatus = BaseStep.getText.text(dashboardPage.CreditScoreStatus);
                Validate.assertEquals(expectedCreditScoreStatus.ToLower(), actualCreditScoreStatus.ToLower(), "expectedCreditScoreStatus is not equal to Actual", false);
                Report.childlog.Log(Status.Info, "actualCreditScoreStatus i.e " + actualCreditScoreStatus + " is eqaul to expectedCreditScoreStatus i.e." + expectedCreditScoreStatus);

            }


        }

        private static void userLogOut()
        {
            DashboardPage dashboardPage = new DashboardPage();
            loginPage loginPage = new loginPage();
            BaseStep.click(dashboardPage.ProfileIcon);
            BaseStep.wait.waitForElementVisibility(dashboardPage.profileoption);
            BaseStep.click(dashboardPage.ProfileOption);

            BaseStep.wait.waitForElementVisibilityLongWait(dashboardPage.profilecurrencyfield, 60);
            BaseStep.click(dashboardPage.LogOut);
            BaseStep.wait.waitForElementVisibilityLongWait(loginPage.loginidnumber, 60);
        }

    }
}
