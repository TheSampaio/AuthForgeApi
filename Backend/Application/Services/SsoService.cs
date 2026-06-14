using Application.Contracts;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class SsoService(
        IUsersRepository usersRepository,
        IApplicationsRepository applicationsRepository,
        IUserApplicationsRepository userApplicationsRepository,
        ICryptoService cryptoService,
        IJwtService jwtService,
        IConfiguration configuration
    )
        : ISsoService
    {
        public async Task<Result<LoginResponse>> LoginAsync(SsoLoginRequest request)
        {
            var user = await usersRepository.GetByEmailAsync(request.Email);

            if (user is null || !cryptoService.VerifyPassword(request.Password, user.PasswordHash))
                return Result<LoginResponse>.Failure("Invalid email or password.");

            var application = await applicationsRepository.GetByClientIdAsync(request.ClientId);

            if (application is null || !application.IsActive)
                return Result<LoginResponse>.Failure("Invalid or inactive application.");

            var userAppGrant = await userApplicationsRepository.GetGrantAsync(user.Id, application.Id);

            if (userAppGrant is null || !userAppGrant.IsActive)
                return Result<LoginResponse>.Failure("User does not have permission to access this application.");

            var token = jwtService.GenerateToken(user, application.Name, userAppGrant.Roles);
            var expirationInMinutes = int.Parse(configuration["JwtSettings:ExpirationInMinutes"] ?? "60");

            return Result<LoginResponse>.Success(new LoginResponse(user.Email, token, expirationInMinutes));
        }

        public async Task<Result<LoginResponse>> RegisterAsync(SsoRegisterRequest request)
        {
            var application = await applicationsRepository.GetByClientIdAsync(request.ClientId);

            if (application is null || !application.IsActive)
                return Result<LoginResponse>.Failure("Invalid or inactive application.");

            var existingUser = await usersRepository.GetByEmailAsync(request.Email);

            if (existingUser is not null)
                return Result<LoginResponse>.Failure("Email is already in use.");

            var hashedPassword = cryptoService.HashPassword(request.Password);
            var newUser = new UsersEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = hashedPassword,
                Birthdate = request.Birthdate
            };

            var userId = await usersRepository.CreateAsync(newUser);
            newUser.Id = userId;

            const string defaultRole = "User";
            await userApplicationsRepository.CreateGrantAsync(userId, application.Id, defaultRole, userId);

            var token = jwtService.GenerateToken(newUser, application.Name, defaultRole);
            var expirationInMinutes = int.Parse(configuration["JwtSettings:ExpirationInMinutes"] ?? "60");

            return Result<LoginResponse>.Success(new LoginResponse(newUser.Email, token, expirationInMinutes));
        }
    }
}