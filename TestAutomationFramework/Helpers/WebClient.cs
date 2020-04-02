using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TestAutomationFramework.Enums;
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

        public async Task<object> InvokeAsyncHelper<T,U>(string baseUrl, object model, RequestTypes method) where T : class
        {

            try
            {
                switch (method)
                {
                    case RequestTypes.GET:
                        using (var client = new HttpClient())
                        {
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            using (HttpResponseMessage response = await client.GetAsync(baseUrl))
                            {
                                response.EnsureSuccessStatusCode();
                                var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync());
                                var responseContent = streamReader.ReadToEnd().Trim();
                                var jsonObject = JsonConvert.DeserializeObject<U>(responseContent);
                                var check = new ResponseModel { StatusCode = (int)response.StatusCode, Value = jsonObject };
                                return check;
                            }
                        }
                    default:
                        return default(T);
                }
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
