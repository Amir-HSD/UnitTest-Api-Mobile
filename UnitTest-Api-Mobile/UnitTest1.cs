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

                // اینیشیالایز کردن کلاس رندوم برای ساخت کد اسکی برای کاراکتر
                random = new Random();

                // متغیر ولیو برای ولیو های ریگوئست
                string Value = "";

                // حلقه ساخت کلمات رندوم
                for (int i = 0; i < 10; i++)
                {
                    // ساخت کد اسکی رندوم
                    int num = random.Next(97, 122);

                    // تبدیل کد اسکی به کاراکتر و اضافه کردن به متغیر مورد نظر
                    // تبدیل کد اسکی به کاراکتر و اضافه کردن به متغیر مورد نظر
                    Value += Convert.ToChar(num).ToString();

                }

                // اضافه کردم پارامتر ها به کالکشن
                dictionary.Add("firstName", $"{Value}firstName");
                dictionary.Add("lastName", $"{Value}lastName");
                dictionary.Add("email", $"{Value}email.amirhsd.testapi@gmail.com");
                dictionary.Add("password", $"{Value}password1234");

                // تبدیل کالکشن به جیسون
                string BodyJson = JsonConvert.SerializeObject(dictionary);

                // ارسال درخواست به ادرس
                var Response = HC.PostAsync(HC.BaseAddress, new StringContent(BodyJson, Encoding.UTF8, "application/json")).Result;

                
                if (Response.IsSuccessStatusCode)
                {

                    // بررسی درستی پاسخ
                    Assert.AreEqual(HttpStatusCode.OK, Response.StatusCode);

                    // خواندن جیسون برگشتی و ریختن آن به داخل کالکشن مورد نظر
                    var ResponseJson = JsonConvert.DeserializeObject<Dictionary<string,string>>(Response.Content.ReadAsStringAsync().Result);

                    string Msg;

                    // دریافت مفدار پیغام برگشتی از سمت سرور
                    ResponseJson.TryGetValue("message", out Msg);

                    // چاپ خروجی
                    Console.WriteLine($"StatusCode : {Response.StatusCode} - Content:{Msg}");
                }
                else
                {
                    // اگر کد برگشتی اوکی نبود فیلد بشود
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
