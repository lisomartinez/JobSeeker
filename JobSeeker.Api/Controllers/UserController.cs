using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace JobSeeker.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "users")]
        public ActionResult<string> Get()
        {
            var jwt = Request.Headers["Authorization"].ToString().Split(" ")[1];
            _logger.LogInformation(jwt);
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(jwt);

            _logger.LogInformation(jwtSecurityToken.Subject);
            return Ok("Success!");
        }
    }
}