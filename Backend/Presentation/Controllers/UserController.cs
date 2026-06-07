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
        /// <summary>
        /// Get all active users.
        /// </summary>
        /// <returns>A list of all active users.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await usersService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="id">The id of the user.</param>
        /// <returns>The user with the specified id.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var result = await usersService.GetByIdAsync(id);
            return result is null
                ? NotFound()
                : Ok(result);
        }
    }
}