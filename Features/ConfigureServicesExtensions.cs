using Contracts.PurchaseOrderFeatures;
using Contracts.VendorFeatures;
using Features.PurchaseOrderFeatures;
using Features.VendorFeatures;
using Microsoft.Extensions.DependencyInjection;

namespace Features
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection ConfigureAppServices(this IServiceCollection services)
        {
            services.AddScoped<IPurchaseOrderServices, PurchaseOrderServices>();
            services.AddScoped<IVendorServices, VendorServices>();

            return services;
        }
    }
}
