using Application.Contracts;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// Authenticates a base user and issues a standard JWT token.
        /// </summary>
        /// <param name="request">User credentials.</param>
        /// <returns>A JWT token and user details if successful.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
        {
            var result = await authService.LoginAsync(request);

            if (!result.IsSuccess)
                return Unauthorized(new { result.Error });

            return Ok(result.Value);
        }

        /// <summary>
        /// Registers a new base user into the central identity system.
        /// </summary>
        /// <param name="request">User details for registration.</param>
        /// <returns>The ID of the newly created user.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            var result = await authService.RegisterAsync(request);

            if (!result.IsSuccess)
                return BadRequest(new { result.Error });

            return Created(string.Empty, new { UserId = result.Value });
        }
    }
}