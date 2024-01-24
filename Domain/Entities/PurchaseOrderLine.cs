using Domain.OwenTypes;

namespace Domain.Entities;

public class PurchaseOrderLine : BaseEntity<long>
{
    public string Status { get; set; } = default!;
    public string ProcurementType { get; set; } = default!;
    public string ProductStatus { get; set; } = default!;
    public string Currency { get; set; } = default!;
    public string Unit { get; set; } = default!;
    public string Street { get; set; } = default!;
    public string City { get; set; } = default!;
    public string Region { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public string Telephone { get; set; } = default!;
    public decimal Quantity { get; set; } = default!;
    public decimal UnitPrice { get; set; } = default!;
    public decimal Discount { get; set; } = default!;
    public decimal DiscountPercent { get; set; } = default!;
    public decimal ExciseDutyValue { get; set; } = default!;
    public decimal TaxPercentage { get; set; } = default!;
    public decimal VatValue { get; set; } = default!;
    public decimal TotalAmount { get; set; } = default!;
    public decimal TotalPoValue { get; set; } = default!;
    public decimal FreightValue { get; set; } = default!;
    public DateTime DeliveryDate { get; set; } = default!;
    public string ItemLine { get; set; }=default!;

    #region Owen Type Properties
    public TaxDescription Tax { get; set; } = default!;
    public SiteDescription Site { get; set; } = default!;
    public ProductDescription Product { get; set; } = default!;
    #endregion

    #region Navigation Properties
    public string PurchaseOrderId { get; set; } = default!;
    public PurchaseOrder PurchaseOrder { get; set; } = default!;
    #endregion
}
