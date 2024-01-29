using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;

namespace Domain.Interfaces;

public interface IAppDbContext
{
    public IDbConnection Connection { get; }
    
    DatabaseFacade Database { get; }

    public DbSet<Vendor> Vendors { get; }

    public Task<int> SaveChangesAsync(CancellationToken ct);
}
