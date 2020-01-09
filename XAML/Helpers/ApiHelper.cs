using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XAML.Models;

namespace XAML.Helpers
{
    public class ApiHelper
    {
        //Get queations
        public async static Task<List<Question>> GetQuestions(int category, string difficulty, string type, int amount)
        {
            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            //Add a user-agent header to the GET request. 
            var headers = httpClient.DefaultRequestHeaders;

            //The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
            //especially if the header value is coming from user input.
            string header = "ie";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }
            
            string url = $"https://opentdb.com/api.php?amount={amount}&category={category}&difficulty={difficulty}";
            
            if (type != "mix")
            {
                url += $"&type={type}";
                    
            }
            HttpClient ApiHelper = new HttpClient();
            ApiHelper.DefaultRequestHeaders.Accept.Clear();
            ApiHelper.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
   

            //Api anrop
            using (HttpResponseMessage response = await ApiHelper.GetAsync(url))
            {
                //hämtar svaret ifrån api
                var answer = response.Content.ReadAsStringAsync();
                QuestionRoot questionRoot = JsonConvert.DeserializeObject<QuestionRoot>(answer.Result);
                return questionRoot.Questions.ToList();
            }
        }

        public async static Task<List<Category>> GetCategories()
        {
            //Create an HTTP client object
            Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

            //Add a user-agent header to the GET request. 
            var headers = httpClient.DefaultRequestHeaders;

            //The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
            //especially if the header value is coming from user input.
            string header = "ie";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }

            string url = $"https://opentdb.com/api_category.php";
            HttpClient ApiHelper = new HttpClient();
            ApiHelper.DefaultRequestHeaders.Accept.Clear();
            ApiHelper.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


            //Api anrop
            using (HttpResponseMessage response = await ApiHelper.GetAsync(url))
            {
                //hämtar svaret ifrån api
                var answer = response.Content.ReadAsStringAsync();
                CategoryRoot categoryRoot = JsonConvert.DeserializeObject<CategoryRoot>(answer.Result);
                return categoryRoot.Categories.ToList();
            }
        }

       
    }
}
