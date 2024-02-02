using Contracts.VendorFeatures;
using Contracts.VendorFeatures.Dtos;
using Contracts.VendorFeatures.Dtos.Create;
using Contracts.VendorFeatures.Dtos.Delete;
using Contracts.VendorFeatures.Dtos.GetById;
using Contracts.VendorFeatures.Dtos.GetPaged;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VendorsController : ControllerBase
{
    private readonly IVendorServices<IDbQueries> _dapper;
    private readonly IVendorServices<IAppDbContext> _vendorServices;

    public VendorsController(IVendorServices<IDbQueries> dapper, IVendorServices<IAppDbContext> vendorServices)
    {
        _dapper = dapper;
        _vendorServices = vendorServices;
    }

    [HttpGet]
    public async Task<GetVendorsPagedResponse> GetPaged([FromQuery] GetVendorsPagedRequest request, IValidator<GetVendorsPagedRequest> validator, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(request);

        return await _vendorServices.GetVendorsPaged(request, ct);
    }

    [HttpGet("GetPagedDapper")]
    public async Task<GetVendorsPagedResponse> GetPagedDapper([FromQuery] GetVendorsPagedRequest request, IValidator<GetVendorsPagedRequest> validator, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(request);

        return await _dapper.GetVendorsPaged(request, ct);
    }

    [HttpGet("{Id}")]
    [ResponseCache(Duration = 60, VaryByHeader = "culture,time-zone", Location = ResponseCacheLocation.Client)]
    public async Task<VendorResponse> GetVendorById([FromRoute]GetVendorByIdRequest request, IValidator<GetVendorByIdRequest> validator, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        return await _vendorServices.GetVendorById(request, ct);
    }

    [HttpPost]
    public async Task<CreateVendorResponse> CreateVendor(CreateVendorRequest request, IValidator<CreateVendorRequest> validator, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        return await _vendorServices.InsertOne(request, ct);
    }

    [HttpDelete]
    public async Task<DeleteVendorResponse> DeleteVendor(DeleteVendorRequest request, IValidator<DeleteVendorRequest> validator, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        return await _vendorServices.Delete(request, ct);
    }

    [HttpPost("InsertBulk")]
    public async Task<object> InsertBulk(CancellationToken ct)
    => await _vendorServices.InsertBulk(ct);
}
