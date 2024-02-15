using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeanAPITesting
{
    public class All
    {
        public class StoredGet
        {
            public int UserId { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public int Id { get; set; }
        }

        public class StoredComment
        {
            public int PostId { get; set; }
            public string Name { get; set; }
            public string Body { get; set; }
            public int Id { get; set; }
            public string Email { get; set; }
        }

        public class PostResponse
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
        }

        public class PostData
        {
            public required int UserId { get; set; }
            public required string Title { get; set; }
            public required string Body { get; set; }
        }

        public class PostComment
        {
            public required string Name { get; set; }
            public required string Body { get; set; }
            public required string Email { get; set; }
            public string PostId { get; set; }
            public int Id { get; set; }
        }

        public static HttpResponseMessage PostToPosts(HttpClient client, int userID, string title, string body)
        {
            var postData = new All.PostData
            {
                UserId = userID,
                Title = title,
                Body = body
            };
            var json = System.Text.Json.JsonSerializer.Serialize(postData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync("posts", content).Result;
            return response;
        }
        public static HttpResponseMessage PostToComments(HttpClient client, string name, string email, string body, int id)
        {
            var postComment = new All.PostComment
            {
                Name = name,
                Email = email,
                Body = body
            };
            var json = System.Text.Json.JsonSerializer.Serialize(postComment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync("posts/" + id.ToString() + "/comments", content).Result;
            return response;
        }

        public static HttpResponseMessage PutToPosts(HttpClient client, int userID, string title, string body, int id)
        {
            var postData = new All.PostData
            {
                UserId = userID,
                Title = title,
                Body = body
            };
            var json = System.Text.Json.JsonSerializer.Serialize(postData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PutAsync("posts/" + id.ToString(), content).Result;
            return response;
        }

        public static int GetPostId(HttpClient client) 
        {
            var response = client.GetAsync("posts").Result.Content.ReadAsStringAsync().Result;
            All.StoredGet[] items = JsonConvert.DeserializeObject<All.StoredGet[]>(response);
            var firstItem = items.FirstOrDefault();
            int firstItemID = firstItem.Id;
            return firstItemID;
        }
    }
}
