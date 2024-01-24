using Contracts.ViewModels.PurchaseOrder;

namespace Contracts.PurchaseOrderFeatures
{
    public interface IPurchaseOrderServices
    {
        Task<object?> GetLastPurchaseOrder(CancellationToken ct);
        Task<object?> GetPurchaseOrders(CancellationToken ct);
        Task<int> InsertBulk(CancellationToken ct);
        Task<int> InsertPO(CreatePurchaseOrderDto request, CancellationToken ct);
    }
}
