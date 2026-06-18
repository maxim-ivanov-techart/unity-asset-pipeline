using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Pipeline/Convention Config")]
public class ConventionConfig : ScriptableObject
{
    [Serializable]
    public struct ShipClass
    {
        public string code;
    }
    
    [Serializable]
    public struct ModuleType
    {
        public string token;
    }

    [Serializable]
    public struct AliasRule
    {
        public string from;
        public string to;
    }
    
    [Serializable]
    public struct TextureRule
    {
        public string suffix;
    }
    
    public List<ShipClass>  shipClasses;
    public List<ModuleType> moduleTypes;
    public List<string> dropTokens;
    public List<AliasRule> aliases;
    public List<TextureRule> textureRules;
    public bool mergeNumbers;
}
