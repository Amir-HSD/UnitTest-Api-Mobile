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

        string UserName = "amirhossein@gmail.com";
        string Password = "amirhossein1234";

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

                Body.Add("email", $"{UserName}");
                Body.Add("password", $"{Password}");

                HttpResponseMessage LoginResponse = Login(Body);

                var ResultLogin = JsonConvert.DeserializeObject<Dictionary<object, object>>(LoginResponse.Content.ReadAsStringAsync().Result);

                if (LoginResponse.StatusCode == HttpStatusCode.OK)
                {
                    ResultLogin.TryGetValue("messge", out var LoginMessage);
                    if (LoginMessage.ToString().Contains("Successfully"))
                    {
                        ResultLogin.TryGetValue("jwt",out object jt);
                        jwtToken = jt.ToString();
                        Console.WriteLine(jt.ToString());
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

                Body.Add("title","ThisIsTestTitle3222");
                Body.Add("description", "ThisIsTestDescription222");
                Body.Add("priority", "High");
                Body.Add("category","Education");
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
                    if (TodoMessage.ToString().Contains("Access Denied"))
                    {
                        Console.WriteLine("Access Denied");
                        Assert.Fail();
                    }
                    else
                    {
                        Console.WriteLine(TodoMessage.ToString());
                        Assert.Fail();
                    }
                }

            });
        }

        [TestMethod]
        public async Task DeleteTodo()
        {
            await Task.Run(async() =>
            {
                Dictionary<object, object> Body = new Dictionary<object, object>();

                var jwtToken = string.Empty;

                Body.Add("email", $"{UserName}");
                Body.Add("password", $"{Password}");

                HttpResponseMessage LoginResponse = Login(Body);

                var ResultLogin = JsonConvert.DeserializeObject<Dictionary<object, object>>(LoginResponse.Content.ReadAsStringAsync().Result);

                if (LoginResponse.StatusCode == HttpStatusCode.OK)
                {
                    ResultLogin.TryGetValue("messge", out var LoginMessage);
                    if (LoginMessage.ToString().Contains("Successfully"))
                    {
                        ResultLogin.TryGetValue("jwt", out object jt);
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

                int TodoID = 140;

                var Request = new HttpRequestMessage(HttpMethod.Delete, $"https://mobile-todo-backend.onrender.com/delete-todo/{TodoID}");

                Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"{jwtToken}");

                HttpResponseMessage Response = await HC.SendAsync(Request);

                Console.WriteLine(Response.Content.ReadAsStringAsync().Result);

                var ResultJson = JsonConvert.DeserializeObject<Dictionary<object, object>>(Response.Content.ReadAsStringAsync().Result);
                ResultJson.TryGetValue("message", out object TodoMessage);
                if (TodoMessage.ToString().Contains("Todo deletion was successful"))
                {
                    Console.WriteLine("Todo Deleted Successfully");
                    ResultJson.TryGetValue("todo", out object TodoDetail);
                    Console.WriteLine(TodoDetail);
                    Assert.IsTrue(true);
                }
                else
                {
                    Console.WriteLine(TodoMessage.ToString());
                    Assert.Fail("Failed To Delete Todo!");
                }

            });
        }

        [TestMethod]
        public async Task UpdateToDoneTodo()
        {
            await Task.Run(async () =>
            {
                Dictionary<object, object> Body = new Dictionary<object, object>();

                var jwtToken = string.Empty;

                Body.Add("email", $"{UserName}");
                Body.Add("password", $"{Password}");

                HttpResponseMessage LoginResponse = Login(Body);

                var ResultLogin = JsonConvert.DeserializeObject<Dictionary<object, object>>(LoginResponse.Content.ReadAsStringAsync().Result);

                if (LoginResponse.StatusCode == HttpStatusCode.OK)
                {
                    ResultLogin.TryGetValue("messge", out var LoginMessage);
                    if (LoginMessage.ToString().Contains("Successfully"))
                    {
                        ResultLogin.TryGetValue("jwt", out object jt);
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

                int TodoID = 140;

                var Request = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://mobile-todo-backend.onrender.com/set-is-done/{TodoID}");

                Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"{jwtToken}");

                HttpResponseMessage Response = await HC.SendAsync(Request);

                Console.WriteLine(Response.Content.ReadAsStringAsync().Result);

                var ResultJson = JsonConvert.DeserializeObject<Dictionary<object, object>>(Response.Content.ReadAsStringAsync().Result);
                ResultJson.TryGetValue("message", out object TodoMessage);
                if (TodoMessage.ToString().Contains("Todo status was changed to Done"))
                {
                    Console.WriteLine("Todo SetToDone Successfully");
                    ResultJson.TryGetValue("todo", out object TodoDetail);
                    Console.WriteLine(TodoDetail);
                    Assert.IsTrue(true);
                }
                else
                {
                    Console.WriteLine(TodoMessage.ToString());
                    Assert.Fail("Failed To SetToDone Todo!");
                }

            });
        }

        [TestMethod]
        public async Task UpdateTodo()
        {
            await Task.Run(async () => {

                Dictionary<object, object> Body = new Dictionary<object, object>();

                var jwtToken = string.Empty;

                Body.Add("email", $"{UserName}");
                Body.Add("password", $"{Password}");

                HttpResponseMessage LoginResponse = Login(Body);

                var ResultLogin = JsonConvert.DeserializeObject<Dictionary<object, object>>(LoginResponse.Content.ReadAsStringAsync().Result);

                if (LoginResponse.StatusCode == HttpStatusCode.OK)
                {
                    ResultLogin.TryGetValue("messge", out var LoginMessage);
                    if (LoginMessage.ToString().Contains("Successfully"))
                    {
                        ResultLogin.TryGetValue("jwt", out object jt);
                        jwtToken = jt.ToString();
                        Console.WriteLine(jt.ToString());
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

                Body.Add("title", "ThisIsTestTitle222");
                Body.Add("description", "ThisIsTestDescriptionAfterUpdate");
                Body.Add("priority", "High");
                Body.Add("category", "Education");
                Body.Add("dueDate", $"{DateTime.Now.ToString("yyyy'-'MM'-'dd")}");

                var BodyJson = JsonConvert.SerializeObject(Body);

                int TodoID = 130;

                var Request = new HttpRequestMessage(new HttpMethod("PATCH"), $"https://mobile-todo-backend.onrender.com/update-todo/{TodoID}") { Content = new StringContent(BodyJson, Encoding.UTF8, "application/json") };

                Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"{jwtToken}");

                HttpResponseMessage Response = await HC.SendAsync(Request);

                Console.WriteLine(Response.Content.ReadAsStringAsync().Result);

                var ResultJson = JsonConvert.DeserializeObject<Dictionary<object, object>>(Response.Content.ReadAsStringAsync().Result);
                ResultJson.TryGetValue("message", out object TodoMessage);
                if (TodoMessage.ToString().Contains("successfully"))
                {
                    Console.WriteLine("Todo Update Successfully");
                    ResultJson.TryGetValue("todo", out object TodoDetail);
                    Console.WriteLine(TodoDetail);
                    Assert.IsTrue(true);
                }
                else
                {
                    if (TodoMessage.ToString().Contains("Access Denied"))
                    {
                        Console.WriteLine("Access Denied");
                        Assert.Fail();
                    }
                    else
                    {
                        Console.WriteLine(TodoMessage.ToString());
                        Assert.Fail();
                    }
                }

            });
        }

        [TestMethod]
        public async Task ShareTodo()
        {
            await Task.Run(async () => {

                Dictionary<object, object> Body = new Dictionary<object, object>();

                var jwtToken = string.Empty;

                Body.Add("email", $"{UserName}");
                Body.Add("password", $"{Password}");

                HttpResponseMessage LoginResponse = Login(Body);

                var ResultLogin = JsonConvert.DeserializeObject<Dictionary<object, object>>(LoginResponse.Content.ReadAsStringAsync().Result);

                if (LoginResponse.StatusCode == HttpStatusCode.OK)
                {
                    ResultLogin.TryGetValue("messge", out var LoginMessage);
                    if (LoginMessage.ToString().Contains("Successfully"))
                    {
                        ResultLogin.TryGetValue("jwt", out object jt);
                        jwtToken = jt.ToString();
                        Console.WriteLine(jt.ToString());
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

                int TodoID = 155;

                Body.Add("todoId", TodoID);
                Body.Add("targetEmail", "gqwamtrklgemail.amirhsd.testapi@gmail.com");

                var BodyJson = JsonConvert.SerializeObject(Body);

                var Request = new HttpRequestMessage(HttpMethod.Post, $"https://mobile-todo-backend.onrender.com/share-todo") { Content = new StringContent(BodyJson, Encoding.UTF8, "application/json") };

                Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"{jwtToken}");

                HttpResponseMessage Response = await HC.SendAsync(Request);

                Console.WriteLine(Response.Content.ReadAsStringAsync().Result);

                var ResultJson = JsonConvert.DeserializeObject<Dictionary<object, object>>(Response.Content.ReadAsStringAsync().Result);
                ResultJson.TryGetValue("message", out object TodoMessage);
                if (TodoMessage.ToString().Contains("successfully"))
                {
                    Console.WriteLine("Todo Shared Successfully");
                    ResultJson.TryGetValue("todo", out object TodoDetail);
                    Console.WriteLine(TodoDetail);
                    Assert.IsTrue(true);
                }
                else
                {
                    if (TodoMessage.ToString().Contains("Access Denied"))
                    {
                        Console.WriteLine("Access Denied");
                        Assert.Fail();
                    }
                    else
                    {
                        Console.WriteLine(TodoMessage.ToString());
                        Assert.Fail();
                    }
                }

            });
        }

    }
}
