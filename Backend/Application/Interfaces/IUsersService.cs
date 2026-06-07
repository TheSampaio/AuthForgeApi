using Application.Contracts;

namespace Application.Interfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<UserResponse>> GetAllAsync();
        Task<UserResponse?> GetByIdAsync(int id);
    }
}
