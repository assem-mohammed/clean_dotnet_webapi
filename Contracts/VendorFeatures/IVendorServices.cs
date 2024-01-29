using Contracts.VendorFeatures.Dtos;
using Contracts.VendorFeatures.Dtos.Create;
using Contracts.VendorFeatures.Dtos.Delete;
using Contracts.VendorFeatures.Dtos.GetById;
using Contracts.VendorFeatures.Dtos.GetPaged;

namespace Contracts.VendorFeatures;

public interface IVendorServices<T>
{
    Task<VendorResponse> GetVendorById(GetVendorByIdRequest request, CancellationToken ct);
    Task<DeleteVendorResponse> Delete(DeleteVendorRequest request, CancellationToken ct);
    Task<GetVendorsPagedResponse> GetVendorsPaged(GetVendorsPagedRequest request,CancellationToken ct);
    Task<object> InsertBulk(CancellationToken ct);
    Task<CreateVendorResponse> InsertOne(CreateVendorRequest request, CancellationToken ct);
}
