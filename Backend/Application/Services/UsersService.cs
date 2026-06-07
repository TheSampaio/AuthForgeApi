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

        public Task<GetUsersResponse?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
