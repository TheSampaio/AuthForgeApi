using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Statements;
using System.Data;

namespace Infrastructure.Repositories
{
    public class UserApplicationsRepository(
        IDbConnection dbConnection
    )
        : IUserApplicationsRepository
    {
        public async Task<UserApplicationsEntity?> GetGrantAsync(int userId, int applicationId)
        {
            var parameters = new { UserId = userId, ApplicationId = applicationId };
            return await dbConnection.QueryFirstOrDefaultAsync<UserApplicationsEntity>(
                UserApplicationsStatements.SelectGrant, parameters
            );
        }

        public async Task<int> CreateGrantAsync(int userId, int applicationId, string roles, int operationUserId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId);
            parameters.Add("ApplicationId", applicationId);
            parameters.Add("Roles", roles);
            parameters.Add("IsActive", 1);
            parameters.Add("OperationUserId", operationUserId);

            return await dbConnection.ExecuteScalarAsync<int>(
                UserApplicationsStatements.UpsertUserApplication, parameters, commandType: CommandType.StoredProcedure
            );
        }
    }
}