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

        public Task<UsersEntity?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
