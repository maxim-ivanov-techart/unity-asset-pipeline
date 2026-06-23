using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;

public class AssetValidatorTests
{
    private ConventionConfig _config;
    private AssetValidator _validator;

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
        
        _validator = new AssetValidator(_config);
    }

    [Test]
    public void Validate_CanonicalName_ReturnsOK()
    {
        var result = _validator.Validate("NightHarbinger_DST_Shield_n");
        
        Assert.AreEqual(ValidationStatus.OK, result.status);
    }

    [Test]
    public void Validate_DirtyName_ReturnsFixable()
    {
        var result = _validator.Validate("NightHarbinger_DST_Module_Gen_n");
        
        Assert.AreEqual(ValidationStatus.Fixable, result.status);
        Assert.AreEqual("NightHarbinger_DST_Shield_n", result.canonicalName);
    }
    
    [Test]
    public void Validate_UnknownClass_ReturnsNeedsHuman()
    {
        var result = _validator.Validate("NightHarbinger_ABC_Turret_n");
        
        Assert.AreEqual(ValidationStatus.NeedsHuman, result.status);
    }
}
