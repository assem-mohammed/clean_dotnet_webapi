using System.Text.Json.Serialization;

namespace Contracts;

public class BasePagedModel
{
    [JsonPropertyName("start")]
    public int Start { get; set; }

    [JsonPropertyName("length")]
    public int Length { get; set; }
    
    [JsonPropertyName("search")]
    public Search? Search { get; set; }
}

public class Search
{
    [JsonPropertyName("value")]

    public string? Value { get; set; }
}
