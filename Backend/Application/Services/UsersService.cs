using Application.Contracts;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class UsersService(IUsersRepository usersRepository) : IUsersService
    {
        public async Task<Result<IEnumerable<UserResponse>>> GetAllAsync()
        {
            var result = await usersRepository.GetAllAsync();
            var response = result.Select(user => new UserResponse(user.FirstName, user.LastName, user.Email, user.Birthdate));
            return Result<IEnumerable<UserResponse>>.Success(response);
        }

        public async Task<Result<UserResponse>> GetByEmailAsync(string email)
        {
            var result = await usersRepository.GetByEmailAsync(email);

            if (result is null)
                return Result<UserResponse>.Failure("User not found.");

            var response = new UserResponse(result.FirstName, result.LastName, result.Email, result.Birthdate);
            return Result<UserResponse>.Success(response);
        }
    }
}