using API.DI.ConfigurationOptions;
using Shared;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Contracts.VendorFeatures;
using Features.VendorFeatures;
using FluentValidation;
using Contracts.VendorFeatures.Dtos.Create;
using System.Text.Json.Serialization;
using Serilog.Enrichers.Sensitive;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using Serilog.Exceptions;

namespace API.DI;

public static class ConfigureServicesExtension
{
    public static WebApplicationBuilder ConfigureAPIServices(this WebApplicationBuilder builder)
    {
        var spportedCulturConfig = builder.Configuration
            .GetSection(SupportedCultureOptions.CONFIG_KEY);

        var supportedCultures = spportedCulturConfig
            .Get<SupportedCultureOptions>()?.Cultures!;

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.WithExceptionDetails()
            .Enrich.WithSensitiveDataMasking(x =>
            {
                x.MaskingOperators.AddRange(new List<IMaskingOperator>()
                {
                    new EmailAddressMaskingOperator(),
                    new CreditCardMaskingOperator(),
                    new IbanMaskingOperator()
                });
                x.MaskProperties.Add("id");
                x.MaskProperties.Add("email");
                x.MaskProperties.Add("password");
            })
            .CreateLogger();

        builder.Host.UseSerilog();

        builder.Services
            .Configure<JsonOptions>(opt =>
            {
                opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            })
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
                //opt.UseLoggerFactory(LoggerFactory.Create(x => x.AddSerilog(logger)));
                opt.UseSqlServer(builder.Configuration.GetConnectionString("TestDb"), sqlOpt =>
                {
                    sqlOpt.CommandTimeout(30);
                    sqlOpt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    sqlOpt.MaxBatchSize(1000);
                });
            })
            .AddScoped<IAppDbContext>(provider => provider.GetService<ApplicationDbContext>()!)
            .AddScoped<IDbQueries, DbQueries>()
            .AddScoped<IDbCommands, DbCommands>()
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
            .AddValidatorsFromAssemblyContaining(typeof(CreateVendorValidator))
            .AddScoped<IVendorServices<IAppDbContext>, VendorServices<IAppDbContext>>()
            .AddScoped<IVendorServices<IDbQueries>, VendorServicesDapper<IDbQueries>>();

        return builder;
    }

}