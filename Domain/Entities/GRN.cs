using Domain.OwenTypes;

namespace Domain.Entities;

public class GRN : BaseEntity<string>
{
    #region Main Properties
    public string DeliveryNoteNumber { get; set; } = default!;
    public decimal TotalValue { get; set; }
    public DateTime Date { get; set; } = default!;
    public DateTime PostingDate { get; set; } = default!;
    public bool IsSubmitted { get; set; }
    #endregion

    #region Navigation Properties
    public string VendorCode { get; set; } = default!;
    public Vendor Vendor { get; set; } = default!;

    public string PurchaseOrderId { get; set; } = default!;
    public PurchaseOrder PurchaseOrder { get; set; } = default!;

    public ICollection<GRNLine> Lines { get; set; } = default!;

    #endregion

    #region Owen Type Properties
    public OrganizationDescription Organization { get; set; } = default!;

    #endregion
}