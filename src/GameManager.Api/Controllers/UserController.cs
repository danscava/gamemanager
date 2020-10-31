using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameManager.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<String> Get()
        {
            var rng = new Random();
            return new List<string>()
            {
                "a", "b"
            }
            .ToArray();
        }
    }
}
