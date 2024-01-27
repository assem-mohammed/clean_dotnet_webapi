﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces
{
    public interface IAppDbContext
    {
        public DbSet<Vendor> Vendors { get; }
    }
}
