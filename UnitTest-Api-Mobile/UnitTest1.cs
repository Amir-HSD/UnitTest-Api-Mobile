using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Threading.Tasks;

namespace UnitTest_Api_Mobile
{

    [TestClass]
    public class ApiTest
    {

        private HttpClient HC;

        [TestInitialize]
        public void Initialization()
        {
            HC = new HttpClient();
        }
        [TestCleanup]
        public void Dispose()
        {
            HC.Dispose();
        }

        [TestMethod]
        public async Task Test_Register()
        {

            var Response = HC.GetAsync("https://jsonplaceholder.typicode.com/posts").Result;

            Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);
            var json = Response.Content.ReadAsStringAsync().Result;

            List<Posts> ResponseContent = JsonConvert.DeserializeObject<List<Posts>>(json);
            Console.WriteLine($"StatusCode: {Response.StatusCode}");
            Console.WriteLine($"Content: {ResponseContent.Count}");
        }

    }

    public class Posts
    {
        public int userId { get; set; }
        public int id { get; set; }

        public string title { get; set; }
        public string body { get; set; }
    }

}
