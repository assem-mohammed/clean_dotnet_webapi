namespace Domain.OwenTypes;

public class TaxDescription
{
    public TaxDescription()
    {

    }

    public TaxDescription(string taxCode, string taxName)
    {
        TaxCode = taxCode;
        TaxName = taxName;
    }

    public string TaxCode { get; set; } = default!;
    public string TaxName { get; set; } = default!;
}
