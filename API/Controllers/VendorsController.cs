using Contracts.VendorFeatures;
using Contracts.VendorFeatures.Dtos;
using Contracts.VendorFeatures.Dtos.Create;
using Contracts.VendorFeatures.Dtos.Delete;
using Contracts.VendorFeatures.Dtos.GetById;
using Contracts.VendorFeatures.Dtos.GetPaged;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SlidingRabbit.Producer;
namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VendorsController(IMessageProducer messageProducer, IVendorServices<IDbQueries> dapper, IVendorServices<IAppDbContext> vendorServices) : ControllerBase
{
    [HttpPost(nameof(TestMQ))]
    public void TestMQ()
    {
        messageProducer.SendMessage(new { id = 10 });
    }



    [HttpPost(nameof(GetPaged))]
    public async Task<GetVendorsPagedResponse> GetPaged([FromBody] GetVendorsPagedRequest request, IValidator<GetVendorsPagedRequest> validator, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: ct);

        messageProducer.SendMessage(request);

        return await vendorServices.GetVendorsPaged(request, ct);
    }

    [HttpGet("GetPagedDapper")]
    public async Task<GetVendorsPagedResponse> GetPagedDapper([FromQuery] GetVendorsPagedRequest request, IValidator<GetVendorsPagedRequest> validator, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken: ct);

        return await dapper.GetVendorsPaged(request, ct);
    }

    [HttpGet($"{nameof(request.Id)}")]
    [ResponseCache(Duration = 60, VaryByHeader = "culture,time-zone", Location = ResponseCacheLocation.Client)]
    public async Task<VendorResponse> GetVendorById([FromRoute] GetVendorByIdRequest request, IValidator<GetVendorByIdRequest> validator, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        return await vendorServices.GetVendorById(request, ct);
    }

    [HttpPost]
    public async Task<CreateVendorResponse> CreateVendor(CreateVendorRequest request, IValidator<CreateVendorRequest> validator, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        return await vendorServices.InsertOne(request, ct);
    }

    [HttpDelete]
    public async Task<DeleteVendorResponse> DeleteVendor(DeleteVendorRequest request, IValidator<DeleteVendorRequest> validator, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(request, ct);

        return await vendorServices.Delete(request, ct);
    }

    [HttpPost("InsertBulk")]
    public async Task<object> InsertBulk(CancellationToken ct)
    => await vendorServices.InsertBulk(ct);
}
