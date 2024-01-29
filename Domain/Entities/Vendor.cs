namespace Domain.Entities;

public class Vendor : BaseEntity<string>
{
    public string? LegacyVendorCode { get; set; }
    public string? Group { get; set; }
    public string? Title { get; set; }
    public string Name { get; set; } = default!;
    public string? Name2 { get; set; }
    public string? Name3 { get; set; }
    public string? Name4 { get; set; }
    public string FirstSearchTerm { get; set; } = default!;
    public string? SecondSearchTerm { get; set; }
    public string Language { get; set; } = default!;
    public string? Telephone { get; set; }
    public string Phone { get; set; } = default!;
    public string? FaxNumber { get; set; }
    public string Email { get; set; } = default!;
    public string? VatRegNumber { get; set; }
    public string? IndustryKey { get; set; }
    public string? CentralBlock { get; set; }
    public string? SSOUserId { get; set; }


    public int TotalCount { get; set; }
    public int FilterCount { get; set; }
}
