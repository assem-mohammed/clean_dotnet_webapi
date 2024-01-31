using API.Middlewares;
using API.DI;
using API.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(x =>
    {
        x.OperationFilter<AddCultureHeaderParameters>();
        x.OperationFilter<AddTimezoneHeaderParameters>();
    });

builder.ConfigureAPIServices();

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseExceptionHandleMiddleware();

//app.UseRequestLoggerMiddleware();
app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseTimeZoneMiddleware();

app.UseCultureMiddleware();

app.MapControllers();

app.Run();
