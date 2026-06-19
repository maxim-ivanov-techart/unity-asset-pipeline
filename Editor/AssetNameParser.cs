using System.Collections.Generic;
using System.IO;

public class AssetNameParser
{
    private ConventionConfig _conventionConfig;
    
    public AssetNameParser(ConventionConfig conventionConfig)
    {
        _conventionConfig = conventionConfig;
    }

    public ParseResult Parse(string fileName)
    {
        ParseResult result = new ParseResult();

        string name = Path.GetFileNameWithoutExtension(fileName);

        foreach (ConventionConfig.TextureRule rule in _conventionConfig.textureRules)
        {
            if (name.EndsWith(rule.suffix))
            {
                result.suffix = rule.suffix;
                name = name.Substring(0, name.Length - rule.suffix.Length);
            }
        }
        
        result.tokens = new List<string>(name.Split('_'));
        List<string> prefixTokens = new List<string>();
        bool foundClass = false;
        
        foreach (string token in result.tokens)
        {
            prefixTokens.Add(token);
            foreach (ConventionConfig.ShipClass shipClass in _conventionConfig.shipClasses)
            {
                if (shipClass.code == token)
                {
                    foundClass = true;
                    break;
                }
            }

            if (foundClass)
            {
                break;
            }
        }
        
        result.prefix = string.Join("_", prefixTokens);
        result.tokens.RemoveRange(0, prefixTokens.Count);

        if (!foundClass)
        {
            result.isValid = false;
            result.errorMessage = "Class not found";
        }
        
        List<string> cleaned  = new List<string>();
        foreach (string token in result.tokens)
        {
            if (!_conventionConfig.dropTokens.Contains(token))
            {
                cleaned.Add(token);
            }
        }
        
        result.tokens = cleaned;

        for (int i = 0; i < result.tokens.Count; i++)
        {
            foreach (ConventionConfig.AliasRule alias in _conventionConfig.aliases)
            {
                if (result.tokens[i] == alias.from)
                {
                    result.tokens[i] = alias.to;
                    break;
                }
            }
        }

        if (_conventionConfig.mergeNumbers)
        {
            for (int i = 0; i < result.tokens.Count; i++)
            {
                if(int.TryParse(result.tokens[i], out _))
                {
                    result.tokens[i] = result.tokens[i-1] + result.tokens[i];
                    result.tokens.RemoveAt(i - 1);
                    i--;
                }
            }
        }
        
        return  result;
    }
}
