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
        public HttpResponseMessage Login(Dictionary<object, object> dic)
        {
            // ست کردن ادرس مورد نظر
            HC.BaseAddress = new Uri("https://mobile-todo-backend.onrender.com/Login");

            // تبدیل کالکشن به جیسون
            string BodyJson = JsonConvert.SerializeObject(dic);

            // ارسال درخواست به ادرس
            HttpResponseMessage Response = HC.PostAsync(HC.BaseAddress, new StringContent(BodyJson, Encoding.UTF8, "application/json")).Result;

            return Response;

        }

        [TestMethod]
        public async Task CreateTodo()
        {
            await Task.Run(async () => {

                Dictionary<object, object> Body = new Dictionary<object, object>();

                var jwtToken = string.Empty;

                Body.Add("email", $"mvwhbabmrsemail.amirhsd.testapi@gmail.com");
                Body.Add("password", $"mvwhbabmrspassword1234");

                HttpResponseMessage LoginResponse = Login(Body);

                var ResultLogin = JsonConvert.DeserializeObject<Dictionary<object, object>>(LoginResponse.Content.ReadAsStringAsync().Result);

                if (LoginResponse.StatusCode == HttpStatusCode.OK)
                {
                    ResultLogin.TryGetValue("messge", out var LoginMessage);
                    if (LoginMessage.ToString().Contains("Successfully"))
                    {
                        Console.WriteLine(LoginResponse.Content.ReadAsStringAsync().Result);
                        ResultLogin.TryGetValue("jwt",out object jt);
                        jwtToken = jt.ToString();
                    }
                    else
                    {
                        Assert.Fail("Failed To Login Email Or Password is InValid");
                    }
                }
                else
                {
                    Assert.Fail("Failed To Login");
                }

                Body.Clear();

                Body.Add("title","ThisIsTestTitle");
                Body.Add("description", "ThisIsTestDescription");
                Body.Add("priority", "Low");
                Body.Add("category","Other");
                Body.Add("dueDate",$"{DateTime.Now.ToString("yyyy'-'MM'-'dd")}");

                var BodyJson = JsonConvert.SerializeObject(Body);

                var Request = new HttpRequestMessage(HttpMethod.Post, "https://mobile-todo-backend.onrender.com/create-todo") { Content= new StringContent(BodyJson, Encoding.UTF8,"application/json")};

                Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"{jwtToken}");

                HttpResponseMessage Response = await HC.SendAsync(Request);

                Console.WriteLine(Response.Content.ReadAsStringAsync().Result);

                var ResultJson = JsonConvert.DeserializeObject<Dictionary<object,object>>(Response.Content.ReadAsStringAsync().Result);
                ResultJson.TryGetValue("message", out object TodoMessage);
                if (TodoMessage.ToString().Contains("successfully"))
                {
                    Console.WriteLine("Todo Created Successfully");
                    ResultJson.TryGetValue("todo", out object TodoDetail);
                    Console.WriteLine(TodoDetail);
                    Assert.IsTrue(true);
                }
                else
                {
                    Assert.Fail("Failed To Create Todo!");
                }

            });
        }

    }
}
