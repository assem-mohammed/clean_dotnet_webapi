using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntitiesConfigurations
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {            
            builder
                .HasMany(x => x.PurchaseOrders)
                .WithOne(x=>x.Vendor)
                .HasForeignKey(x=>x.VendorCode)
                .IsRequired();
        }
    }
}
