using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public struct UnitRarityInfo
{
    [JsonConverter(typeof(StringEnumConverter))]
    public UnitRarity rarity;
    public float weight;
}