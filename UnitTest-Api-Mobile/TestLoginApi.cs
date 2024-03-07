using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest_Api_Mobile
{

    [TestClass]
    public class TestLoginApi
    {
        private HttpClient HC;

        private Dictionary<object, object> dictionary;

        private Dictionary<HttpStatusCode, string> RP;

        public TestContext TC { get; set; }

        [TestInitialize]
        public void Initialization()
        {
            HC = new HttpClient();
            Console.WriteLine($"TestName: {TC.TestName}");
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

        public async Task LoginValidAccount()
        {
            await Task.Run(() =>
            {



            });
        }

    }
}
