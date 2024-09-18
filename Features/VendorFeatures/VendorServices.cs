using Shared;
using Contracts.VendorFeatures;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Throw;
using Shared.Resources.ErrorLocalization;
using Contracts.VendorFeatures.Dtos.Create;
using Contracts.VendorFeatures.Dtos.GetById;
using Contracts.VendorFeatures.Dtos.Delete;
using Contracts.VendorFeatures.Dtos.GetPaged;
using Contracts.VendorFeatures.Dtos;
using System.Linq.Expressions;
namespace Features.VendorFeatures;

public class VendorServices<T>(IAppDbContext context, IStringLocalizer<Error> localizer, TimezoneHandler timezoneHandler) : IVendorServices<T>
{
    private readonly Func<Vendor, VendorResponse> vendorExpression = vendor => new VendorResponse(vendor.Id, vendor.Name, vendor.Email, vendor.Phone, vendor.Language, vendor.FirstSearchTerm,
            vendor.DateCreated.ConvertUTCToLocalTime(timezoneHandler), vendor.ModifiedBy, vendor.CentralBlock,
            vendor.DateRemoved.ConvertUTCToLocalTime(timezoneHandler),
            vendor.DateUpdated.ConvertUTCToLocalTime(timezoneHandler), vendor.FaxNumber, vendor.SecondSearchTerm, vendor.Group,
            vendor.IndustryKey, vendor.IsActive, vendor.IsDeleted, vendor.LegacyVendorCode, vendor.Name2,
            vendor.Name3, vendor.Name4, vendor.SSOUserId, vendor.Telephone, vendor.Title, vendor.VatRegNumber);

    public async Task<DeleteVendorResponse> Delete(DeleteVendorRequest request, CancellationToken ct)
    {
        Vendor? vendor = await context.Vendors.FirstOrDefaultAsync(x => x.Id == request.Id, ct);

        vendor
            .ThrowIfNull(_ => new BusinessException(localizer["VendotNotFoundEx"]));

        vendor
            .Throw(_ => new BusinessException(localizer[""]))
            .IfTrue(x => x.IsDeleted.HasValue && x.IsDeleted.Value);

        context.Vendors.Remove(vendor);

        return new DeleteVendorResponse(await context.SaveChangesAsync(ct) > 0);
    }

    public async Task<GetVendorsPagedResponse> GetVendorsPaged(GetVendorsPagedRequest request, CancellationToken ct)
    {
        var totalCount = await context.Vendors.CountAsync(ct);

        if (totalCount == 0)
            return new GetVendorsPagedResponse([], 0, 0);

        request.Search ??= new Contracts.Search();

        Expression<Func<Vendor, bool>> expression = x =>
            string.IsNullOrEmpty(request.Search.Value) ||
            x.Email.Contains(request.Search.Value) ||
            x.Name.Contains(request.Search.Value);

        var filterCount = await context.Vendors.CountAsync(expression, ct);

        var vendors = await context.Vendors
            .Where(expression)
            .PageBy(request.Start, request.Length)
            .ToListAsync(ct);

        return new GetVendorsPagedResponse(vendors?.Select(vendorExpression), totalCount, filterCount);
    }

    public async Task<VendorResponse> GetVendorById(GetVendorByIdRequest request, CancellationToken ct)
    {
        var vendor = await context.Vendors.Where(x => x.Id == request.Id).ToListAsync(ct);

        vendor.ThrowIfNull(_ => new BusinessException(localizer["VendotNotFoundEx"]));
        vendor.Throw(_ => new BusinessException(localizer["VendotNotFoundEx"])).IfFalse(x => x.Count != 0);

        return vendor.Select(vendorExpression).First();
    }

    public async Task<object> InsertBulk(CancellationToken ct)
    {
        List<Vendor> vendors = [];

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

        await context.Vendors.AddRangeAsync(vendors, ct);

        return await context.SaveChangesAsync(ct);
    }

    public async Task<CreateVendorResponse> InsertOne(CreateVendorRequest request, CancellationToken ct)
    {
        var vendor = new Vendor()
        {
            Id = request.Id,
            Email = request.Email,
            Phone = request.Phone,
            FirstSearchTerm = request.FirstSearchTerm,
            Language = request.Language,
            ModifiedBy = "Seeding",
            Name = request.Name
        };

        await context.Vendors.AddAsync(vendor, ct);

        await context.SaveChangesAsync(ct);

        return new CreateVendorResponse(vendor.Id);
    }
}
