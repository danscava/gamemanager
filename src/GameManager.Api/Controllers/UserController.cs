using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using GameManager.Data.Dtos;
using GameManager.Data.Models;
using GameManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GameManager.Api.Controllers
{
    /// <summary>
    /// Controller responsible for user management and authentication
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly IOptions<ApiSettings> _apiSettings;

        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger,
            IOptions<ApiSettings> apiSettings,
            IUserService userService)
        {
            _logger = logger;
            _apiSettings = apiSettings;
            _userService = userService;
        }

        
        /// <summary>
        /// Authenticates a user
        /// </summary>
        /// <param name="request">Json with username and password</param>
        /// <returns>User info and token</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequestDto request)
        {
            var user = await _userService.Authenticate(request.username, request.password);

            if (user == null)
            {
                return BadRequest(new {message = "Username or password is incorrect"});
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_apiSettings.Value.Secret);

            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Login));
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var response = new AuthenticateResponseDto()
            {
                name = user.Name, 
                username = user.Login, 
                token = tokenString
            };

            return Ok(response);
        }
    }
}
