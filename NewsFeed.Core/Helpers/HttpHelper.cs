using System;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace NewsFeed.Core.Helpers
{
    public class HttpHelper
    {
        /// <summary>
        /// Get a request to an api url using http client
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<string> GetAsync(string url, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add
                    (
                        new MediaTypeWithQualityHeaderValue("application/json")
                    );

                var request = new HttpRequestMessage
                    (
                        HttpMethod.Get,
                        client.BaseAddress
                    );

                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    //return Object
                    return response.Content.ReadAsStringAsync().Result;

                }
                else
                {
                    // return this error;
                    return $"{response.StatusCode} ({response.ReasonPhrase})";
                }
            }
        }

        /// <summary>
        /// Post a request to an api url using http client
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<string> PostAync(string url, dynamic param)
        {
            //Convert json string to Byte
            var byteParam =
               new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(param)));

            byteParam.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync(new Uri(url), byteParam);

                if (response.IsSuccessStatusCode)
                {
                    //return Object
                    return response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    // return this error;
                    return $"{response.StatusCode} ({response.ReasonPhrase})";
                }
            }
        }
    }
}
