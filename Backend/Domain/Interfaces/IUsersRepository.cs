using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<UsersEntity>> GetAllAsync();
        Task<UsersEntity?> GetByIdAsync(int id);
    }
}
