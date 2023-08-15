using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public struct UnitInfo
{
    public string unitId;

    [JsonConverter(typeof(StringEnumConverter))]
    public UnitRarity rarity;
    public float atk;
    public float range;
    public float coolTime;
    public ProjectileInfo projectileInfo;


    public float CalculateRange()
    {
        return range + range + 1;
    }

}