namespace Contracts;

public class BasePagedModel
{
    public int Page { get; set; }
    public int Length { get; set; }
    public string? SearchKey { get; set; }
}
