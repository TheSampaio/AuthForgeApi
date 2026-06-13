using Application.Contracts;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class AuthService(
        IUsersRepository usersRepository,
        ICryptoService cryptoService,
        IJwtService jwtService,
        IConfiguration configuration
    ) : IAuthService
    {
        public async Task<int> RegisterAsync(RegisterRequest request)
        {
            var existingUser = await usersRepository.GetByEmailAsync(request.Email);

            if (existingUser is not null)
                throw new InvalidOperationException("Email is already in use.");

            var hashedPassword = cryptoService.HashPassword(request.Password);

            var newUser = new UsersEntity
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PasswordHash = hashedPassword,
                Birthdate = request.Birthdate
            };

            return await usersRepository.CreateAsync(newUser);
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var user = await usersRepository.GetByEmailAsync(request.Email);

            if (user is null || !cryptoService.VerifyPassword(request.Password, user.PasswordHash))
                return null; // Unauthorized

            var token = jwtService.GenerateToken(user);
            var expirationInMinutes = int.Parse(configuration["JwtSettings:ExpirationInMinutes"] ?? "60");

            return new LoginResponse(user.Email, token, expirationInMinutes);
        }
    }
}