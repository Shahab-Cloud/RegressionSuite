using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;



namespace JM.MainUtils
{
    public class BaseStep : WebdriverSession
    {

        public static void scrollToElement(IWebElement Element)
        {
            ((IJavaScriptExecutor)WebdriverSession.Driver).ExecuteScript("arguments[0].scrollIntoView({ block: 'center', inline: 'center' });", Element);
            
        }

        public static void scrollToElementOnLazyLoadingPage(IWebElement Element)
        {
            var elementLocation = Element.Location;

            ((IJavaScriptExecutor)WebdriverSession.Driver).ExecuteScript($"window.scrollTo({elementLocation.X}, {elementLocation.Y});");
        }

        public static void sendkeys(IWebElement Element, String input)
        {
            Element.SendKeys(input);
        }

        public static void clearAndSendkeys(IWebElement Element, String input)
        {
            try 
            {
                BaseStep.wait.genericWait(2000);
                Element.Clear();
                Element.SendKeys(input);
            }
            catch 
            {
                Element.Clear();
                Element.SendKeys(input);
            }
           
            
        }

        public static void click(IWebElement Element)
        {
            Element.Click();
        }

        public class wait
        {
            public static void genericWait(int miliseconds)
            {
                Thread.Sleep(miliseconds);
            }
            public static void waitForElementVisibility(By locator)
            {

                WebDriverWait wait = new WebDriverWait(WebdriverSession.Driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.ElementIsVisible(locator));


            }

            public static void waitForElementVisibilityLongWait(By locator, int sec)
            {

                WebDriverWait wait = new WebDriverWait(WebdriverSession.Driver, TimeSpan.FromSeconds(sec));
                wait.Until(ExpectedConditions.ElementIsVisible(locator));

            }

            public static void waitForElementInvisibilityLongWait(By locator, int sec)
            {

                WebDriverWait wait = new WebDriverWait(WebdriverSession.Driver, TimeSpan.FromSeconds(sec));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(locator));

            }

            public static void waitForElementExistsLongWait(By locator, int sec)
            {

                WebDriverWait wait = new WebDriverWait(WebdriverSession.Driver, TimeSpan.FromSeconds(sec));
                wait.Until(ExpectedConditions.ElementExists(locator));

            }
            public static void waitForElementCliackableLongWait(By locator, int sec)
            {

                WebDriverWait wait = new WebDriverWait(WebdriverSession.Driver, TimeSpan.FromSeconds(sec));
                wait.Until(ExpectedConditions.ElementToBeClickable(locator));

            }
            public static void waitTillPageLoad()
            {
                IWebDriver driver = WebdriverSession.Driver;
                // Wait for the page to load completely
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(600));
        
                wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
               
                By spinner = By.XPath("//div[@class='overlay']");
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(spinner));
                By spinner1 = By.XPath("/html/body/app-root/ngx-spinner/div/div[1]");
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(spinner1));
            }

            public static void waitTillElementIsInteractable(By locator, int sec)
            {
                IWebDriver driver = WebdriverSession.Driver;
                // Wait for the page to load completely
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(sec));
                IWebElement element = wait.Until(d =>
                {
                    try
                    {
                        var elem = d.FindElement(locator);
                        if (elem.Displayed && elem.Enabled)
                            return elem;
                        return null;
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                });
                element.Click();
            }

            public static void waitTakingSystemTimeRefrence(double waitingTimeinHours)
            {
               var targetTime = currentDateTime.AddHours(waitingTimeinHours);
                TimeSpan timeToWait = targetTime - currentDateTime;
                int millisecondsToWait = (int)timeToWait.TotalMilliseconds;

                if (millisecondsToWait > 0)
                {
                    Thread.Sleep(millisecondsToWait);
                }
            }
        }

        public class Dropdown
        {

            public static void selectbyText(IWebElement Element, String text)
            {
                SelectElement selectElement = new SelectElement(Element);
                selectElement.SelectByText(text);
            }
        }

        public class getText
        {

            public static string text(IWebElement Element)
            {
                return Element.Text;
            }
            public static string textBox(IWebElement Element)
            {
                return Element.GetAttribute("value");
            }
            public static string button(IWebElement Element)
            {
                return Element.Text;
            }
            public static string dropdown(IWebElement Element)
            {
                return new SelectElement(Element).AllSelectedOptions.SingleOrDefault().Text;
            }


        }



    }
}
