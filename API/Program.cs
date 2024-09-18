using API.Middlewares;
using API.Configurations;
using Serilog;
using Serilog.Exceptions;
using Serilog.Enrichers.Sensitive;
using System.Text.Json.Serialization;
using Infrastructure.Converters.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Shared;
using Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Infrastructure;
using Contracts.VendorFeatures.Dtos.Create;
using Features.VendorFeatures;
using Contracts.VendorFeatures;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using SlidingRabbit.Models;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithExceptionDetails()
    .Enrich.WithSensitiveDataMasking(x =>
    {
        x.MaskingOperators.AddRange(
        [
                    new EmailAddressMaskingOperator(),
                    new CreditCardMaskingOperator(),
                    new IbanMaskingOperator()
        ]);
        x.MaskProperties.Add("id");
        x.MaskProperties.Add("email");
        x.MaskProperties.Add("password");
    })
    .CreateLogger();

builder.Host.UseSerilog();

IConfigurationSection mq = builder.Configuration.GetSection("RabbitMQSettings");

SlidingRabbitMQConfigurationOptions mqConfig = mq.Get<SlidingRabbitMQConfigurationOptions>() ?? throw new KeyNotFoundException("MQ Configuration not found");

builder.Services.ConfigureSlidingRabbitMQ(x =>
{
    x.URL = mqConfig.URL;
    x.HostName = mqConfig.HostName;
    x.PortNumber = mqConfig.PortNumber;
    x.StreamHostname = mqConfig.StreamHostname;
    x.StreamPortNumber = mqConfig.StreamPortNumber;
    x.VirtualHost = mqConfig.VirtualHost;
    x.UserName = mqConfig.UserName;
    x.Password = mqConfig.Password;
    x.StreamName = mqConfig.StreamName;
});

builder.Services
    .AddCors(x => x.AddDefaultPolicy(p => p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

builder.Services
    .AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddResponseCaching()
    .AddSwaggerGen(x =>
    {
        x.OperationFilter<AddCultureHeaderParameters>();
        x.OperationFilter<AddTimezoneHeaderParameters>();
    });

var spportedCulturConfig = builder.Configuration
            .GetSection(SupportedCultureOptions.CONFIG_KEY);

var supportedCultures = spportedCulturConfig
    .Get<SupportedCultureOptions>()?.Cultures!;

builder.Services
    .Configure<JsonOptions>(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        opt.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
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
    .AddDbContextPool<SystemUserIdentityDbContext>(opt =>
    {
        opt.UseSqlServer(builder.Configuration.GetConnectionString("TestDb"));
    })
    .AddScoped<IAppDbContext>(provider => provider.GetService<ApplicationDbContext>()!)
    .AddScoped<IDbQueries, DbQueries>()
    .AddScoped<IDbCommands, DbCommands>()
    .AddScoped<TimezoneHandler>()
    .AddLocalization()
    .AddRequestLocalization(options =>
    {
        supportedCultures = new List<string>(supportedCultures.Where(x => !string.IsNullOrEmpty(x)));
        if (supportedCultures.Count != 0)
        {
            options.SetDefaultCulture(supportedCultures.FirstOrDefault()!);
            options.AddSupportedCultures([.. supportedCultures]);
            options.AddSupportedUICultures([.. supportedCultures]);
        }
    })
    .AddValidatorsFromAssemblyContaining(typeof(CreateVendorValidator))
    .AddScoped<IVendorServices<IAppDbContext>, VendorServices<IAppDbContext>>()
    .AddScoped<IVendorServices<IDbQueries>, VendorServicesDapper<IDbQueries>>();

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedPhoneNumber = true;

    options.User.RequireUniqueEmail = true;

    options.Lockout.MaxFailedAccessAttempts = 3;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);

    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
})
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>(TokenOptions.DefaultProvider)
    .AddTokenProvider<PhoneNumberTokenProvider<IdentityUser>>(TokenOptions.DefaultPhoneProvider)
    .AddTokenProvider<EmailTokenProvider<IdentityUser>>(TokenOptions.DefaultEmailProvider)
    .AddTokenProvider<AuthenticatorTokenProvider<IdentityUser>>(TokenOptions.DefaultAuthenticatorProvider)
    .AddEntityFrameworkStores<SystemUserIdentityDbContext>();

var app = builder.Build();

app.UseCors();

app.UseRequestLoggerMiddleware();

app.UseExceptionHandleMiddleware();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseTimeZoneMiddleware();

app.UseCultureMiddleware();

app.MapControllers();

app.UseResponseCaching();

app.Run();
