using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                        var check = new CommonResponseModel { StatusCode = (int)response.StatusCode, Value = jsonObject };
                        return check;
                    }
                }
            }
            catch
            {
                return default(T);
            }
        }

        public async Task<T> PostAsyncHelper<T>(string requestUrl, object model) where T : class
        {

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var serialized = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await client.PostAsync(requestUrl, serialized))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync());
                            var responseContent = streamReader.ReadToEnd().Trim();
                            JObject j = JsonConvert.DeserializeObject<JObject>(responseContent);
                            var parsedata = new CommonResponseModel { Id = int.Parse(j["id"].ToString()), StatusCode = (int)response.StatusCode, Token = j["token"].ToString() };
                            return (T)Convert.ChangeType(parsedata, typeof(T));
                        }
                        else
                        {
                            var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync());
                            var responseContent = streamReader.ReadToEnd().Trim();
                            JObject j = JsonConvert.DeserializeObject<JObject>(responseContent);
                            var parsedata = new CommonResponseModel { Error = j["error"].ToString(), StatusCode = (int)response.StatusCode};
                            return (T)Convert.ChangeType(parsedata, typeof(T));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return default;
            }

        }

        public T ReadJsonFile<T>(string fileName)
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd().Trim();//.Replace('.', '_');
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

    }
}
