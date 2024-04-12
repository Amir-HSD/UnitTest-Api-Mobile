using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Windows;
using System.Runtime.InteropServices;

namespace UnitTest_Api_Mobile
{

    [TestClass]
    public class TestRegisterApi
    {
        private HttpClient HC;

        private Dictionary<object, object> dictionary;

        private Random random;

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

        public HttpResponseMessage Register(Dictionary<object, object> dic, out Dictionary<object,object> ResponseJson)
        {
            // ست کردن ادرس مورد نظر
            HC.BaseAddress = new Uri("https://mobile-todo-backend.onrender.com/register");

            // تبدیل کالکشن به جیسون
            string BodyJson = JsonConvert.SerializeObject(dic);

            // ارسال درخواست به ادرس
            HttpResponseMessage Response = HC.PostAsync(HC.BaseAddress, new StringContent(BodyJson, Encoding.UTF8, "application/json")).Result;

            ResponseJson = JsonConvert.DeserializeObject<Dictionary<object, object>>(Response.Content.ReadAsStringAsync().Result);

            return Response;

        }

        [TestMethod]
        public async Task CreateAccount()
        {
            await Task.Run(async () =>
            {
                dictionary = new Dictionary<object,object>();

                random = new Random();

                string Value = "";

                for (int i = 0; i < 10; i++)
                {
                    int num = random.Next(97, 122);

                    Value += Convert.ToChar(num).ToString();

                }

                dictionary.Add("firstName", $"{Value}firstName");
                dictionary.Add("lastName", $"{Value}lastName");
                dictionary.Add("email", $"{Value}email.amirhsd.testapi@gmail.com");
                dictionary.Add("password", $"{Value}password1234");

                HttpResponseMessage Response = Register(dictionary, out Dictionary<object, object> ResponseJson);
                if (Response.StatusCode == HttpStatusCode.OK)
                {

                    ResponseJson.TryGetValue("message", out object Msg);

                    if (Msg.ToString() == "User creation successful")
                    {
                        Assert.AreEqual(true, true);

                        Console.WriteLine($"Successfully To Register Account\nStatusCode: {Response.StatusCode}, Message: {Msg}");

                        ResponseJson.TryGetValue("createdUser", out object UserInfo);

                        Console.WriteLine($"User Info:\n{UserInfo}");
                    }
                    else
                    {
                        Assert.Inconclusive($"Faild To Register Account, Message: {Msg}");
                    }
                }
                else
                {
                    Assert.Fail("Faild");
                }

            });
        }

        [TestMethod]
        public async Task VerifyAccount()
        {
            await Task.Run(async () =>
            {
                dictionary = new Dictionary<object, object>();

                random = new Random();

                string Value = "";

                for (int i = 0; i < 10; i++)
                {
                    int num = random.Next(97, 122);

                    Value += Convert.ToChar(num).ToString();

                }

                dictionary.Add("firstName", $"{Value}firstName");
                dictionary.Add("lastName", $"{Value}lastName");
                dictionary.Add("email", $"{Value}email.amirhsd.testapi@gmail.com");
                dictionary.Add("password", $"{Value}password1234");

                HttpResponseMessage Response = Register(dictionary, out Dictionary<object, object> ResponseJson);
                if (Response.StatusCode == HttpStatusCode.OK)
                {

                    ResponseJson.TryGetValue("message", out object Msg);

                    if (Msg.ToString() == "User creation successful")
                    {
                        Console.WriteLine($"Successfully To Register Account\nStatusCode: {Response.StatusCode}, Message: {Msg}");

                        ResponseJson.TryGetValue("createdUser", out object createdUser);

                        Dictionary<object, object> UserInfo = new Dictionary<object, object>();
                        UserInfo = JsonConvert.DeserializeObject<Dictionary<object, object>>(createdUser.ToString());

                        UserInfo.TryGetValue("verificationKey", out object verificationKey);

                        HttpResponseMessage VerficationMessage = await HC.GetAsync(new Uri($"https://mobile-todo-backend.onrender.com/verify-email/{verificationKey}"));

                        if (VerficationMessage.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Successfully Verified");
                            Console.WriteLine(VerficationMessage.Content.ReadAsStringAsync().Result);
                            if (VerficationMessage.Content.ReadAsStringAsync().Result.Contains("successfully") || VerficationMessage.Content.ReadAsStringAsync().Result.Contains("verified"))
                            {
                                Assert.IsTrue(true);
                            }
                            else
                            {
                                Assert.Fail($"Faild To Verfication Account Result: {VerficationMessage.Content.ReadAsStringAsync().Result}");
                            }


                        }
                        else
                        {
                            Assert.Fail("Faild To Verfication Account");
                        }

                        

                        
                    }
                    else
                    {
                        Assert.Inconclusive($"Faild To Register Account, Message: {Msg}");
                    }
                }
                else
                {
                    Assert.Fail("Faild");
                }

            });
        }

