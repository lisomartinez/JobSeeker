using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobSeeker.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "users")]
        public ActionResult<string> Get()
        {
            return Ok("Success!");
        }
    }
}