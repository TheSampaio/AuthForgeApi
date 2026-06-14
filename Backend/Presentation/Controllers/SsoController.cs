using Application.Contracts;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SsoController(ISsoService ssoService) : ControllerBase
    {
        /// <summary>
        /// Authenticates a user against a specific application for Single Sign-On.
        /// </summary>
        /// <param name="request">The credentials and client identifier.</param>
        /// <returns>A successful authentication response containing the SSO JWT.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] SsoLoginRequest request)
        {
            var result = await ssoService.LoginAsync(request);
            return result.IsSuccess ? Ok(result) : Unauthorized(result);
        }

        /// <summary>
        /// Registers a new user and automatically enrolls them into the requesting SSO application.
        /// </summary>
        /// <param name="request">The user details and client identifier.</param>
        /// <returns>A successful registration response containing the SSO JWT.</returns>
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] SsoRegisterRequest request)
        {
            var result = await ssoService.RegisterAsync(request);
            return result.IsSuccess ? Created(string.Empty, result) : BadRequest(result);
        }
    }
}