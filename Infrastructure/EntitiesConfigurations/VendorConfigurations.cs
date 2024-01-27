using Domain.Entities;
using Infrastructure.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EntitiesConfigurations
{
    public class VendorConfigurations : IEntityTypeConfiguration<Vendor>
    {
        private readonly TimezoneHandler timezoneHandler;

        public VendorConfigurations(TimezoneHandler timezoneHandler)
        {
            this.timezoneHandler = timezoneHandler;
        }
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder
                .HasKey(x => x.Id)
                .HasName("VendorCode");

            builder
                .Property(x => x.Id)
                .HasMaxLength(10);

            builder
                .Property(x => x.LegacyVendorCode)
                .HasMaxLength(20);

            builder
                .Property(x => x.Group)
                .HasMaxLength(4);

            builder
                .Property(x => x.Title)
                .HasMaxLength(15);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(35);

            builder
                .Property(x => x.Name2)
                .HasMaxLength(35);

            builder
                .Property(x => x.Name3)
                .HasMaxLength(35);

            builder
                .Property(x => x.Name4)
                .HasMaxLength(35);

            builder
                .Property(x => x.FirstSearchTerm)
                .IsRequired()
                .HasMaxLength(10);

            builder
                .Property(x => x.SecondSearchTerm)
                .HasMaxLength(20);

            builder
                .Property(x => x.Language)
                .IsRequired()
                .HasMaxLength(2);

            builder
                .Property(x => x.Telephone)
                .HasMaxLength(16);

            builder
                .Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(16);

            builder
                .Property(x => x.FaxNumber)
                .HasMaxLength(31);

            builder
                .Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(241);

            builder
                .Property(x => x.VatRegNumber)
                .HasMaxLength(20);

            builder
                .Property(x => x.IndustryKey)
                .HasMaxLength(4);

            builder
                .Property(x => x.CentralBlock)
                .HasMaxLength(1);

            builder
                .Property(x => x.SSOUserId)
                .HasMaxLength(50);

            builder.Property(x => x.DateCreated).HasConversion(new UTCtoLocalConverter(timezoneHandler));
        }
    }
}
