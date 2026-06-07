using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Statements;
using System.Data;

namespace Infrastructure.Repositories
{
    public class UsersRepository(
        IDbConnection dbConnection
    )
        : IUsersRepository
    {
        public async Task<IEnumerable<UsersEntity>> GetAllAsync()
        {
            var result = await dbConnection.QueryAsync<UsersEntity>(UsersStatements.SelectAll);
            return result;
        }

        public async Task<UsersEntity?> GetByIdAsync(int id)
        {
            var parameters = new { Id = id };
            var result = await dbConnection.QueryFirstOrDefaultAsync<UsersEntity>(
                UsersStatements.SelectById,
                parameters
            );
            return result;
        }

        public async Task<UsersEntity?> GetByEmailAsync(string email)
        {
            var parameters = new { Email = email };
            var result = await dbConnection.QueryFirstOrDefaultAsync<UsersEntity>(
                UsersStatements.SelectByEmail,
                parameters
             );
            return result;
        }

        public async Task<int> CreateAsync(UsersEntity user)
        {
            var parameters = new { 
                user.FirstName,
                user.LastName,
                user.Email,
                user.PasswordHash,
                user.Birthdate,
                IsActive = 1,
                OperationUserId = 0
            };

            var result = await dbConnection.ExecuteScalarAsync<int>(
                "sp_UpsertUser",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
    }
}
