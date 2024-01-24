using Domain.Entities;
using Domain.OwenTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntitiesConfigurations
{
    public class PurchaseOrderLineConfiguration : IEntityTypeConfiguration<PurchaseOrderLine>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderLine> builder)
        {
            builder
                .OwnsOne(x => x.Tax, opt =>
                {
                    opt.HasIndex(x => x.TaxCode);
                    opt.HasIndex(x => x.TaxName);
                    opt.Property(x => x.TaxCode).HasColumnName(nameof(TaxDescription.TaxCode));
                    opt.Property(x => x.TaxName).HasColumnName(nameof(TaxDescription.TaxName));
                });

            builder
                .OwnsOne(x => x.Site, opt =>
                {
                    opt.HasIndex(x => x.SiteCode);
                    opt.HasIndex(x => x.SiteName);
                    opt.Property(x => x.SiteCode).HasColumnName(nameof(SiteDescription.SiteCode));
                    opt.Property(x => x.SiteName).HasColumnName(nameof(SiteDescription.SiteName));
                });

            builder
                .OwnsOne(x => x.Product, opt =>
                {
                    opt.HasIndex(x => x.ProdctCode);
                    opt.HasIndex(x => x.ProdctName);
                    opt.Property(x => x.ProdctCode).HasColumnName(nameof(ProductDescription.ProdctCode));
                    opt.Property(x => x.ProdctName).HasColumnName(nameof(ProductDescription.ProdctName));
                });

            builder.HasIndex(x=>x.IsActive);
            builder.HasIndex(x=>x.IsDeleted);
            builder.HasIndex(x=>x.Quantity);
            builder.HasIndex(x=>x.VatValue);
            builder.HasIndex(x=>x.Unit);
            builder.HasIndex(x=>x.City);
            builder.HasIndex(x=>x.Street);
            builder.HasIndex(x=>x.Currency);
            builder.HasIndex(x=>x.DateCreated);
            builder.HasIndex(x=>x.DateRemoved);
            builder.HasIndex(x=>x.DateUpdated);
            builder.HasIndex(x=>x.DeliveryDate);
            builder.HasIndex(x=>x.Discount);
            builder.HasIndex(x=>x.DiscountPercent);
            builder.HasIndex(x=>x.ExciseDutyValue);
            builder.HasIndex(x=>x.FreightValue);
            builder.HasIndex(x=>x.ItemLine);
            builder.HasIndex(x=>x.ModifiedBy);
            builder.HasIndex(x=>x.PostalCode);
            builder.HasIndex(x=>x.ProcurementType);
            builder.HasIndex(x=>x.ProductStatus);
            builder.HasIndex(x=>x.TotalAmount);
            builder.HasIndex(x=>x.TotalPoValue);
            builder.HasIndex(x=>x.Region);
            builder.HasIndex(x=>x.UnitPrice);
            builder.HasIndex(x=>x.TaxPercentage);
            builder.HasIndex(x=>x.Telephone);
            builder.HasIndex(x=>x.Status);
        }
    }
}
