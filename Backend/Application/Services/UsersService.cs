using Application.Contracts;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class UsersService(
        IUsersRepository usersRepository
    )
        : IUsersService
    {
        public async Task<Result<IEnumerable<UserResponse>>> GetAllAsync()
        {
            var result = await usersRepository.GetAllAsync();
            var response = result.Select(user => new UserResponse(user.Id, user.FirstName, user.LastName, user.Email));
            return Result<IEnumerable<UserResponse>>.Success(response);
        }

        public async Task<Result<UserResponse>> GetByIdAsync(int id)
        {
            var result = await usersRepository.GetByIdAsync(id);

            if (result is null)
                return Result<UserResponse>.Failure("User not found.");

            var response = new UserResponse(result.Id, result.FirstName, result.LastName, result.Email);
            return Result<UserResponse>.Success(response);
        }
    }
}