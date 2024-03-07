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
                    ResponseJson.TryGetValue("messge", out object Msg);

                    if (Msg.ToString().Contains("Successfully") || Msg.ToString().Contains("logged in"))
                    {
                        Assert.IsTrue(true);
                        ResponseJson.TryGetValue("jwt", out object Token);
                        Console.WriteLine($"Message: {Msg}");
                        Console.WriteLine($"Token:\n{Token}");
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

    }
}
