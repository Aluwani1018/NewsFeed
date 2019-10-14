using Microsoft.Extensions.Options;
using NewsFeed.Business.Interfaces;
using NewsFeed.Core.Helpers;
using NewsFeed.Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NewsFeed.Business.Manager
{
    public class HeadlineManage : IHeadlinesManager
    {
        private readonly AppSettings _appSettings;

        public HeadlineManage
            (
                IOptions<AppSettings> appSettings
            )
        {
            this._appSettings = appSettings.Value;
        }
        /// <summary>
        /// Gets News Headlines from Google api
        /// </summary>
        /// <returns></returns>
        public async Task<Headlines> GetNewsHeadlineAsync()
        {
            string baseUrl = _appSettings.NewsApi;

            string parameters = $"?sources={_appSettings.Source}&apiKey={_appSettings.ApiKey}";

            string jsonResults = await HttpHelper.GetAsync(baseUrl, parameters);

            return JsonConvert.DeserializeObject<Headlines>(jsonResults);
        }
    }
}
