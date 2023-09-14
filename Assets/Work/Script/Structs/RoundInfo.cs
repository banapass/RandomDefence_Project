using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

[System.Serializable]
public struct RoundInfo
{
    public string monsterId;
    [JsonConverter(typeof(StringEnumConverter))]
    public MonsterType type;
    public MonsterStatInfo monsterStatInfo;
    public int spawnCount;
}