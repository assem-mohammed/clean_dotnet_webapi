using Contracts.VendorFeatures.Dtos.Create;

namespace Contracts.VendorFeatures;

public interface IVendorServices
{
    Task<object> GetVendorById(string id, CancellationToken ct);
    Task<object> GetAllVendors(CancellationToken ct);
    Task<object> InsertBulk(CancellationToken ct);
    Task<CreateVendorResponse> InsertOne(CreateVendorRequest request, CancellationToken ct);
}
