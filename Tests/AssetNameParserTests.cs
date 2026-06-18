using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

public class AssetNameParserTests
{
    private ConventionConfig _config;
    private AssetNameParser _parser;

    [SetUp]
    public void SetUp()
    {
        _config = ScriptableObject.CreateInstance<ConventionConfig>();
        
        _config.shipClasses = new List<ConventionConfig.ShipClass>
        {
            new ConventionConfig.ShipClass { code = "DST" }
        };
        
        _config.moduleTypes = new List<ConventionConfig.ModuleType>();
        _config.dropTokens = new List<string> { "Module" };
        _config.aliases = new List<ConventionConfig.AliasRule>
        {
            new ConventionConfig.AliasRule { from = "Gen", to = "Shield" }
        };
        _config.textureRules = new List<ConventionConfig.TextureRule>
        {
            new ConventionConfig.TextureRule { suffix = "_n" },
            new ConventionConfig.TextureRule { suffix = "_d" },
            new ConventionConfig.TextureRule { suffix = "_orm" }
        };
        _config.mergeNumbers = true;
        
        _parser = new AssetNameParser(_config);
    }

    [Test]
    public void Parse_BasicCase_ReturnsParsedResult()
    {
        var result = _parser.Parse("NightHarbinger_DST_Module_Gen_n.tif");
        
        Assert.AreEqual("NightHarbinger_DST", result.prefix);
        Assert.AreEqual("_n", result.suffix);
        Assert.AreEqual(1, result.tokens.Count);
        Assert.AreEqual("Shield", result.tokens[0]);
    }
    
    [Test]
    public void Parse_MergesNumber_ReturnsMergedToken()
    {
        var result = _parser.Parse("NightHarbinger_DST_Turret_1_d.png");
        
        Assert.AreEqual("NightHarbinger_DST", result.prefix);
        Assert.AreEqual("_d", result.suffix);
        Assert.AreEqual(1, result.tokens.Count);
        Assert.AreEqual("Turret1", result.tokens[0]);
    }
    
    [Test]
    public void Parse_OrmSuffix_ReturnsCorrectSuffix()
    {
        var result = _parser.Parse("NightHarbinger_DST_Sensor_orm.png");
        
        Assert.AreEqual("NightHarbinger_DST", result.prefix);
        Assert.AreEqual("_orm", result.suffix);
        Assert.AreEqual(1, result.tokens.Count);
        Assert.AreEqual("Sensor", result.tokens[0]);
    }
}
