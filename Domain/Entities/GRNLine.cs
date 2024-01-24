using Domain.OwenTypes;

namespace Domain.Entities;

public class GRNLine : BaseEntity<long>
{
    #region Main Properties
    public decimal Quantity { get; set; } = default!;
    public decimal UnitPrice { get; set; } = default!;
    public decimal TaxRate { get; set; } = default!;
    public decimal Value { get; set; } = default!;
    public string UnitOfMeasure { get; set; } = default!;
    public string Category { get; set; } = default!;
    public string SubCategory { get; set; } = default!;
    public string BarCode { get; set; } = default!;
    #endregion

    #region Navigation Properties
    public GRN GRN { get; set; } = default!;
    #endregion

    #region Owen Type Properties
    public SiteDescription Site { get; set; } = default!;
    #endregion
}
