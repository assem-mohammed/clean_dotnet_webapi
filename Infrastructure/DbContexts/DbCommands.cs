using Dapper;
using Domain.Interfaces;
using System.Data;

namespace Infrastructure.DbContexts
{
    public class DbCommands(IAppDbContext context) : IDbCommands
    {
        public async Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            return await context.Connection.ExecuteAsync(sql, param, transaction);
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            var query = await context.Connection.QueryAsync<T>(sql, param, transaction);

            return query.AsList();
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            var item = await context.Connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction);

            return item!;
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default)
        {
            var item = await context.Connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction);

            return item!;
        }

        void IDisposable.Dispose() => GC.SuppressFinalize(this);
    }
}
