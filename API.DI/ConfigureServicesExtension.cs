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
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Contracts.PurchaseOrderFeatures;
using Contracts.VendorFeatures;
using Features.PurchaseOrderFeatures;
using Features.VendorFeatures;
using FluentValidation;
using Contracts;
namespace API.DI
{
    public static class ConfigureServicesExtension
    {
        public static WebApplicationBuilder ConfigureAPIServices(this WebApplicationBuilder builder)
        {
            var spportedCulturConfig = builder.Configuration
                .GetSection(SupportedCultureOptions.CONFIG_KEY);

            var supportedCultures = spportedCulturConfig
                .Get<SupportedCultureOptions>()?.Cultures!;

            var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .Enrich.FromLogContext()
                    .CreateLogger();

            builder.Host.UseSerilog(logger);

            builder.Services
                .Configure<SupportedCultureOptions>(spportedCulturConfig)
                .Configure<ApiBehaviorOptions>(x =>
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
                })
                .AddDbContextPool<ApplicationDbContext>(opt =>
                {
                    opt.UseLoggerFactory(LoggerFactory.Create(x => x.AddSerilog(logger)));
                    opt.UseSqlServer(builder.Configuration.GetConnectionString("TestDb"), sqlOpt =>
                    {
                        sqlOpt.CommandTimeout(30);
                        sqlOpt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                        sqlOpt.MaxBatchSize(1000);
                    });
                })
                .AddScoped<IAppDbContext>(provider => provider.GetService<ApplicationDbContext>()!)
                .AddScoped<TimezoneHandler>()
                .AddLocalization()
                .AddRequestLocalization(options =>
                {
                    supportedCultures = new List<string>(supportedCultures.Where(x => !string.IsNullOrEmpty(x)));
                    if (supportedCultures.Any())
                    {
                        options.SetDefaultCulture(supportedCultures.FirstOrDefault()!);
                        options.AddSupportedCultures(supportedCultures.ToArray());
                        options.AddSupportedUICultures(supportedCultures.ToArray());
                    }
                })
                .AddValidatorsFromAssemblyContaining(typeof(BusinessException))
                .AddScoped<IPurchaseOrderServices, PurchaseOrderServices>()
                .AddScoped<IVendorServices, VendorServices>();

            return builder;
        }

    }
}