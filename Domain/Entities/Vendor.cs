namespace Domain.Entities;

public class Vendor : BaseEntity<string>
{
    public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = default!;
}
