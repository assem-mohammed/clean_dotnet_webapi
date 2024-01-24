using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace API.DI
{
    public static class ConfigureServicesExtension
    {
        public static IServiceCollection ConfigureAPIServices(this IServiceCollection services, IConfiguration configuration, IHostBuilder hostBuilder)
        {
            var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .Enrich.FromLogContext()
                    .CreateLogger();

            hostBuilder.UseSerilog(logger);

            services.
                AddDbContextPool<ApplicationDbContext>(opt =>
                {
                    opt.UseLoggerFactory(LoggerFactory.Create(x => x.AddSerilog(logger)));
                    //opt.EnableSensitiveDataLogging();
                    //opt.EnableDetailedErrors();
                    opt.UseSqlServer(configuration.GetConnectionString("TestDb"), sqlOpt =>
                    {
                        sqlOpt.CommandTimeout(30);
                        sqlOpt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        sqlOpt.MaxBatchSize(1000);
                    });
                }, poolSize: 10);

            return services;
        }

    }
}