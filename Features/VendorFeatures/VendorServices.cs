using Contracts.Shared;
using Contracts.VendorFeatures;
using Domain.Entities;
using Domain.Interfaces;
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

        public VendorServices(IAppDbContext context, IStringLocalizer<Error> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<object> GetAllVendors(CancellationToken ct)
        {
            var vendors = await _context.Vendors.ToListAsync(ct);

            return vendors!;
        }

        public async Task<object> GetVendorById(string id, CancellationToken ct)
        {
            var vendor = await _context.Vendors.FirstOrDefaultAsync(x => x.Id == id, ct);

            vendor.ThrowIfNull(_ => new BusinessException(_localizer["VendotNotFoundEx"]));

            return vendor!;
        }

        public async Task<object> InsertBulk(CancellationToken ct)
        {
            List<Vendor> vendors = new();

            for (int i = 1; i <= 100000; i++)
            {
                vendors.Add(new()
                {
                    Id = $"{i}".PadLeft(10, '0'),
                    Email = $"Vendor{i}@email.com",
                    ModifiedBy = "Seeding",
                    Name = $"Vendor{i}"
                });
            }
        }
    }
}
