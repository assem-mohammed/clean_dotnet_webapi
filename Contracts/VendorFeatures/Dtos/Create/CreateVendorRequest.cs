using Collector.Serilog.Enrichers.SensitiveInformation.Attributed;

namespace Contracts.VendorFeatures.Dtos.Create;

public class CreateVendorRequest
{
    public string Id { get; set; } = default!;
    
    [LogAsSensitive]
    public string Email { get; set; } = default!;
    
    public string Name { get; set; } = default!;
    
    public string FirstSearchTerm { get; set; } = default!;
    
    public string Language { get; set; } = default!;

    [LogAsSensitive]
    public string Phone { get; set; } = default!;
}
