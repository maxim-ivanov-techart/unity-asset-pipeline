using System.Collections.Generic;

public class ParseResult
{
    public string prefix;
    public string suffix;
    public List<string> tokens;

    public string BuildName()
    {
        string descriptor = string.Join("_",  tokens);
        return $"{prefix}_{descriptor}{suffix}";
    }
}
