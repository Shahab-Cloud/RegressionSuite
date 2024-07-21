using AventStack.ExtentReports;
using JM;
using JM.MainUtils;
using JM.Test.Pages;
using JMAutomation;
using JMAutomation.MainUtils;
using JMAutomation.Test.Pages;
using OpenQA.Selenium;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace JMAutomation.Test.Steps
{
    public class CreditReportPageSteps
    {
        public static void verifyCreditReport(string IdNumber)
        {
            BaseStep.wait.waitTillPageLoad();
            validateAccountSummary(IdNumber);
            judgementsandlegalaction();
            debtcounselling();
            updateddate();
        }

        private static void validateAccountSummary(string IdNumber)
        {
            CreditReportPage CreditReportPage = new CreditReportPage();
            DashboardPageSteps.isdashboardPageDispalyed();
            BaseStep.click(CreditReportPage.CreditReportBtn);
            BaseStep.wait.waitTillPageLoad();
            BaseStep.wait.waitForElementVisibilityLongWait(CreditReportPage.totalmonthlyincome, 60);

            //Total Monthly Income

            string expectedTMI = GenericUtils.splitString(getTotalIncome_Balance_Repayment("Monthly Income"), " ", 1);
            string ETMI = null;
            if (expectedTMI.Contains(","))
            {
                ETMI = expectedTMI.Replace(",", "");
            }
            else
            {
                ETMI = expectedTMI;
            }
            string actualTMI = GenericUtils.splitString(DBCreditCoach.getTotalIncome(IdNumber), ".", 0);
            Validate.assertEquals(ETMI, actualTMI, "TotalMonthlyIncome is mismatched with DB", false);
            Report.childlog.Log(Status.Info, "Expected TotalMonthlyIncome is eqaul to actual i.e " + expectedTMI);

            //Total Current Balance
            string expectedTCB = GenericUtils.splitString(getTotalIncome_Balance_Repayment("current balance"), " ", 1);
            string ETCB = null;
            if (expectedTCB.Contains(","))
            {
                ETCB = expectedTCB.Replace(",", "");
            }
            else
            {
                ETCB = expectedTCB;
            }
            string actualTCB = GenericUtils.splitString(DBCreditCoach.getTotalCurrentBalance(IdNumber), ".", 0);
            string outputString = actualTCB.Replace(",", "");
            Validate.assertEquals(ETCB, actualTCB, "TotalCurrentBalance is mismatched with DB", true);
            Report.childlog.Log(Status.Info, "Expected TotalCurrentBalance is eqaul to actual i.e " + expectedTCB);

            //Total Monthly Repayment            
            string expectedTMR = GenericUtils.splitString(getTotalIncome_Balance_Repayment("monthly repayment"), " ", 1);
            string ETMR = null;
            if (expectedTMR.Contains(","))
            {
                ETMR = expectedTMR.Replace(",", "");
            }
            else
            {
                ETMR = expectedTMR;
            }
            string actualTMR = GenericUtils.splitString(DBCreditCoach.getTotalMonthlyRepayment(IdNumber), ".", 0);
            Validate.assertEquals(ETMR, actualTMR, "TotalMonthlyRepayment is mismatched with DB", true);
            Report.childlog.Log(Status.Info, "Expected TotalMonthlyRepayment is eqaul to actual i.e " + expectedTMR);
            int i = 1;
            foreach (IWebElement loan in CreditReportPage.NoOfAccountSummaryOptions)
            {

                string loanText = BaseStep.getText.text(loan);
                Report.childlog.Log(Status.Info, "Loan Details are " + loanText);
                loan.Click();
                BaseStep.wait.genericWait(2000);
                Validate.takestepFullScreenShot("Loan Details No."+i, Status.Info);
                loan.Click();
                i++;
            }
        }

        private static void judgementsandlegalaction()
        {
            CreditReportPage CreditReportPage = new CreditReportPage();
            Report.childlog.Log(Status.Info, "judgementsandlegalaction");
            BaseStep.scrollToElement(CreditReportPage.JudgementsAndLegalAction);
            Validate.takestepFullScreenShot("judgementsandlegalaction", Status.Info);

        }
        private static void debtcounselling()
        {
            CreditReportPage CreditReportPage = new CreditReportPage();
            Report.childlog.Log(Status.Info, "debtcounselling");
            BaseStep.scrollToElement(CreditReportPage.DebtCounselling);
            Validate.takestepFullScreenShot("debtcounselling", Status.Info);

        }

        private static void updateddate()
        {
            CreditReportPage CreditReportPage = new CreditReportPage();
            Report.childlog.Log(Status.Info, "updateddate");
            BaseStep.scrollToElement(CreditReportPage.UpdatedDate);
            string UpdatedDate = BaseStep.getText.text(CreditReportPage.UpdatedDate);
            string[] splitString = UpdatedDate.Split(' ');
            string firstString = splitString[0];
            string secondString = string.Join(" ", splitString.Skip(1));
            string systemDate = GenericUtils.getCurrentDate("dd MMMM yyyy");
            Validate.assertEquals(systemDate, secondString, "Updated date is not matched", false);
            Report.childlog.Log(Status.Info, "Updated string i.e. "+ secondString+" is matched with system date");
        }

        private static string getTotalIncome_Balance_Repayment(string totaltype)
        {
            CreditReportPage CreditReportPage = new CreditReportPage();
            string total = null;
            if (totaltype.ToLower().Equals("monthly income"))
            {
                string TotalMonthlyIncome = BaseStep.getText.text(CreditReportPage.TotalMonthlyIncome);
                Report.childlog.Log(Status.Info, "Actual Total" + totaltype + " is " + TotalMonthlyIncome);
                total = TotalMonthlyIncome;
            }
            else if (totaltype.ToLower().Equals("current balance"))
            {
                string TotalCurrentBalance = BaseStep.getText.text(CreditReportPage.TotalCurrentBalance);
                Report.childlog.Log(Status.Info, "Actual Total" + totaltype + " is " + TotalCurrentBalance);
                total = TotalCurrentBalance;
            }
            else if (totaltype.ToLower().Equals("monthly repayment"))
            {
                string TotalMonthlyRepayment = BaseStep.getText.text(CreditReportPage.TotalMonthlyRepayment);
                Report.childlog.Log(Status.Info, "Actual Total" + totaltype + " is " + TotalMonthlyRepayment);
                total = TotalMonthlyRepayment;
            }
            return total;
        }
    }
}
