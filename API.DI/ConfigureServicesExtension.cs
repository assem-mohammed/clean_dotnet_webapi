using API.DI.ConfigurationOptions;
using Contracts.Shared;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

            services.Configure<ApiBehaviorOptions>(x =>
            {
                x.InvalidModelStateResponseFactory = (context) =>
                {
                    var messages = context.ModelState.Values
                        .Where(x => x.ValidationState == ModelValidationState.Invalid)
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToList();

                    throw new BusinessException(messages);
                };
            });

            var supportedCultures = spportedCulturConfig
                .Get<SupportedCultureOptions>()?.Cultures!;

            var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .Enrich.FromLogContext()
                    .CreateLogger();

            hostBuilder.UseSerilog(logger);

            services.
                AddDbContext<ApplicationDbContext>(opt =>
                {
                    opt.UseLoggerFactory(LoggerFactory.Create(x => x.AddSerilog(logger)));
                    opt.UseSqlServer(configuration.GetConnectionString("TestDb"), sqlOpt =>
                    {
                        sqlOpt.CommandTimeout(30);
                        sqlOpt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        sqlOpt.MaxBatchSize(1000);
                    });
                });

            services.AddScoped<IAppDbContext>(provider => provider.GetService<ApplicationDbContext>()!);

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