using Application.Contracts;

namespace Application.Interfaces
{
    public interface IUsersService
    {
        Task<Result<IEnumerable<UserResponse>>> GetAllAsync();
        Task<Result<UserResponse>> GetByEmailAsync(string email);
    }
}