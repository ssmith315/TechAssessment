using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace TechAssessment
{
        public class UiTests
        {
            private IWebDriver driver { get; set; }
            private Actions action { get; set; }
            [SetUp]
            public void Setup()
            {
                driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("https://www.agdata.com/");
                action = new Actions(driver);
            }

            [Test]
            public void UITest()
            {
                //Sends the xpath for the menu and sub menu item so that the submenu can be displayed and clicked. 
                UiClasses.SelectSubMenuItem(driver, action, "//li [@id = 'menu-item-992']", "//li [@id = 'menu-item-829']");
                Assert.IsTrue(driver.FindElement(By.XPath("//div[text() = 'Our Values']")).Displayed, "Company values are not shown on page.");
                TestContext.WriteLine("The test has navigated to the company overview page");

                //Gets and saves the company values into a list
                var value = UiClasses.GetValues(driver, "//div[@class  = 'col box one-fourth']/h3");
                Assert.IsTrue(value.Count > 0, "No values were found");
                TestContext.WriteLine("The company values have been recorded");

                //Verifies that the company values shown on the web page are matching the correct values
                Assert.AreEqual("CURIOUS", value[0].ToString(), "The first company value is not CURIOUS.");
                Assert.AreEqual("TRANSFORMATIVE", value[1].ToString(), "The second company value is not TRANSFORMATIVE.");
                Assert.AreEqual("AGILE", value[2].ToString(), "The third company value is not AGILE.");
                Assert.AreEqual("TRANSPARENT", value[3].ToString(), "The fourth company value is not TRANSPARENT.");
                TestContext.WriteLine("The company values are showing correctly on the web page.");

                //Clicks the Lets Get Started button to change to the Contact page.
                UiClasses.ClickElement(driver, "//div[@class = 'banner-row']/a");

                //Verifies that the contact page has loaded and is being displayed.
                Assert.IsTrue(driver.FindElement(By.XPath("//h1[text() = 'GET IN TOUCH WITH US']")).Displayed, "The contact page is not showing.");
                TestContext.WriteLine("The contact page has been loaded through the Lets Get Started button");
                driver.Quit();
            }
        }
    }


