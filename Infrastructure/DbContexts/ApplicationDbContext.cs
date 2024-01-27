using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts
{
    public class ApplicationDbContext : DbContext, IAppDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Vendor> Vendors { get; set; } = default!;

        public async Task<int> SaveChangesAsync()
            => await base.SaveChangesAsync();

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>()
                .HaveMaxLength(100);

            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 6);

            base.ConfigureConventions(configurationBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Configure();

            base.OnModelCreating(modelBuilder);
        }
    }
}
