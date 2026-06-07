using Application.Contracts;

namespace Application.Interfaces
{
    public interface IUsersService
    {
        Task<IEnumerable<GetUsersResponse>> GetAllAsync();
        Task<GetUsersResponse?> GetByIdAsync(int id);
    }
}
