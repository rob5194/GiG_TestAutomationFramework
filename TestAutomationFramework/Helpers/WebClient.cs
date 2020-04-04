using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TestAutomationFramework.Models;

namespace TestAutomationFramework.Helpers
{
    public class WebClient
    {
        private HttpClient _client;
        public WebClient()
        {
            _client = new HttpClient(); 
        }

        public async Task<Object> GetAsyncHelper<T>(string requestUrl) where T : class
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage response = await client.GetAsync(requestUrl))
                    {
                        response.EnsureSuccessStatusCode();
                        var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync());
                        var responseContent = streamReader.ReadToEnd().Trim();
                        var jsonObject = JsonConvert.DeserializeObject<T>(responseContent);
                        var check = new ResponseModel { statusCode = (int)response.StatusCode, value = jsonObject };
                        return check;
                    }
                }
            }
            catch
            {
                return default(T);
            }
        }

        public async Task<object> PostAsyncHelper<T>(string requestUrl, object model) where T : class
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var serialized = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await client.PostAsync(requestUrl, serialized))
                    {
                        response.EnsureSuccessStatusCode();
                        var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync());
                        var responseContent = streamReader.ReadToEnd().Trim();
                        var jsonObject = JsonConvert.DeserializeObject<T>(responseContent);
                        ResponseModel check = new ResponseModel { statusCode = (int)response.StatusCode, value = jsonObject};
                        return check;

                    }
                }
                //return default(T);
            }
            catch
            {
                return default(T);
            }

        }

        public string RequestParameters(string jsonString)
        {
            IDictionary<string, string> jsonInputCSharp = JsonConvert.DeserializeObject<IDictionary<string, string>>(jsonString);
            List<string> stringValues = new List<string>();
            foreach (var item in jsonInputCSharp)
            {
                if (!string.IsNullOrWhiteSpace(item.Value))
                {
                    stringValues.Add(item.Key + "=" + item.Value);
                }
            }
            return string.Format("?{0}", string.Join("&", stringValues));
        }

        public T ReadJsonFile<T>(string fileName)
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd();//.Replace('.', '_');
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

    }
}
