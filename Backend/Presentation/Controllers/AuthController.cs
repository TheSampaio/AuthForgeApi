using Application.Contracts;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController(
        IAuthService authService
    ) : ControllerBase
    {
        /// <summary>
        /// Registers a new user into the system.
        /// </summary>
        /// <param name="request">User details for registration.</param>
        /// <returns>The ID of the newly created user.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            try
            {
                var userId = await authService.RegisterAsync(request);
                return Created(string.Empty, new { UserId = userId });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        /// <summary>
        /// Authenticates a user and issues a JWT token.
        /// </summary>
        /// <param name="request">User credentials.</param>
        /// <returns>A JWT token and user details if successful.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var result = await authService.LoginAsync(request);
            return result is null
                ? Unauthorized(new { Error = "Invalid email or password." })
                : Ok(result);
        }
    }
}