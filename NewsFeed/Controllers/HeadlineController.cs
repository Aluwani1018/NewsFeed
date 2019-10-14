using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsFeed.Business.Interfaces;
using NewsFeed.Core.Models;

namespace NewsFeed.WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class HeadlineController : ControllerBase
    {
        private readonly IHeadlinesManager _headlinesManager;

        public HeadlineController
            (
                IHeadlinesManager headlinesManager
            )
        {
            this._headlinesManager = headlinesManager;
        }

        /// <summary>
        /// Gets News feeds from Google api
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetNewsHeadlines")]
        [Authorize(Policy = "NewsFeed")]
        public async Task<IActionResult> Get()
        {
            try
            {
                //generate claims information
                var results = await _headlinesManager.GetNewsHeadlineAsync();

                return Ok(results);
            }
            catch (Exception ex)
            {
                return BadRequest("Could not get GetNewsHeadlines -> " + ex.Message);
            }
        }
    }
}
