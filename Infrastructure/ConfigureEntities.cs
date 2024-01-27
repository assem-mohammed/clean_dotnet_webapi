﻿using Infrastructure.EntitiesConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public static class ConfigureEntities
    {
        public static void Configure(this ModelBuilder modelBuilder, TimezoneHandler timezoneHandler)
        {
            modelBuilder.ApplyConfiguration(new VendorConfigurations(timezoneHandler));
        }
    }
}