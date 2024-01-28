namespace Shared;

[Serializable]
public class BusinessException : Exception
{
    public List<string>? Errors { get; set; }
    public BusinessException(string errorMessage, List<string>? failures = null, Exception? innerException = null) : base(errorMessage, innerException)
    {
        Errors = failures;
    }

    public BusinessException(List<string>? failures) : base("Validation failed")
    {
        Errors = failures;
    }
}
