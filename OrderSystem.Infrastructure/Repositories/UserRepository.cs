using Dapper;
using OrderSystem.Domain.Entities;
using OrderSystem.Infrastructure.Data;
using OrderSystem.Application.Interfaces;

namespace OrderSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _dapperContext;

        public UserRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int> CreateAsync(User user)
        {
            if (user == null) { throw new ArgumentNullException("user"); }

            var sql = @"
                INSERT INTO Users (FullName, NationalId, Email, PasswordHash)
                VALUES (@FullName, @NationalId, @Email, @PasswordHash);
                SELECT CAST(SCOPE_IDENTITY() as int);
            ";

            using var conn = _dapperContext.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var sql = "SELECT * FROM Users WHERE Email = @Email";

            using var conn = _dapperContext.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<User>(sql, new { Email = email }) ??
                throw new Exception($"Not found user match with: {email}");
        }
    }
}