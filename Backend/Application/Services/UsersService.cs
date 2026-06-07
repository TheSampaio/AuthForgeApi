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
        public async Task<IEnumerable<GetUsersResponse>> GetAllAsync()
        {
            var result = await usersRepository.GetAllAsync();
            return result.Select(user => new GetUsersResponse(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email
            ));
        }

        public async Task<GetUsersResponse?> GetByIdAsync(int id)
        {
            var result = await usersRepository.GetByIdAsync(id);
            return result is null
                ? null
                : new GetUsersResponse(
                    result.Id,
                    result.FirstName,
                    result.LastName,
                    result.Email
                );
        }
    }
}
