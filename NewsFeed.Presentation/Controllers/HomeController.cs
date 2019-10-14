using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NewsFeed.Core.Helpers;
using NewsFeed.Presentation.Models;
using Newtonsoft.Json;

namespace NewsFeed.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppSettings _appSettings;

        public HomeController
            (
                IOptions<AppSettings> appSettings
            )
        {
            this._appSettings = appSettings.Value;
        }

        /// <summary>
        /// gets a news headlines
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                string baseUrl = _appSettings.NewsFeedWebApi;

                string token = await GetToken();

                string jsonResults = await HttpHelper.GetAsync($"{baseUrl}/GetNewsHeadlines", token);

                var results = JsonConvert.DeserializeObject<HeadlineViewModel>(jsonResults);

                return View(results);
            }
            catch (Exception)
            {
                return Error();
            }
        }

        /// <summary>
        /// gets authorization token
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetToken()
        {
            string baseUrl = _appSettings.NewsFeedWebApi;

            string jsonResults = await HttpHelper.PostAync($"{baseUrl}/Login", 
                new LoginViewModel
                {
                    Email = "mathode.alu@gmail.com",
                    Password = "P@ssw0rd"
                });

            return JsonConvert.DeserializeObject<string>(jsonResults);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
