using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DbContexts
{
    public class ApplicationDbContext : DbContext, IAppDbContext
    {
        private readonly TimezoneHandler timezoneHandler;

        public ApplicationDbContext(DbContextOptions options,TimezoneHandler timezoneHandler) : base(options)
        {
            this.timezoneHandler = timezoneHandler;
        }

        public DbSet<Vendor> Vendors { get; set; } = default!;

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
            modelBuilder.Configure(timezoneHandler);

            base.OnModelCreating(modelBuilder);
        }
    }
}