        [TestMethod]
        public async Task CreateExistAccount()
        {
            await Task.Run(async () =>
            {

                dictionary = new Dictionary<object, object>();

                dictionary.Add("firstName", $"amirhsdtestapifirstName");
                dictionary.Add("lastName", $"amirhsdtestapilastName");
                dictionary.Add("email", $"amirhsdtestapi.amirhsd.testapi@gmail.com");
                dictionary.Add("password", $"amirhsdtestapipassword1234");

                HttpResponseMessage Response = Register(dictionary, out Dictionary<object, object> ResponseJson);
                if (Response.StatusCode == HttpStatusCode.OK)
                {

                    ResponseJson.TryGetValue("message", out object Msg);

                    if (Msg.ToString() == "User creation successful")
                    {
                        Assert.AreEqual(true,true);
                        
                        Console.WriteLine($"Successfully To Register Account\nStatusCode: {Response.StatusCode}, Message: {Msg}");
                    }
                    else
                    {
                        Assert.Inconclusive($"Faild To Register Account, Message: {Msg}");
                    }
                }
                else
                {
                    Assert.Fail("Faild");
                }

            });
        }

        [TestMethod]
        public async Task NotValidAccount_FistNameisEmpty()
        {

            await Task.Run(async () =>
            {

                dictionary = new Dictionary<object, object>();

                dictionary.Add("firstName", $"");
                dictionary.Add("lastName", $"amirhsdtestapilastName");
                dictionary.Add("email", $"amirhsdtestapi.amirhsd.testapi@gmail.com");
                dictionary.Add("password", $"amirhsdtestapipassword1234");

                HttpResponseMessage Response = Register(dictionary, out Dictionary<object, object> ResponseJson);
                if (Response.StatusCode == HttpStatusCode.OK)
                {

                    ResponseJson.TryGetValue("message", out object Msg);

                    if (Msg.ToString() == "User creation successful")
                    {
                        Assert.AreEqual(true, true);
                        Console.WriteLine($"Successfully To Register Account\nStatusCode: {Response.StatusCode}, Message: {Msg}");
                    }
                    else
                    {
                        Assert.Inconclusive($"Faild To Register Account, Message: {Msg}");
                    }
                }
                else
                {
                    Assert.Fail("Faild");
                }

            });

        }

        [TestMethod]
        public async Task NotValidAccount_LastNameisEmpty()
        {

            await Task.Run(async () =>
            {
                dictionary = new Dictionary<object, object>();

                dictionary.Add("firstName", $"amirhsdtestapifirstName");
                dictionary.Add("lastName", $"");
                dictionary.Add("email", $"amirhsdtestapi.amirhsd.testapi@gmail.com");
                dictionary.Add("password", $"amirhsdtestapipassword1234");

                HttpResponseMessage Response = Register(dictionary, out Dictionary<object, object> ResponseJson);
                if (Response.StatusCode == HttpStatusCode.OK)
                {
                    ResponseJson.TryGetValue("message", out object Msg);

                    if (Msg.ToString() == "User creation successful")
                    {
                        Assert.AreEqual(true, true);
                        Console.WriteLine($"Successfully To Register Account\nStatusCode: {Response.StatusCode}, Message: {Msg}");
                    }
                    else
                    {
                        Assert.Inconclusive($"Faild To Register Account, Message: {Msg}");
                    }
                }
                else
                {
                    Assert.Fail("Faild");
                }

            });

        }

        [TestMethod]
        public async Task NotValidAccount_EmailisEmpty()
        {

            await Task.Run(async () =>
            {

                dictionary = new Dictionary<object, object>();

                dictionary.Add("firstName", $"amirhsdtestapifirstName");
                dictionary.Add("lastName", $"amirhsdtestapilastName");
                dictionary.Add("email", $"");
                dictionary.Add("password", $"amirhsdtestapipassword1234");

                // Get response from api
                HttpResponseMessage Response = Register(dictionary, out Dictionary<object, object> ResponseJson);
                if (Response.StatusCode == HttpStatusCode.OK)
                {

                    ResponseJson.TryGetValue("message", out object Msg);

                    if (Msg.ToString() == "User creation successful")
                    {
                        Assert.AreEqual(true, true);
                        Console.WriteLine($"Successfully To Register Account\nStatusCode: {Response.StatusCode}, Message: {Msg}");
                    }
                    else
                    {
                        Assert.Inconclusive($"Faild To Register Account, Message: {Msg}");
                    }
                }
                else
                {
                    Assert.Fail("Faild");
                }

            });

        }

        [TestMethod]
        public async Task NotValidAccount_PasswordisEmpty()
        {

            await Task.Run(async () =>
            {

                dictionary = new Dictionary<object, object>();

                dictionary.Add("firstName", $"amirhsdtestapifirstName");
                dictionary.Add("lastName", $"amirhsdtestapilastName");
                dictionary.Add("email", $"amirhsdtestapi.amirhsd.testapi@gmail.com");
                dictionary.Add("password", $"");

                HttpResponseMessage Response = Register(dictionary, out Dictionary<object, object> ResponseJson);
                if (Response.StatusCode == HttpStatusCode.OK)
                {

                    ResponseJson.TryGetValue("message", out object Msg);

                    if (Msg.ToString() == "User creation successful")
                    {
                        Assert.AreEqual(true, true);
                        Console.WriteLine($"Successfully To Register Account\nStatusCode: {Response.StatusCode}, Message: {Msg}");
                    }
                    else
                    {
                        Assert.Inconclusive($"Faild To Register Account, Message: {Msg}");
                    }
                }
                else
                {
                    Assert.Fail("Faild");
                }

            });

        }

    }

}
