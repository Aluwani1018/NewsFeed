using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NewsFeed.ThirdpartyIntergration.Helpers
{
    public class HttpHelper
    {
        /// <summary>
        /// Get a request to an api url using http
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<string> GetAsync(string url, string param)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();

                client.DefaultRequestHeaders.Accept.Add
                    (
                        new MediaTypeWithQualityHeaderValue("application/json")
                    );

                var request = new HttpRequestMessage
                    (
                        HttpMethod.Get,
                        client.BaseAddress
                    );

                request.Content = new StringContent
                    (
                        param,
                        Encoding.UTF8,
                        "application/json"
                    );

                var response = await client.GetAsync($"{url}{param}");

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
