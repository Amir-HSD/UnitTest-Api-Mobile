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
using System.Text;
using Newtonsoft.Json.Serialization;

namespace UnitTest_Api_Mobile
{

    [TestClass]
    public class TestRegisterApi
    {
        private HttpClient HC;

        private Dictionary<string, string> dictionary;

        private Random random;

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
        public async Task CreateAccount()
        {
            await Task.Run(async () =>
            {
                // ست کردن ادرس مورد نظر
                HC.BaseAddress = new Uri("https://mobile-todo-backend.onrender.com/register");

                // ساخت کالکشن دیکشنری
                dictionary = new Dictionary<string,string>();
                random = new Random();

                string Value = "";

                int num;

                for (int i = 0; i < 10; i++)
                {
                    num = random.Next(97, 122);

                    Value += Convert.ToChar(num).ToString();

                }

                dictionary.Add("firstName", $"{Value}firstName");
                dictionary.Add("lastName", $"{Value}lastName");
                dictionary.Add("email", $"{Value}email.amirhsd.testapi@gmail.com");
                dictionary.Add("password", $"{Value}password1234");

                string BodyJson = JsonConvert.SerializeObject(dictionary);

                var Response = HC.PostAsync(HC.BaseAddress, new StringContent(BodyJson, Encoding.UTF8, "application/json")).Result;

                if (Response.IsSuccessStatusCode)
                {
                    Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);

                    var ResponseJson = JsonConvert.DeserializeObject<Dictionary<string,string>>(Response.Content.ReadAsStringAsync().Result);

                    string Msg;

                    ResponseJson.TryGetValue("message", out Msg);

                    Console.WriteLine($"StatusCode : {Response.StatusCode} - Content:{Msg}");
                }
                else
                {
                    Assert.Fail("Faild",Response.StatusCode);
                }

            });
        }

    }

    public class Test
    {
        public int userId { get; set; }
        public int id { get; set; }

        public string title { get; set; }
        public string body { get; set; }

        /*var json = Response.Content.ReadAsStringAsync().Result;

        List<Posts> ResponseContent = JsonConvert.DeserializeObject<List<Posts>>(json);

        Console.WriteLine($"StatusCode: {Response.StatusCode}");
        Console.WriteLine($"Content: {ResponseContent.Count}");*/

    }

}
