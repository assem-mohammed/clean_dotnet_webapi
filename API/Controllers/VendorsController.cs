using Contracts.VendorFeatures;
using Contracts.VendorFeatures.Dtos.Create;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VendorsController : ControllerBase
{
    private readonly IVendorServices _vendorServices;

    public VendorsController(IVendorServices vendorServices)
    {
        _vendorServices = vendorServices;
    }

    [HttpGet]
    public async Task<object> GetAllVendors(CancellationToken ct)
        => await _vendorServices.GetAllVendors(ct);

    [HttpGet("{id}")]
    public async Task<object> GetVendor(string id, CancellationToken ct)
        => await _vendorServices.GetVendorById(id, ct);

    [HttpPost("InsertBulk")]
    public async Task<object> InsertBulk(CancellationToken ct)
        => await _vendorServices.InsertBulk(ct);

    [HttpPost]
    public async Task<CreateVendorResponse> InsertOne(CreateVendorRequest request, IValidator<CreateVendorRequest> validators, CancellationToken ct)
    {
        await validators.ValidateAndThrowAsync(request, ct);

        return await _vendorServices.InsertOne(request, ct);
    }
}
