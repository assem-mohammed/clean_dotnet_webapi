using Dapper;
using Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Infrastructure.DbContexts
{
    public class DbQueries(IConfiguration configuration) : IDbQueries
    {
        private readonly SqlConnection connection = new(configuration.GetConnectionString("TestDb"));

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            var query = await connection.QueryAsync<T>(sql, param, transaction);

            return query.AsList();
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            var item = await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);

            return item!;
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            return await connection.QuerySingleAsync<T>(sql, param, transaction);
        }

        void IDisposable.Dispose() => GC.SuppressFinalize(this);
    }
}
