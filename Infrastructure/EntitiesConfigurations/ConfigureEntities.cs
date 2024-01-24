using Microsoft.EntityFrameworkCore;

namespace Domain.EntitiesConfigurations
{
    public static class ConfigureEntities
    {
        public static void Configure(this ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new VendorConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseOrderConfiguration());
            modelBuilder.ApplyConfiguration(new PurchaseOrderLineConfiguration());
        }
    }
}
