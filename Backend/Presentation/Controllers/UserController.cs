using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController(
        IUsersService usersService
    ) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await usersService.GetAllAsync();
            return Ok(result);
        }
    }
}