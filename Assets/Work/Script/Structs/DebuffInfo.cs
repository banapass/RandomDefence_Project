using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
[System.Serializable]
public struct DebuffInfo
{
    [JsonConverter(typeof(StringEnumConverter))]
    public DebuffType debuffType;
    public float duration;
    public float intensity;
    public float chanceOfDebuff;

    public bool TryApplyDebuff()
    {
        return Random.value < chanceOfDebuff;
    }

}