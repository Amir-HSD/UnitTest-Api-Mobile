using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace UnitTest_Api_Mobile
{
    [TestClass]
    public class TestTodoApi
    {
        private HttpClient HC;

        private Dictionary<object, object> dictionary;

        private Dictionary<HttpStatusCode, string> RP;

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
        public async Task CreateTodo()
        {
            await Task.Run(async () => {

                Dictionary<object,object> Body = new Dictionary<object,object>();

                Body.Add("title","ThisIsTestTitle");
                Body.Add("description", "ThisIsTestDescription");
                Body.Add("priority", "Low");
                Body.Add("category","Other");
                Body.Add("dueDate",$"{DateTime.Now.ToString("yyyy'-'MM'-'dd")}");

                var BodyJson = JsonConvert.SerializeObject(Body);

                var Request = new HttpRequestMessage(HttpMethod.Post, "https://mobile-todo-backend.onrender.com/create-todo") { Content= new StringContent(BodyJson, Encoding.UTF8,"application/json")};

                Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", "7uayJ5g91OzgudavtlGmJJJy2PuIMaxCb0BDNTOL9eT9EzDQVHiBMrdaq2VY6vMcUt08B3QhmELVp3LYslZ");

                //Request.Headers.Add("Authorization", "Bearer ${7uayJ5g91OzgudavtlGmJJJy2PuIMaxCb0BDNTOL9eT9EzDQVHiBMrdaq2VY6vMcUt08B3QhmELVp3LYslZ}");

                //Request.Headers.Add("Authorization", "Bearer <7uayJ5g91OzgudavtlGmJJJy2PuIMaxCb0BDNTOL9eT9EzDQVHiBMrdaq2VY6vMcUt08B3QhmELVp3LYslZ>");

                Request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Response = await HC.SendAsync(Request);

                Console.WriteLine(Response.Content.ReadAsStringAsync().Result);

                Assert.IsTrue(true);

                

            });
        }

    }
}
