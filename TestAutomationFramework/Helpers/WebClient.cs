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
    /// <summary>
    /// A class for creating requests and getting responses from APIs in a generic way
    /// </summary>
    public class WebClient
    {
        private HttpClient _client;
        public WebClient()
        {
            _client = new HttpClient(); 
        }

        /// <summary>
        /// Call an unparameterised GET method based on the URL passed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        public async Task<Object> GetAsyncHelper<T>(string requestUrl) where T : class
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //Setting the header type of the request
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (HttpResponseMessage response = await client.GetAsync(requestUrl))
                    {
                        //check response is successful (200)
                        if (response.IsSuccessStatusCode)
                        {
                            //Read content from reponse
                            var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync());
                            var responseContent = streamReader.ReadToEnd().Trim();
                            //Deserialize the response to the generic type
                            var jsonObject = JsonConvert.DeserializeObject<T>(responseContent);
                            //Create a new response model
                            return new CommonResponseModel { StatusCode = (int)response.StatusCode, Value = jsonObject };          
                        }
                        else
                        {
                            return default(T);
                        }
                    }
                }
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Call a POST method based on the URL and model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<T> PostAsyncHelper<T>(string requestUrl, object model) where T : class
        {

            try
            {
                using (var client = new HttpClient())
                {
                    //Setting the header type of the request
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var serialized = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

                    using (HttpResponseMessage response = await client.PostAsync(requestUrl, serialized))
                    {
                        //Read content from response
                        var streamReader = new StreamReader(await response.Content.ReadAsStreamAsync());
                        var responseContent = streamReader.ReadToEnd().Trim();

                        //Deserialize the response to the generic type
                        JObject j = JsonConvert.DeserializeObject<JObject>(responseContent);

                        //Check response is successful (200)
                        if (response.IsSuccessStatusCode)
                        {
                            //parse ID, Status Code and Token data to the CommonResponseModel
                            var parsedata = new CommonResponseModel { Id = int.Parse(j["id"].ToString()), StatusCode = (int)response.StatusCode, Token = j["token"].ToString() };
                            return (T)Convert.ChangeType(parsedata, typeof(T));
                        }
                        //response is unsuccessful (400)
                        else
                        {
                            //Parse the error and status code data to the CommonResponseModel
                            var parsedata = new CommonResponseModel { Error = j["error"].ToString(), StatusCode = (int)response.StatusCode};
                            return (T)Convert.ChangeType(parsedata, typeof(T));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return default;
            }

        }
        /// <summary>
        /// Read data from JSON Files
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T ReadJsonFile<T>(string fileName)
        {
            using (StreamReader r = new StreamReader(fileName))
            {
                string json = r.ReadToEnd().Trim();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

    }
}
