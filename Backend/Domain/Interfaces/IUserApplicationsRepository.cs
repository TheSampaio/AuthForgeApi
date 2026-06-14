using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserApplicationsRepository
    {
        Task<UserApplicationsEntity?> GetGrantAsync(int userId, int applicationId);
        Task<int> CreateGrantAsync(int userId, int applicationId, string roles, int operationUserId);
    }
}