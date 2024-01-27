using API.Middlewares;
using Contracts;
using Features;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Contracts.Shared;
using API.DI;
using API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureAPIServices(builder.Configuration, builder.Host)
    .ConfigureAppServices()
    .ConfigureContractServices();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(x =>
{
    x.OperationFilter<AddCultureHeaderParameters>();
    x.OperationFilter<AddTimezoneHeaderParameters>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseTimeZoneMiddleware();

app.UseCultureMiddleware();

app.MapControllers();

app.UseExceptionHandleMiddleware();

app.Run();
