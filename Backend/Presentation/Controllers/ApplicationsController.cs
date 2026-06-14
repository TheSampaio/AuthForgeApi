using Application.Contracts;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ApplicationsController(IApplicationsService appsService) : ControllerBase
    {
        /// <summary>
        /// Registers a new application ecosystem. The creator is automatically assigned as Admin.
        /// </summary>
        /// <param name="request">The application details.</param>
        /// <returns>The generated public Client ID for the application.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateApplicationRequest request)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdString, out var userId))
                return Unauthorized(Result<string>.Failure("Invalid user token."));

            var result = await appsService.CreateApplicationAsync(request, userId);

            return result.IsSuccess
                ? Created(string.Empty, result)
                : BadRequest(result);
        }

        /// <summary>
        /// Grants a user access to a specific application ecosystem.
        /// </summary>
        /// <param name="request">The assignment configuration details.</param>
        /// <returns>HTTP status confirming the operation.</returns>
        [HttpPost("users")]
        public async Task<IActionResult> AssignUserAsync([FromBody] AssignUserRequest request)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdString, out var requesterUserId))
                return Unauthorized(Result<string>.Failure("Invalid user token."));

            var result = await appsService.AssignUserAsync(request, requesterUserId);

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }

        /// <summary>
        /// Retrieves a list of applications managed by the authenticated user.
        /// </summary>
        /// <returns>A collection of applications with their Client IDs.</returns>
        [HttpGet]
        public async Task<IActionResult> GetMyApplicationsAsync()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdString, out var userId))
                return Unauthorized(Result<string>.Failure("Invalid user token."));

            var result = await appsService.GetUserApplicationsAsync(userId);

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }
    }
}