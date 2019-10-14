using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NewsFeed.Store.EF.Entities;
using NewsFeed.WebApi.ViewModel;

namespace NewsFeed.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public AccountController(
                UserManager<User> userManager,
                SignInManager<User> signInManager,
                ILogger<AccountController> logger,
                IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["BearerTokens:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(_config["BearerTokens:Issuer"],
                            _config["BearerTokens:Issuer"],
                            claims,
                            expires: DateTime.Now.AddDays(30),
                            signingCredentials: creds);

                        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                    }
                    else
                    {
                        var message = result.IsLockedOut ? "Is Locked Out" : result.IsNotAllowed ? "Is Not Allowed" : result.RequiresTwoFactor ? "Requires 2 Factor Login" : "Unknown Failed Login Reason";
                        return BadRequest("Could not create token -> " + message);
                    }
                }
            }

            return BadRequest("Could not create token");
        }
    }
}
