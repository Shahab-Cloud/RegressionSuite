using JM.MainUtils;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JM.Test.Pages
{
    public class DashboardPage:WebdriverSession
    {
       

        public By homepage = By.XPath("/html/body/app-root/app-layout/app-header/header/nav/div/div/div/ul/li[1]/a");
        public IWebElement HomePageVisible => Driver.FindElement(homepage);

        public By salarypopup = By.XPath("//*[text()='Please enter your monthly after tax income']");
        public IWebElement SalaryPopUp => Driver.FindElement(salarypopup);

        public By takehomesalary = By.XPath("//*[@id=\"currency-number\"]");
        public IList<IWebElement> TakeHomeSalary => Driver.FindElements(takehomesalary);

        public By salarypopupcontinuebtn = By.XPath("/html/body/ngb-modal-window/div/div/form/div[2]/button");
        public IList<IWebElement> SalaryPopUpCntnBtn => Driver.FindElements(salarypopupcontinuebtn);

        // profileIcon

        public By profileicon = By.XPath("//*[@id=\"navbarDropdown\"]");
        public IWebElement ProfileIcon => Driver.FindElement(profileicon);

        public By profileoption = By.XPath("//*[@id=\"navbarDropdown\"]/following-sibling::ul/li[1]/a");
        public IWebElement ProfileOption => Driver.FindElement(profileoption);
       
        public By profilecurrencyfield = By.XPath("//*[@id=\"currency-number\"]");
        public IWebElement ProfileCurrencyField => Driver.FindElement(profilecurrencyfield);

        public By profileupdatebtn = By.XPath("//button[contains(text(),' Update profile ')]");
        public IWebElement ProfileUpdateBtn => Driver.FindElement(profileupdatebtn);

        public By profileupdatemsg= By.XPath("//*[text()=' Profile Updated Successfully. ']");
        public IWebElement ProfileUpdateMsg => Driver.FindElement(profileupdatemsg);

        public By logout = By.XPath("/html/body/app-root/app-layout/app-header/header/nav/div/div/div/ul/li[7]/ul/li[3]/a");
        public IWebElement LogOut => Driver.FindElement(logout);

        // Can You Afford Your Debt?

        public By dashboardicon = By.XPath("//a[text()='Dashboard']");
        public IWebElement DashboardIcon => Driver.FindElement(dashboardicon);

        public By repaymentscircle = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[7]/div/div[1]/div[1]/div/div[1]/div/h3");
        public IWebElement RepaymentsCircle => Driver.FindElement(repaymentscircle);

        public By incomecircle = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[7]/div/div[1]/div[3]/div/div[1]/div/h3");
        public IWebElement IncomeCircle => Driver.FindElement(incomecircle);

        public By debttoincomeratio = By.XPath("//*[@class='circle-svg-large']");
        public IWebElement DebtToIncomeRatio => Driver.FindElement(debttoincomeratio);        

        public By debttoincomeratiotext = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[7]/div/div[1]/div[5]/div/div[1]/h3[1]");
        public IWebElement DebtToIncomeRatioText => Driver.FindElement(debttoincomeratiotext);

        // Understand Your Debt

        /***
         * noofopenaccounts
         * ***/

        public By noofopenaccountsforhome = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[2]/div/div[2]/p");
        public IWebElement NoOfOpenAccountsForHome => Driver.FindElement(noofopenaccountsforhome);

        public By noofopenaccountsforcar = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[3]/div/div[2]/p");
        public IWebElement NoOfOpenAccountsForCar => Driver.FindElement(noofopenaccountsforcar);

        public By noofopenaccountsforclothing = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[4]/div/div[2]/p");
        public IWebElement NoOfOpenAccountsForClothing => Driver.FindElement(noofopenaccountsforclothing);

        public By noofopenaccountsforcreditcard = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[5]/div/div[2]/p");
        public IWebElement NoOfOpenAccountsForCreditCard => Driver.FindElement(noofopenaccountsforcreditcard);

        public By noofopenaccountsforloans = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[6]/div/div[2]/p");
        public IWebElement NoOfOpenAccountsForLoans => Driver.FindElement(noofopenaccountsforloans);

        /***
         * 
         * Your debt is made up of
         * 
         * ***/

        public By yourdeptismadeupofhome = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[2]/div/div[2]/p/parent::div/following-sibling::div[1]");
        public IWebElement YourDeptIsMadeUpOfHome => Driver.FindElement(yourdeptismadeupofhome);

        public By yourdeptismadeupofcar = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[3]/div/div[2]/p/parent::div/following-sibling::div[1]");
        public IWebElement YourDeptIsMadeUpOfCar => Driver.FindElement(yourdeptismadeupofcar);

        public By yourdeptismadeupofclothing = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[4]/div/div[2]/p/parent::div/following-sibling::div[1]");
        public IWebElement YourDeptIsMadeUpOfClothing => Driver.FindElement(yourdeptismadeupofclothing);

        public By yourdeptismadeupofcreditcard = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[5]/div/div[2]/p/parent::div/following-sibling::div[1]");
        public IWebElement YourDeptIsMadeUpOfCreditCard => Driver.FindElement(yourdeptismadeupofcreditcard);

        public By yourdeptismadeupofloans = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[6]/div/div[2]/p/parent::div/following-sibling::div[1]");
        public IWebElement YourDeptIsMadeUpOfLoans => Driver.FindElement(yourdeptismadeupofloans);

        /***
         * 
         * How much have you paid-off
         * 
         * **/

        public By howmuchhaveyoupaidoffhome = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[2]/div/div[2]/p/parent::div/following-sibling::div[2]");
        public IWebElement HowMuchHaveYouPaidOffHome => Driver.FindElement(howmuchhaveyoupaidoffhome);

        public By howmuchhaveyoupaidoffcar = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[3]/div/div[2]/p/parent::div/following-sibling::div[2]");
        public IWebElement HowMuchHaveYouPaidOffCar => Driver.FindElement(howmuchhaveyoupaidoffcar);

        public By howmuchhaveyoupaidoffclothing = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[4]/div/div[2]/p/parent::div/following-sibling::div[2]");
        public IWebElement HowMuchHaveYouPaidOffClothing => Driver.FindElement(howmuchhaveyoupaidoffclothing);

        public By howmuchhaveyoupaidoffcreditcard = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[5]/div/div[2]/p/parent::div/following-sibling::div[2]");
        public IWebElement HowMuchHaveYouPaidOffCreditCard => Driver.FindElement(howmuchhaveyoupaidoffcreditcard);

        public By howmuchhaveyoupaidoffloans = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[1]/div[2]/div[6]/div/div[2]/p/parent::div/following-sibling::div[2]");
        public IWebElement HowMuchHaveYouPaidOffLoans => Driver.FindElement(howmuchhaveyoupaidoffloans);


        public By totalcurrentbalance = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[6]/div/div/div/div[2]/div[2]/div[2]/div/h4");
        public IWebElement TotalCurrentBalance => Driver.FindElement(totalcurrentbalance);

        // Your Credit Score

        public By creditscore = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[1]/div/div[2]/div/div[1]/div[1]/div[1]/div[3]/div/div[1]/div/h2/span");
        public IWebElement CreditScore => Driver.FindElement(creditscore);

        public By creditscorestatus = By.XPath("/html/body/app-root/app-layout/app-dashboard/section/section[1]/div/div[2]/div/div[1]/div[1]/div[1]/div[3]/div/div[1]/div/h5");
        public IWebElement CreditScoreStatus => Driver.FindElement(creditscorestatus);




        public bool isHomePageDisplayed() {
            bool stat = false;
            try {
                if (HomePageVisible.Displayed) {
                    stat = true;
                    return stat;
                } else {
                    return false;
                }
                 
            } catch (Exception e)
            {
                return stat;
            }
           
        }
    }
}
