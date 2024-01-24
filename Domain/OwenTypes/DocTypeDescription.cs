namespace Domain.OwenTypes;

public class DocTypeDescription
{
    public DocTypeDescription()
    {
    }
    public DocTypeDescription(string docTypeCode, string docTypeName)
    {
        DocTypeCode = docTypeCode;
        DocTypeName = docTypeName;
    }

    public string DocTypeCode { get; set; } = default!;
    public string DocTypeName { get; set; } = default!;
}
