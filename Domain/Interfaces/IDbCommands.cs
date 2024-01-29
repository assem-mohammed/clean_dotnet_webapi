using System.Data;

namespace Domain.Interfaces;

public interface IDbCommands : IDbQueries
{
    Task<int> ExecuteAsync(string sql, object? param = null, IDbTransaction? transaction = null, CancellationToken cancellationToken = default);
}
