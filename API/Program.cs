using API.Middlewares;
using API.DI;
using API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(x =>
{
    x.OperationFilter<AddCultureHeaderParameters>();
    x.OperationFilter<AddTimezoneHeaderParameters>();
});

builder.ConfigureAPIServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseExceptionHandleMiddleware();

app.UseTimeZoneMiddleware();

app.UseCultureMiddleware();

app.MapControllers();

app.Run();
