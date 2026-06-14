using Application.Contracts;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class ApplicationsService(
        IApplicationsRepository applicationsRepository,
        IUserApplicationsRepository userApplicationsRepository
    )
        : IApplicationsService
    {
        public async Task<Result<Guid>> CreateApplicationAsync(CreateApplicationRequest request, int requesterUserId)
        {
            var clientId = await applicationsRepository.CreateAsync(request.Name, requesterUserId);
            var application = await applicationsRepository.GetByClientIdAsync(clientId);

            if (application != null)
            {
                await userApplicationsRepository.CreateGrantAsync(requesterUserId, application.Id, "Admin", requesterUserId);
            }

            return Result<Guid>.Success(clientId);
        }

        public async Task<Result<bool>> AssignUserAsync(AssignUserRequest request, int requesterUserId)
        {
            var application = await applicationsRepository.GetByClientIdAsync(request.ClientId);

            if (application is null)
                return Result<bool>.Failure("Application not found.");

            if (request.UserId == requesterUserId && request.Role.Equals("User", StringComparison.OrdinalIgnoreCase))
            {
                await userApplicationsRepository.CreateGrantAsync(request.UserId, application.Id, "User", requesterUserId);
                return Result<bool>.Success(true);
            }

            var requesterGrant = await userApplicationsRepository.GetGrantAsync(requesterUserId, application.Id);
            var isRequesterAdmin = requesterGrant?.Roles?.Contains("Admin", StringComparison.OrdinalIgnoreCase) == true;

            if (!isRequesterAdmin)
                return Result<bool>.Failure("You do not have administrative privileges for this application.");

            await userApplicationsRepository.CreateGrantAsync(request.UserId, application.Id, request.Role, requesterUserId);

            return Result<bool>.Success(true);
        }

        public async Task<Result<IEnumerable<ApplicationResponse>>> GetUserApplicationsAsync(int userId)
        {
            var result = await applicationsRepository.GetByUserIdAsync(userId);
            var response = result.Select(app => new ApplicationResponse(
                app.Id,
                app.Name,
                app.ClientId
            ));

            return Result<IEnumerable<ApplicationResponse>>.Success(response);
        }
    }
}