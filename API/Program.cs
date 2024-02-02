using API.Middlewares;
using API.DI;
using API.Configurations;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

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

builder.ConfigureAPIServices();

var app = builder.Build();

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
