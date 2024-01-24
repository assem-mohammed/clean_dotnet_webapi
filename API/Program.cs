using API.Middlewares;
using Contracts;
using Features;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Contracts.Shared;
using API.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureAPIServices(builder.Configuration, builder.Host)
    .ConfigureAppServices()
    .ConfigureContractServices();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(x =>
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

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandleMiddleware();

app.Run();
