using Contracts.VendorFeatures;
using Contracts.VendorFeatures.Dtos;
using Contracts.VendorFeatures.Dtos.Create;
using Contracts.VendorFeatures.Dtos.Delete;
using Contracts.VendorFeatures.Dtos.GetById;
using Contracts.VendorFeatures.Dtos.GetPaged;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.ExtensionMethods;
using Microsoft.Extensions.Localization;
using Shared;
using Shared.Resources.ErrorLocalization;
using System.Data;
using Throw;

namespace Features.VendorFeatures
{
    public class VendorServicesDapper<T> : IVendorServices<T>
    {
        private readonly IDbQueries _context;
        private readonly IStringLocalizer<Error> _localizer;
        private readonly Func<Vendor, VendorResponse> vendorExpression;

        public VendorServicesDapper(IDbQueries context, IStringLocalizer<Error> localizer, TimezoneHandler timezoneHandler)
        {
            _context = context;
            _localizer = localizer;
            vendorExpression = vendor => new VendorResponse(vendor.Id, vendor.Name, vendor.Email, vendor.Phone, vendor.Language, vendor.FirstSearchTerm,
            vendor.DateCreated.ConvertUTCToLocalTime(timezoneHandler), vendor.ModifiedBy, vendor.CentralBlock,
            vendor.DateRemoved.ConvertUTCToLocalTime(timezoneHandler),
            vendor.DateUpdated.ConvertUTCToLocalTime(timezoneHandler), vendor.FaxNumber, vendor.SecondSearchTerm, vendor.Group,
            vendor.IndustryKey, vendor.IsActive, vendor.IsDeleted, vendor.LegacyVendorCode, vendor.Name2,
            vendor.Name3, vendor.Name4, vendor.SSOUserId, vendor.Telephone, vendor.Title, vendor.VatRegNumber);
        }
        public Task<DeleteVendorResponse> Delete(DeleteVendorRequest request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<VendorResponse> GetVendorById(GetVendorByIdRequest request, CancellationToken ct)
        {
            string query = @$"SELECT TOP 1 * FROM Vendors WHERE VendorCode = '{request.Id}'";

            var vendor = await _context.QueryAsync<Vendor>(query);

            vendor.ThrowIfNull(_ => new BusinessException(_localizer["VendotNotFoundEx"]));

            vendor.Throw(_ => new BusinessException(_localizer["VendotNotFoundEx"])).IfFalse(x => x.Any());

            return vendor.Select(vendorExpression).First();
        }

        public async Task<GetVendorsPagedResponse> GetVendorsPaged(GetVendorsPagedRequest request, CancellationToken ct)
        {
            request.SearchKey = string.IsNullOrEmpty(request.SearchKey) ? "" : request.SearchKey;

            string query = @$"SELECT *,COUNT(*) OVER() FilterCount FROM 
                            (
	                            SELECT *,COUNT(*) OVER() TotalCount FROM vendors
                            )data_total_cnt
                            WHERE [Name] LIKE '%{request.SearchKey}%'
                            ORDER BY DateCreated
                            OFFSET {request.Page} ROWS
                            FETCH NEXT {request.Length} ROWS ONLY;";

            var vendors = await _context.QueryAsync<Vendor>(query);

            if (vendors == null || !vendors.Any())
                return new GetVendorsPagedResponse(new List<VendorResponse>(), 0, 0);

            var counts = vendors.Select(x => new { x.TotalCount, x.FilterCount }).First();

            return new GetVendorsPagedResponse(vendors.Select(vendorExpression), counts.TotalCount, counts.FilterCount);
        }

        public Task<object> InsertBulk(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<CreateVendorResponse> InsertOne(CreateVendorRequest request, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
    }
}
