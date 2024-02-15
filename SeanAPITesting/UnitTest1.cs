using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;


namespace SeanAPITesting
{
    public class Tests
    {
        private HttpClient client { get; set; }
        private IWebDriver driver { get; set; }
        private Actions action { get; set; }
        [SetUp]
        public void Setup()
        {

            client = new HttpClient();
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/");

            //driver = new ChromeDriver();
           // driver.Manage().Window.Maximize();
            //driver.Navigate().GoToUrl("https://www.agdata.com/");
           // action = new Actions(driver);
        }

        [Test]
        //Post posts
        public void PostTest()
        {
            //Use the api to post a new record.
            var response = All.PostToPosts(client, 101 , "Test Title", "Test Body");
            Assert.IsTrue(response.IsSuccessStatusCode, "The post has failed please check uri and arguments");
            TestContext.WriteLine("The API was able to succesfully post");
            // Verify that what was returned matched what was sent
            var responseContent = response.Content.ReadAsStringAsync().Result;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var postResponse = System.Text.Json.JsonSerializer.Deserialize<All.PostResponse>(responseContent, options);
            Assert.AreEqual(101, postResponse.UserId,"The UserId does not match what was sent.");
            Assert.AreEqual("Test Title", postResponse.Title, "The Title does not match what was sent.");
            Assert.AreEqual("Test Body", postResponse.Body, "The Body does not match what was sent.");
            TestContext.WriteLine("The returned data matches what was sent.");
        }

        [Test]
        //GET posts
        public void GetPost()
        {
            var response = client.GetAsync("posts").Result.Content.ReadAsStringAsync().Result;
        }

        [Test]
        //PUT posts
        public void PutPost()
        {
            //Get the postID from the first object update it
            var responseID = All.GetPostId(client);
            Assert.IsNotNull(responseID, "no information was gotten by the API.");
            TestContext.WriteLine("Succefully grabbed a postId");

            var responsePut = All.PutToPosts(client, 1, "TestEmail@test.com", "Hello", responseID);

            Assert.IsTrue(responsePut.IsSuccessStatusCode, "The post has failed please check uri and arguments");
            TestContext.WriteLine("The API was able to succesfully post");
            // Verify that what was returned matched what was sent
            var responseContent = responsePut.Content.ReadAsStringAsync().Result;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var postResponse = System.Text.Json.JsonSerializer.Deserialize<All.PostResponse>(responseContent, options);
            Assert.AreEqual(responseID, postResponse.UserId, "The UserId does not match what was sent.");
            Assert.AreEqual("TestEmail@test.com", postResponse.Title, "The Title does not match what was sent.");
            Assert.AreEqual("Hello", postResponse.Body, "The Body does not match what was sent.");
            Assert.AreEqual(responseID, postResponse.Id, "PostId matches what was sent.");
            TestContext.WriteLine("The returned data matches what was sent.");
        
        }

        [Test]
        //DELET posts
        public void DeletePost()
        {
            //Get the first post to be deleted.
            var responseID = All.GetPostId(client);
            Assert.IsNotNull(responseID, "no information was gotten by the API.");
            TestContext.WriteLine("Records were found grabbing one for deleting");
            
            //Delete the grabbed post.
            var deleteRecord = client.DeleteAsync("posts/" + responseID.ToString()).Result;
            Assert.IsTrue(deleteRecord.IsSuccessStatusCode, "The deletion of the record has failed please check uri");
            TestContext.WriteLine("The record has been deleted.");
        }

