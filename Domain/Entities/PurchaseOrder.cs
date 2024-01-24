using Domain.OwenTypes;

namespace Domain.Entities;

public class PurchaseOrder : BaseEntity<string>
{
    public string ApprovalStatus { get; set; } = default!;
    public DateTime Date { get; set; } = default!;
    public decimal TotalValue { get; set; } = default!;

    #region Owen Type Properties
    public DocTypeDescription DocType { get; set; } = default!;
    public OrganizationDescription Organization { get; set; } = default!;

    #endregion

    #region Navigation Properties
    public string VendorCode { get; set; } = default!;
    public Vendor Vendor { get; set; } = default!;
    //public GRN GRN { get; set; } = default!;
    public ICollection<PurchaseOrderLine> Lines { get; set; } = default!;
    #endregion
}