using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace UnitTest_Api_Mobile
{

    [TestClass]
    public class TestLoginApi
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

        

        
        public HttpResponseMessage Login(Dictionary<object, object> dic, out Dictionary<object, object> ResponseJson)
        {
            // ست کردن ادرس مورد نظر
            HC.BaseAddress = new Uri("https://mobile-todo-backend.onrender.com/Login");

            // تبدیل کالکشن به جیسون
            string BodyJson = JsonConvert.SerializeObject(dic);

            // ارسال درخواست به ادرس
            HttpResponseMessage Response = HC.PostAsync(HC.BaseAddress, new StringContent(BodyJson, Encoding.UTF8, "application/json")).Result;

            ResponseJson = JsonConvert.DeserializeObject<Dictionary<object, object>>(Response.Content.ReadAsStringAsync().Result);

            return Response;

        }

        [TestMethod]
        public async Task LoginValidAccount()
        {
            await Task.Run(() =>
            {

                
                dictionary = new Dictionary<object, object>();

                dictionary.Add("email", $"amirhsdtata@gmail.com");
                dictionary.Add("password", $"Aamirhsdtata123456");

                HttpResponseMessage Response = Login(dictionary, out Dictionary<object, object> ResponseJson);

                if (Response.StatusCode == HttpStatusCode.OK)
                {
                    ResponseJson.TryGetValue("message", out object Msg);

                    if (Msg.ToString().Contains("Successfully") || Msg.ToString().Contains("logged in"))
                    {
                        Assert.IsTrue(true);
                        ResponseJson.TryGetValue("jwt", out object Token);
                        Console.WriteLine($"Message: {Msg}");
                        Console.WriteLine($"Token:\n{Token}");
                    }
                    else if (Msg.ToString().Contains("not verified") || Msg.ToString().Contains("Your email is not verified"))
                    {
                        Assert.Inconclusive($"Email Is Not Verifed, Message: {Msg}");
                    }
                    else
                    {
                        Assert.Fail($"Invaild Account, Message: {Msg}");
                    }
                }
                else
                {
                    Assert.Fail("Faild");
                }

            });
        }

        [TestMethod]
        public async Task LoginInValidAccount()
        {
            await Task.Run(() =>
            {


                dictionary = new Dictionary<object, object>();

                dictionary.Add("email", $"mamadgholami@gmail.com");
                dictionary.Add("password", $"mamadgholampass");

                HttpResponseMessage Response = Login(dictionary, out Dictionary<object, object> ResponseJson);

                if (Response.StatusCode == HttpStatusCode.OK)
                {
                    ResponseJson.TryGetValue("message", out object Msg);

                    if (Msg.ToString().Contains("Successfully") || Msg.ToString().Contains("logged in"))
                    {
                        Assert.IsTrue(true);
                        ResponseJson.TryGetValue("jwt", out object Token);
                        Console.WriteLine($"Message: {Msg}");
                        Console.WriteLine($"Token:\n{Token}");
                    }
                    else if (Msg.ToString().Contains("not verified") || Msg.ToString().Contains("Your email is not verified"))
                    {
                        Assert.Inconclusive($"Email Is Not Verifed, Message: {Msg}");
                    }
                    else
                    {
                        Assert.Inconclusive($"Invaild Account, Message: {Msg}");
                    }
                }
                else
                {
                    Assert.Fail("Faild");
                }

            });
        }

        [TestMethod]
        public async Task LoginNotVerifedAccount()
        {
            await Task.Run(() =>
            {


                dictionary = new Dictionary<object, object>();

                dictionary.Add("email", $"mamadgholami@gmail.com");
                dictionary.Add("password", $"mamadgholampass");

                HttpResponseMessage Response = Login(dictionary, out Dictionary<object, object> ResponseJson);

                if (Response.StatusCode == HttpStatusCode.OK)
                {
                    ResponseJson.TryGetValue("message", out object Msg);

                    if (Msg.ToString().Contains("Successfully") || Msg.ToString().Contains("logged in"))
                    {
                        Assert.IsTrue(true);
                        ResponseJson.TryGetValue("jwt", out object Token);
                        Console.WriteLine($"Message: {Msg}");
                        Console.WriteLine($"Token:\n{Token}");
                    }
                    else if (Msg.ToString().Contains("not verified") || Msg.ToString().Contains("Your email is not verified"))
                    {
                        Assert.Inconclusive($"Email Is Not Verifed, Message: {Msg}");
                    }
                    else
                    {
                        Assert.Inconclusive($"Invaild Account, Message: {Msg}");
                    }
                }
                else
                {
                    Assert.Fail("Faild");
                }

            });
        }

        [TestMethod]
        public async Task ForgetAndResetPassword()
        {
            await Task.Run(async () =>
            {

                Dictionary<object,object> Body = new Dictionary<object,object>();

                Body.Add("email", "amirhsdtata@gmail.com");

                var BodyJsonForget = JsonConvert.SerializeObject(Body);

                var RequestToForget = new HttpRequestMessage(new HttpMethod("PATCH"), "https://mobile-todo-backend.onrender.com/forget-password") { Content = new StringContent(BodyJsonForget, Encoding.UTF8, "application/json") };

                HttpResponseMessage ResponseForget = await HC.SendAsync(RequestToForget);

                Console.WriteLine(ResponseForget.Content.ReadAsStringAsync().Result);

                var GetResult = JsonConvert.DeserializeObject<Dictionary<object,object>>(ResponseForget.Content.ReadAsStringAsync().Result);

                GetResult.TryGetValue("message", out var Message);

                if (Message.ToString().Contains("Successfully"))
                {
                    GetResult.TryGetValue("OTP", out var OTP);

                    Body.Add("OTP", OTP);
                    Body.Add("newPassword", "Aamirhsdtata123456");

                    var BodyJsonReset = JsonConvert.SerializeObject(Body);

                    var RequestToReset = new HttpRequestMessage(new HttpMethod("PATCH"), "https://mobile-todo-backend.onrender.com/reset-password") { Content= new StringContent(BodyJsonReset, Encoding.UTF8, "application/json") };

                    HttpResponseMessage ResponseReset = await HC.SendAsync(RequestToReset);

                    var GetResetResult = JsonConvert.DeserializeObject<Dictionary<object, object>>(ResponseReset.Content.ReadAsStringAsync().Result);

                    GetResetResult.TryGetValue("message", out var ResetMessage);

                    if (ResetMessage.ToString().Contains("successfully"))
                    {
                        Console.WriteLine("Successfully To Reset Password");
                        Assert.IsTrue(true);
                    }
                    else
                    {
                        Assert.Fail($"Faild To Reset Password Message: {ResetMessage}");
                    }

                    

                }
                else
                {
                    Assert.Fail("Faild To Forget Password");
                }

                

                

            });
        }
    }
}
