namespace Contracts.VendorFeatures;

public interface IVendorServices
{
    Task<object> GetVendorById(string id, CancellationToken ct);
    Task<object> GetAllVendors(CancellationToken ct);
    Task<object> InsertBulk(CancellationToken ct);
}
