using UnityEditor.VersionControl;

public class AssetValidator
{
    private AssetNameParser _parser;

    public AssetValidator(ConventionConfig conventionConfig)
    {
        _parser = new AssetNameParser(conventionConfig);
    }

    public ValidationResult Validate(string fileName)
    {
        ValidationResult result = new ValidationResult();
        result.originalName = fileName;
        
        ParseResult parseResult = _parser.Parse(fileName);

        if (!parseResult.isValid)
        {
            result.status = ValidationStatus.NeedsHuman;
            result.message = parseResult.errorMessage;
            return result;
        }

        result.canonicalName = parseResult.BuildName();

        if (result.canonicalName == result.originalName)
        {
            result.status = ValidationStatus.OK;
            result.message = "Name is canonical";
        }
        else
        {
            result.status = ValidationStatus.Fixable;
            result.message = $"Can be renamed to: {result.canonicalName}";
        }
        
        return result;
        
    }
}
