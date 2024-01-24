using Domain.Entities;
using Domain.OwenTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntitiesConfigurations
{
    public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrder>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrder> builder)
        {
            builder
                .OwnsOne(x => x.DocType, opt =>
                {
                    opt.HasIndex(x=>x.DocTypeName);
                    opt.HasIndex(x=>x.DocTypeCode);
                    opt.Property(x => x.DocTypeCode).HasColumnName(nameof(DocTypeDescription.DocTypeCode));
                    opt.Property(x => x.DocTypeName).HasColumnName(nameof(DocTypeDescription.DocTypeName));
                });

            builder
                .OwnsOne(x => x.Organization, opt =>
                {
                    opt.HasIndex(x => x.OrganizationCode);
                    opt.HasIndex(x => x.OrganizationName);
                    opt.Property(x => x.OrganizationCode).HasColumnName(nameof(OrganizationDescription.OrganizationCode));
                    opt.Property(x => x.OrganizationName).HasColumnName(nameof(OrganizationDescription.OrganizationName));
                });

            builder
                .HasMany(x => x.Lines)
                .WithOne(x => x.PurchaseOrder)
                .HasForeignKey(x => x.PurchaseOrderId)
                .IsRequired();
        }
    }
}
