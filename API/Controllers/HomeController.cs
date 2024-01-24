using Contracts.PurchaseOrderFeatures;
using Contracts.ViewModels.PurchaseOrder;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IPurchaseOrderServices _purchaseOrderServices;

        public HomeController(IPurchaseOrderServices purchaseOrderServices)
        {
            _purchaseOrderServices = purchaseOrderServices;
        }

        [HttpPost("InsertPO")]
        public async Task<int> InsertPO(CreatePurchaseOrderDto request, IValidator<CreatePurchaseOrderDto> validator, CancellationToken ct)
        {
            await validator.ValidateAndThrowAsync(request);

            return await _purchaseOrderServices.InsertPO(request, ct);
        }

        [HttpPost("InsertBulk")]
        public async Task<int> InsertBulk(CancellationToken ct) => await _purchaseOrderServices.InsertBulk(ct);

        [HttpGet("GetLastPurchaseOrder")]
        public async Task<object?> GetLastPurchaseOrder(CancellationToken ct) => await _purchaseOrderServices.GetLastPurchaseOrder(ct);

        [HttpGet("GetPurchaseOrders")]
        public async Task<object?> GetPurchaseOrders(CancellationToken ct) => await _purchaseOrderServices.GetPurchaseOrders(ct);
    }
}