        [Test]
        //POST Comments
        public void PostComment()
        {
            //Get the postID from the first object to post a new comment  to it.
            var responseID = All.GetPostId(client);
            Assert.IsNotNull(responseID, "The API call returned no information please check the uri");
            TestContext.WriteLine("Succefully grabbed a postId");

            //Post a new comment to the grabbed post.
            var responsePost = All.PostToComments(client,"Sean", "TestEmail@test.com", "Hello", responseID);
            Assert.IsTrue(responsePost.IsSuccessStatusCode, "The post has failed please check uri and arguments");
            TestContext.WriteLine("The API was able to succesfully post");
            // Verify that what was returned matched what was sent
            var responseContent = responsePost.Content.ReadAsStringAsync().Result;
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var postResponse = System.Text.Json.JsonSerializer.Deserialize<All.PostComment>(responseContent, options);
            Assert.AreEqual("Sean", postResponse.Name, "The name does not match what was sent.");
            Assert.AreEqual("TestEmail@test.com", postResponse.Email, "The email does not match what was sent.");
            Assert.AreEqual("Hello", postResponse.Body, "The body does not match what was sent.");
            Assert.AreEqual(responseID, Convert.ToInt32(postResponse.PostId),"PostId matches what was sent.");
            TestContext.WriteLine("The returned data matches what was sent.");
        }
        

        [Test]
        //GET Comments
        public void GetComments()
        {
            //Get the postID from the first object to get the comments attached to it
            var responseID = All.GetPostId(client);
            Assert.IsNotNull(responseID, "The API call returned no information please check the uri");
            TestContext.WriteLine("Succefully grabbed a postId");

            //Get the comments attached to the sent post.
            var response2 = client.GetAsync("comments?postId=" + responseID.ToString()).Result.Content.ReadAsStringAsync().Result;
            Assert.IsNotNull(response2, "The API call returned no information please check the uri");
            TestContext.WriteLine("Comments attached to the post were returned by the API");
            All.StoredComment[] allComments = JsonConvert.DeserializeObject<All.StoredComment[]>(response2);
            Assert.IsTrue(allComments.Count() > 0, "No comments were found. Please check postID");
            TestContext.WriteLine("Comments attached to the post were stored.");
        }

        [Test]
        //Fail GET Comments
        public void FailGetComments()
        {
            //Get the postID from the first object to get the comments attached to it
            var responseID = All.GetPostId(client);
            Assert.IsNotNull(responseID, "The API call returned no information please check the uri");
            TestContext.WriteLine("Succefully grabbed a postId");

            //Pass in an incorrect uri to get a 404 error
            var response2 = client.GetAsync("commets?postID" + responseID.ToString()).Result.StatusCode;
            Assert.IsTrue(Convert.ToInt32(response2) == 404,"The returned status was not 404.");
            TestContext.WriteLine("Passing in an incorrect uri gets back a 404 error.");
        }

        [Test]
        public void UITest()
        {
            //Sends the xpath for the menu and sub menu item so that the submenu can be displayed and clicked. 
            UI.SelectSubMenuItem(driver, action, "//li [@id = 'menu-item-992']", "//li [@id = 'menu-item-829']");
            Assert.IsTrue(driver.FindElement(By.XPath("//div[text() = 'Our Values']")).Displayed,"Company values are not shown on page.");
            TestContext.WriteLine("The test has navigated to the company overview page");
           
            //Gets and saves the company values into a list
            var value = UI.GetValues(driver, "//div[@class  = 'col box one-fourth']/h3");
            Assert.IsTrue(value.Count > 0,"No values were found");
            TestContext.WriteLine("The company values have been recorded");
            
            //Verifies that the company values shown on the web page are matching the correct values
            Assert.AreEqual("CURIOUS", value[0].ToString(), "The first company value is not CURIOUS.");
            Assert.AreEqual("TRANSFORMATIVE", value[1].ToString(), "The second company value is not TRANSFORMATIVE."); 
            Assert.AreEqual("AGILE", value[2].ToString(), "The third company value is not AGILE.");
            Assert.AreEqual("TRANSPARENT", value[3].ToString(), "The fourth company value is not TRANSPARENT.");
            TestContext.WriteLine("The company values are showing correctly on the web page.");
           
            //Clicks the Lets Get Started button to change to the Contact page.
            UI.ClickElement(driver, "//div[@class = 'banner-row']/a");
            
            //Verifies that the contact page has loaded and is being displayed.
            Assert.IsTrue(driver.FindElement(By.XPath("//h1[text() = 'GET IN TOUCH WITH US']")).Displayed, "The contact page is not showing.");
            TestContext.WriteLine("The contact page has been loaded through the Lets Get Started button");
            driver.Quit();
        }
    }
}