namespace Domain.OwenTypes;

public class ProductDescription
{
    public ProductDescription()
    {

    }

    public ProductDescription(string prodctCode, string prodctName)
    {
        ProdctCode = prodctCode;
        ProdctName = prodctName;
    }

    public string ProdctCode { get; set; } = default!;
    public string ProdctName { get; set; } = default!;
}
