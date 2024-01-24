using Contracts.PurchaseOrderFeatures;
using Features.PurchaseOrderFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Features
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection ConfigureAppServices(this IServiceCollection services)
        {
            services.AddScoped<IPurchaseOrderServices, PurchaseOrderServices>();

            return services;
        }
    }
}
