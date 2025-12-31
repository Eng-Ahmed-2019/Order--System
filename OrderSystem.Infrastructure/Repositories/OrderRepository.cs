using Dapper;
using OrderSystem.Domain.Entities;
using OrderSystem.Infrastructure.Data;
using OrderSystem.Application.Interfaces;

namespace OrderSystem.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperContext _dapperContext;

        public OrderRepository(DapperContext dapperContext)
        {
            _dapperContext = dapperContext;
        }

        public async Task<int>CreateAsync(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));

            var sql = @"INSERT INTO Orders (OrderNumber, TotalAmount, Status)
                    VALUES (@OrderNumber, @TotalAmount, @Status);
                    SELECT CAST(SCOPE_IDENTITY() as int);";

            using var conn = _dapperContext.CreateConnection();
            return await conn.ExecuteScalarAsync<int>(sql, order);
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            if (id == 0) throw new ArgumentNullException(nameof(id));

            var sql = "SELECT * FROM Orders WHERE Id = @Id";

            using var connection = _dapperContext.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<Order>(sql, new { Id = id });
        }
    }
}