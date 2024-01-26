using API.DI.ConfigurationOptions;
using Infrastructure;
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
            var spportedCulturConfig = configuration
                .GetSection(SupportedCultureOptions.CONFIG_KEY);

            services
                .Configure<SupportedCultureOptions>(spportedCulturConfig);

            var supportedCultures = spportedCulturConfig
                .Get<SupportedCultureOptions>()?.Cultures!;

            var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .Enrich.FromLogContext()
                    .CreateLogger();

            hostBuilder.UseSerilog(logger);

            services.
                AddDbContextPool<ApplicationDbContext>(opt =>
                {
                    opt.UseLoggerFactory(LoggerFactory.Create(x => x.AddSerilog(logger)));
                    opt.UseSqlServer(configuration.GetConnectionString("TestDb"), sqlOpt =>
                    {
                        sqlOpt.CommandTimeout(30);
                        sqlOpt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        sqlOpt.MaxBatchSize(1000);
                    });
                }, poolSize: 10);

            services.AddScoped<TimezoneHandler>();

            services.AddLocalization();

            services.AddRequestLocalization(options =>
            {
                supportedCultures = new List<string>(supportedCultures.Where(x => !string.IsNullOrEmpty(x)));
                if (supportedCultures.Any())
                {
                    options.SetDefaultCulture(supportedCultures.FirstOrDefault()!);
                    options.AddSupportedCultures(supportedCultures.ToArray());
                    options.AddSupportedUICultures(supportedCultures.ToArray());
                }
            });

            return services;
        }

    }
}