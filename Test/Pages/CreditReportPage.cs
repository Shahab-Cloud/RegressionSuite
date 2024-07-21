using JM.MainUtils;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMAutomation.Test.Pages
{
    public class CreditReportPage:WebdriverSession
    {
        public By creditreportbtn = By.XPath("//a[text()='Credit Report']");
        public IWebElement CreditReportBtn => Driver.FindElement(creditreportbtn);

        public By totalmonthlyincome = By.XPath("/html/body/app-root/app-layout/app-account/section[1]/div/div[1]/div[2]/div/div[1]/div/h3[2]");
        public IWebElement TotalMonthlyIncome => Driver.FindElement(totalmonthlyincome);

        public By totalcurrentbalance = By.XPath("/html/body/app-root/app-layout/app-account/section[1]/div/div[1]/div[2]/div/div[2]/div/h3[2]");
        public IWebElement TotalCurrentBalance => Driver.FindElement(totalcurrentbalance);

        public By totalmonthlyrepayment = By.XPath("/html/body/app-root/app-layout/app-account/section[1]/div/div[1]/div[2]/div/div[3]/div/h3[2]");
        public IWebElement TotalMonthlyRepayment => Driver.FindElement(totalmonthlyrepayment);

        public By accountsummaryoptions = By.XPath("//*[@id=\"accordion_summary\"]/div");
        public IList<IWebElement> NoOfAccountSummaryOptions => Driver.FindElements(accountsummaryoptions);

        public By judgementsandlegalaction = By.XPath("//h2[text()='Judgements & Legal Action']");
        public IWebElement JudgementsAndLegalAction => Driver.FindElement(judgementsandlegalaction);

        public By debtcounselling = By.XPath("//h2[text()='Debt Counselling']");
        public IWebElement DebtCounselling => Driver.FindElement(debtcounselling);

        public By updateddate = By.XPath("/html/body/app-root/app-layout/app-account/section[5]/div/div/div/h5");
        public IWebElement UpdatedDate => Driver.FindElement(updateddate);


        public static IWebElement selectAccountSummaryOption(int i) { 
        
            return Driver.FindElement(By.XPath("//*[@id='heading"+i+"']/h5/button"));

        }


    }
}
