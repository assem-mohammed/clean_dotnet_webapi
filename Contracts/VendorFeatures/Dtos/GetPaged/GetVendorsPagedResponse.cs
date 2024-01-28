namespace Contracts.VendorFeatures.Dtos.GetPaged;

public record GetVendorsPagedResponse(IEnumerable<VendorResponse>? data, int? totalCount, int? filterCount);
