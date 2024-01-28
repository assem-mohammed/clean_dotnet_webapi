using Infrastructure.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public static class ConfigureEntitiesExtensions
{
    public static void Configure(this ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new VendorConfigurations());
    }
}
