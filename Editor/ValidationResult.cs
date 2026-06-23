public enum ValidationStatus
{
    OK,
    Fixable,
    NeedsHuman
}

public class ValidationResult
{
    public ValidationStatus status;
    public string originalName;
    public string canonicalName;
    public string message;
}
