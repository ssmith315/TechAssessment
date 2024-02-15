using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SeanAPITesting
{
    public class UI
    {

        public static void SelectSubMenuItem(IWebDriver driver, Actions action, string menuPath, string subMenuPath) 
        {
            //Finds and hovers the over the menu item to lower the submenu.
            IWebElement menuItem = driver.FindElement(By.XPath(menuPath));
            action.MoveToElement(menuItem).Perform();
            //Clicks on the submenu item
            driver.FindElement(By.XPath(subMenuPath)).Click();
        }

        public static List<string> GetValues(IWebDriver driver, string valuePath)
        {
            var valueElements = driver.FindElements(By.XPath(valuePath));
            List<string> valueList = new List<string>(valueElements.Count);
            for (int i = 0; i < valueElements.Count; i++)
            {
                valueList.Add(valueElements.ElementAt(i).Text);
            }
            return valueList;
        }

        public static void ClickElement(IWebDriver driver, string buttonPath) 
        {
            driver.FindElement(By.XPath(buttonPath)).Click();
        }
    }
}
