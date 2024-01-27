using Contracts.Shared;
using Contracts.VendorFeatures;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Resources.ErrorLocalization;
using Throw;

namespace Features.VendorFeatures
{
    public class VendorServices : IVendorServices
    {
        private readonly IAppDbContext _context;
        private readonly IStringLocalizer<Error> _localizer;
        private readonly TimezoneHandler timezoneHandler;

        public VendorServices(IAppDbContext context, IStringLocalizer<Error> localizer, TimezoneHandler timezoneHandler)
        {
            _context = context;
            _localizer = localizer;
            this.timezoneHandler = timezoneHandler;
        }

        public async Task<object> GetAllVendors(CancellationToken ct)
        {
            var vendors = await _context.Vendors.ToListAsync(ct);

            vendors?.ForEach(x => x.DateCreated = x.DateCreated.ConvertUTCToLocalTime(timezoneHandler));

            return vendors!;
        }

        public async Task<object> GetVendorById(string id, CancellationToken ct)
        {
            var vendor = await _context.Vendors.FirstOrDefaultAsync(x => x.Id == id, ct);

            vendor.ThrowIfNull(_ => new BusinessException(_localizer["VendotNotFoundEx"]));

            vendor.DateCreated = vendor.DateCreated.ConvertUTCToLocalTime(timezoneHandler);

            return vendor!;
        }

        public async Task<object> InsertBulk(CancellationToken ct)
        {
            List<Vendor> vendors = new();

            for (int i = 1; i <= 100000; i++)
                vendors.Add(new()
                {
                    Id = $"{i}".PadLeft(10, '0'),
                    Email = $"Vendor{i}@email.com",
                    ModifiedBy = "Seeding",
                    Name = $"Vendor{i}",
                    FirstSearchTerm = $"V{i}",
                    Language = "EN",
                    Phone = $"971528{i.ToString().PadLeft(6, '0')}"
                });

            await _context.Vendors.AddRangeAsync(vendors);

            return await _context.SaveChangesAsync();
        }
    }
}
