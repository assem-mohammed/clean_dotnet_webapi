using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Contracts
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection ConfigureContractServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining(typeof(ConfigureServicesExtensions));

            return services;
        }
    }
}
