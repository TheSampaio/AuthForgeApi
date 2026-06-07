using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok("Just a test from the 'Users' controller.");
        }
    }
}